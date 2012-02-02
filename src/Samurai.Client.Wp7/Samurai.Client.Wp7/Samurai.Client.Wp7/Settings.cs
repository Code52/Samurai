using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace Samurai.Client.Wp7
{
    internal class Settings
    {
        // Settings loaded from isolated storage
        public IsolatedStorageSettings SettingsStore { get; set; }

        public Settings()
        {
            SettingsStore = IsolatedStorageSettings.ApplicationSettings;
        }

        /// <summary>
        /// Update a setting and save it to IsolatedStorageSettings
        /// </summary>
        /// <typeparam name="T">Type to be saved</typeparam>
        /// <param name="key">Key for the setting to use in store</param>
        /// <param name="setting">The updated setting to save</param>
        protected void UpdateSetting<T>(string key, T setting)
        {
            SettingsStore[key] = setting;
            SettingsStore.Save();
        }

        /// <summary>
        /// Load setting from IsolatedStorageSettings
        /// </summary>
        /// <typeparam name="T">Type to be loaded</typeparam>
        /// <param name="key">Key for setting</param>
        /// <param name="defaultValue">Default value if setting doesn't exist</param>
        /// <returns>The last saved or default setting</returns>
        protected T LoadSetting<T>(string key, T defaultValue)
        {
            T setting;

            try
            {
                setting = (T)SettingsStore[key];
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }

            return setting;
        }

        public bool Sound { get { return LoadSetting("Sound", false); } set { UpdateSetting("Sound", value); } }
    }
}
