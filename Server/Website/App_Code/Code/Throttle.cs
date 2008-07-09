//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Web;
using System.Web.Util;
using System.Web.UI;
using System.Web.Caching;

namespace Terrarium.Server {
    /*
        Class:      Throttle
        Purpose:    Whenever a user is accessing a shared resource and
        a time limit needs to be placed on the duration between each
        access a throttle is used.  Each throttle is assigned a user (ip),
        the name of the throttle, the max number of access a user has
        in a given time limit, and a date at which the throttle can be removed.
    */
	public class Throttle {
		private static Hashtable throttledUsers = new Hashtable();
    
        /*
            Method:     Throttled
            Purpose:    Returns whether or not a given users is already
            throttled.  True if they are throttled, false if they may
            still access the resource.
        */
		public static bool Throttled(string user, string throttle) {
			try {
				lock(typeof(Throttle)) {
					Hashtable throttles = (Hashtable) throttledUsers[user];
					if ( throttles == null ) {
						return false;
					}
                
					ThrottleData td = (ThrottleData) throttles[throttle];
					if ( td == null ) {
						return false;
					}
                
					if ( td.cur == td.max ) {
						return true;
					}
				}
			}
            catch {
            }
    
			return false;
		}
    
        /*
            Method:     AddThrottle
            Purpose:    Adds a brand new throttle or increments the existing throttle
            value.  Returns true if the throttle is added successfully and the user
            is allowed to access the resource.  Returns false if the user is throttled
            and no update is performed.  You should always check using the Throttled
            function first though.
        */
		public static bool AddThrottle(string user, string throttle, int max, DateTime removeThrottle) {
			try {
				lock(typeof(Throttle)) {
					Hashtable throttles = (Hashtable) throttledUsers[user];
					ThrottleData td;

					if ( throttles == null ) {
						throttles = new Hashtable();
						throttledUsers[user] = throttles;
					}

					td = (ThrottleData) throttles[throttle];
					if ( td == null ) {
						td = new ThrottleData();
						td.max = max;
						throttles[throttle] = td;
					}

					if ( td.cur == td.max ) {
						return false;
					}

					td.cur++;
					HttpContext.Current.Cache.Insert(
						user + ":" + throttle + ":" + removeThrottle.ToString(),
						td,
						null,
						removeThrottle,
						Cache.NoSlidingExpiration,
						CacheItemPriority.Normal,
						new CacheItemRemovedCallback(RemoveThrottleCallback)
                    );

					return true;
				}
			}
            catch {
            }
        
			return false;
		}
    
        /*
            Method:     RemoveThrottleCallback
            Purpose:    Callback function used to decrement the throttle count
            of a given user accessing a given throttled resource.
        */
		public static void RemoveThrottleCallback(string key, object value, CacheItemRemovedReason reason) {
			try {
				lock(typeof(Throttle)) {
					ThrottleData td = (ThrottleData) value;
					td.cur--;
				}
			} catch(Exception){}
		}
	}

    /*
        Class:      ThrottleData
        Purpose:    This class holds information about a throttle.
        It holds the max allowed throttle count set by the AddThrottle
        function, and the cur throttle count which is incremented by
        AddThrottle and decremented by RemoteThrottleCallback.
    */
	public class ThrottleData {
		public int max;
		public int cur;
	}
}