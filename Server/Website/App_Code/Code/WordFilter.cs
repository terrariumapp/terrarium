//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------


using System;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Caching;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace Terrarium.Server {
    /*
        Class:      WordFilterSettings
        Purpose:    This class is capable of turning a line delimited
        list of keywords into a series of regular expressions which
        can be used to filter words out of a string.
    */
    public sealed class WordFilterSettings {

		private WordFilterSettings() {}

		private static string[] _wordListArray;
		private static Regex[] _wordListPattern;

        /*
            Property:   WordListArray
            Purpose:    Returns an array of words based on the given
            word list file.  This function caches the result and will
            only build the array on the first call.  The Word List File
            used by the official Terrarium has some strangely formatted
            constructs which are removed by the trim code.
        */
		public static string[] WordListArray {
			get {
				if ( _wordListArray == null ) {
					lock(typeof(WordFilterSettings)) {
						try {
							using(StreamReader sr = new StreamReader(ServerSettings.WordListFile)) {
								ArrayList wordList = new ArrayList();
								string line = null;
								while((line = sr.ReadLine()) != null) {
									if ( line.IndexOf("###") > -1 ) {
										line = line.Substring(0, line.IndexOf("###"));
									}
									line = line.Trim();
									if ( line.StartsWith("!") ) {
										line = line.Substring(1);
									}
									line = line.Trim();
									if ( line != string.Empty ) {
										wordList.Add(Regex.Escape(line));
									}
								}
								sr.Close();

								_wordListArray = (string[]) wordList.ToArray(typeof(string));
							}
						}
                        catch(Exception e) {
							InstallerInfo.WriteEventLog("Policheck", e.ToString());
							_wordListArray = new string[0];
						}
					}
				}
            
				return _wordListArray;
			}
		}
    
        /*
            Property:   WordListPattern
            Purpose:    Returns an array of regular expression objects
            each of which represents 200 of the keywords in the list or
            the remainder thereof.  This property is cached and will only
            generate the array of regular expressions the first time through
            and then return the same array each time thereafter.
        */
		public static Regex[] WordListPattern {
			get {
				if ( _wordListPattern == null ) {
					lock(typeof(WordFilterSettings)) {
						_wordListPattern = new Regex[(WordListArray.Length / 200) + 1];
						for(int i = 0; i < (WordListArray.Length / 200) + 1; i++) {
							if ( i < (WordListArray.Length / 200) ) {
								_wordListPattern[i] = new Regex(
									"(?:(" +
									string.Join("|", WordListArray, i * 200, 200) + // Uses a StringBuilder internally
									"))+",
									RegexOptions.Compiled | RegexOptions.IgnoreCase
									);
							}
                            else {
								_wordListPattern[i] = new Regex(
									"(?:(" +
									string.Join("|", WordListArray, i * 200, WordListArray.Length - (i*200)) + // Uses a StringBuilder internally
									"))+",
									RegexOptions.Compiled | RegexOptions.IgnoreCase
									);
							}
						}
					}
				}
            
				return _wordListPattern;
			}
		}
	}

    /*
        Class:      WordFilter
        Purpose:    This class contains a single method which uses the
        PoliSettings class to search for keywords within a string.
    */
    public sealed class WordFilter {

		private WordFilter() {}

        /*
            Method:     RunQuickWordFilter
            Purpose:    This method gets the array of regular expressions
            from the PoliSettings class.  It then tests the string against
            each expression looking for a match.  If a match is found the
            function returns true, else the function returns false.
        */
		public static bool RunQuickWordFilter(string text)
		{
			try 
			{
				if ( WordFilterSettings.WordListArray.Length > 0 ) 
				{
					foreach(Regex r in WordFilterSettings.WordListPattern) 
					{
						if ( r.IsMatch(text) ) 
						{
							return true;
						}
					}
				}
			}
			catch(Exception e) 
			{
				InstallerInfo.WriteEventLog("WordFilter", e.ToString());
			}
			return false;
		}

	}
}