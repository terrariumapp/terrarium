//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                        
//------------------------------------------------------------------------------

namespace Terrarium.Configuration
{
    /// <summary>
    ///  This class is used to enumerate and validate
    ///  the list of 50 US States.
    /// </summary>
    public class StateSettings 
    {
        /// <summary>
        ///  Provides direct access to the state array listing
        /// </summary>
        public static string[] States = new[] {
                                                           "Alabama",              "Alaska",           "Arizona",          "Arkansas",
                                                           "California",           "Colorado",         "Connecticut",      "Delaware",
                                                           "District of Columbia", "Florida",          "Georgia",          "Hawaii",
                                                           "Idaho",                "Illinois",         "Indiana",          "Iowa",
                                                           "Kansas",               "Kentucky",         "Louisiana",        "Maine",
                                                           "Maryland",             "Massachusetts",    "Michigan",         "Minnesota",
                                                           "Mississippi",          "Missouri",         "Montana",          "Nebraska",
                                                           "Nevada",               "New Hampshire",    "New Jersey",       "New Mexico",
                                                           "New York",             "North Carolina",   "North Dakota",     "Ohio",
                                                           "Oklahoma",             "Oregon",           "Pennsylvania",     "Rhode Island",
                                                           "South Carolina",       "South Dakota",     "Tennessee",        "Texas",
                                                           "Utah",                 "Vermont",          "Virginia",         "Washington",
                                                           "West Virginia",        "Wisconsin",        "Wyoming",          "<Unknown>"
                                                       };

        /// <summary>
        ///  Validates a string value against the array
        ///  of states.  Note this looks for a direct match
        ///  and doesn't match variations or two character
        ///  state abbreviations
        /// </summary>
        /// <param name="state">A string value to match against the list of states</param>
        /// <returns>True if the string matches one of the 50 states, false otherwise.</returns>
        public static bool Validate(string state)
        {
            foreach (string s in States)
            {
                if (s == state)
                {
                    return true;
                }
            }
        
            return false;
        }
    }
}