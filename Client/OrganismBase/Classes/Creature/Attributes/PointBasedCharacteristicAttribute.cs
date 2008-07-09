//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase 
{
    /// <internal/>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class PointBasedCharacteristicAttribute : System.Attribute
    {
        /// <summary>
        ///  Number of points given to the attribute
        /// </summary>
        int appliedPoints;
        
        /// <summary>
        ///  Number of points originally assigned to the attribute
        /// </summary>
        int originalPoints;
        
        /// <summary>
        ///  Maximum number of points able to be assigned to this attribute.
        /// </summary>
        int maximumValue;

        /// <internal/>
        protected PointBasedCharacteristicAttribute(int points, int maximumValue)
        {
            this.originalPoints = points;
            this.maximumValue = maximumValue;

            if (points > EngineSettings.MaxAvailableCharacteristicPoints || points < 0)
            {
                throw new TooManyPointsOnOneCharacteristicException();
            }

            this.appliedPoints = points;
        }

        /// <summary>
        ///  This method formats an attribute warning to users when attributes have been used
        ///  in an invalid manner or in a way that is not beneficial.  If points are not alotted
        ///  in the proper increments the GetPointsNotUsedWarning method is called to indicate
        ///  how many points have been wasted.
        /// </summary>
        /// <returns>A warning message of wasted points, or an empty string if none wasted</returns>
        public string GetWarnings()
        {
            // If adding a single point doesn't increase an attribute, make sure the user doesn't add points
            // that aren't being used
            if (((double) maximumValue / (double) EngineSettings.MaxAvailableCharacteristicPoints) < (double) 1)
            {
                // Find out how many points it takes to increment the characteristic by one
                // and make sure the user is applying points in that increment
                int pointsToIncrement = EngineSettings.MaxAvailableCharacteristicPoints / maximumValue;
                if (originalPoints % pointsToIncrement != 0)
                {
                    return "Points applied to '" + this.GetType().Name + "' should be in increments of " + pointsToIncrement.ToString() + ".  Anything else is wasted.";
                }
            }

            return string.Empty;
        }

        /// <internal/>
        public int Points
        {
            get
            {
                return appliedPoints;
            }
        }

        // Percent of the maximum of this characteristic that you get based on the points you selected
        internal float PercentOfMaximum
        {
            get
            {
                return (float) Points / (float) EngineSettings.MaxAvailableCharacteristicPoints;
            }
        }
    }
}