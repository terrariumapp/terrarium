//------------------------------------------------------------------------------  
//      Copyright (c) Microsoft Corporation.  All rights reserved.                                                          
//------------------------------------------------------------------------------

namespace Terrarium.Configuration
{
    /// <summary>
    ///  Facilitates the creation of boolean configuration
    ///  variables using the XmlConfig class.  Since all
    ///  values are retrieved as strings, the values are
    ///  first retrieved, then cached, and finally parsed
    ///  into boolean variables.  When values can't be
    ///  parsed the class allows a default value to be set.
    /// </summary>
    public class CachedBooleanConfig
    {
        /// <summary>
        ///  Field representing the name of the setting in the config file
        /// </summary>
        private readonly string _booleanName;

        /// <summary>
        ///  Field representing the string or config representation of the setting
        /// </summary>
        private string _booleanString;

        /// <summary>
        ///  Field representing the boolean value of the setting
        /// </summary>
        private bool _booleanValue;

        /// <summary>
        ///  Initialize a new CachedBooleanConfig given the name of the configuration
        ///  setting and the initial default boolean value of false.
        /// </summary>
        /// <param name="name">Name of config file setting.</param>
        public CachedBooleanConfig(string name) : this(name, false)
        {
        }

        /// <summary>
        ///  Initialize a new CachedBooleanConfig given teh name fo the configuration
        ///  setting and an initial default value.
        /// </summary>
        /// <param name="name">Name of the config file setting.</param>
        /// <param name="defaultValue">Initial default value.</param>
        public CachedBooleanConfig(string name, bool defaultValue)
        {
            _booleanName = name;
            _booleanValue = defaultValue;
        }

        /// <summary>
        ///  Used to retrieve the value of this boolean config setting.
        ///  If the value has not been processed before then it is 
        ///  retrieved and processed, else it is returned from cache.
        /// </summary>
        /// <returns>True/False value of the current setting.</returns>
        public bool Getter()
        {
            if (_booleanString == null)
            {
                _booleanString = GameConfig.GetSetting(_booleanName);

                try
                {
                    _booleanValue = bool.Parse(_booleanString);
                }
                catch
                {
                    // By default it will be set to it's default value
                    // or whatever the value was before the config
                    // setting was changed
                }
            }

            return _booleanValue;
        }

        /// <summary>
        ///  Used to set the value of a setting.  This method will create
        ///  the setting if it doesn't exist, or update it otherwise.
        /// </summary>
        /// <param name="value">New True/False value for the setting.</param>
        public void Setter(bool value)
        {
            _booleanString = value.ToString();
            _booleanValue = value;
            GameConfig.SetSetting(_booleanName, _booleanString);
        }
    }
}