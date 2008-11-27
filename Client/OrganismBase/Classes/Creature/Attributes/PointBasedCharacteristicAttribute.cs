//------------------------------------------------------------------------------
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                             
//------------------------------------------------------------------------------

using System;

namespace OrganismBase
{
    /// <internal/>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public abstract class PointBasedCharacteristicAttribute : Attribute
    {
        /// <summary>
        ///  Maximum number of points able to be assigned to this attribute.
        /// </summary>
        private readonly int _maximumValue;

        /// <summary>
        ///  Number of points originally assigned to the attribute
        /// </summary>
        private readonly int _originalPoints;

        /// <internal/>
        protected PointBasedCharacteristicAttribute(int points, int maximumValue)
        {
            _originalPoints = points;
            _maximumValue = maximumValue;

            if (points > EngineSettings.MaxAvailableCharacteristicPoints || points < 0)
            {
                throw new TooManyPointsOnOneCharacteristicException();
            }

            Points = points;
        }

        ///<summary>
        ///</summary>
        public int Points { get; private set; }

        // Percent of the maximum of this characteristic that you get based on the points you selected
        internal float PercentOfMaximum
        {
            get { return Points/(float) EngineSettings.MaxAvailableCharacteristicPoints; }
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
            if ((_maximumValue/(double) EngineSettings.MaxAvailableCharacteristicPoints) < 1)
            {
                // Find out how many points it takes to increment the characteristic by one
                // and make sure the user is applying points in that increment
                var pointsToIncrement = EngineSettings.MaxAvailableCharacteristicPoints/_maximumValue;
                if (_originalPoints%pointsToIncrement != 0)
                {
                    return "Points applied to '" + GetType().Name + "' should be in increments of " + pointsToIncrement +
                           ".  Anything else is wasted.";
                }
            }

            return string.Empty;
        }
    }
}