using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using SamuraiServer.Data;

namespace Samurai.Client.Wp7
{
    internal class Settings
    {
        // Version of settings file so we can migrate data if needed
        const int SETTINGSVERSION = 1;

        // Settings loaded from isolated storage
        public IsolatedStorageSettings SettingsStore { get; set; }

        // Lets make it a singleton
        private static volatile Settings _instance = null;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(Settings))
                    {
                        _instance = new Settings();
                        _instance.LoadSettings();
                    }
                }
                return _instance;
            }
        }

        public Settings()
        {
            SettingsStore = IsolatedStorageSettings.ApplicationSettings;

            if (LoadSetting<int>("Version", 0) != SETTINGSVERSION)
                RunSettingMigrate(LoadSetting<int>("Version", 0));
        }

        /// <summary>
        /// Updates settings files to the current version and migrate data if needed
        /// </summary>
        /// <param name="fileVersion">Version that current settings file is at</param>
        private void RunSettingMigrate(int fileVersion)
        {
            switch (fileVersion)
            {
                case 0:
                    if (CurrentPlayer != null)
                        return;

                    using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        // Load old player.dat and save it to new settings
                        if (iso.FileExists("player.dat"))
                        {
                            using (var file = iso.OpenFile("player.dat", FileMode.Open, FileAccess.Read))
                            using (var br = new BinaryReader(file))
                            {
                                var player = new Player();
                                player.Id = Guid.Parse(br.ReadString());
                                player.Name = br.ReadString();
                                player.ApiKey = br.ReadString();
                                CurrentPlayer = player;
                            }
                            iso.DeleteFile("player.dat");
                        }
                    }

                    UpdateSetting<bool>("IsMute", LoadSetting<bool>("Sound", false));

                    SettingsStore.Remove("Sound");
                    SettingsStore.Save();

                    UpdateSetting<int>("Version", 1);
                    return;

                case 1:
                    return;

                default:
                    return;
            }
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

        /// <summary>
        /// Loads all the settings and stores it in the settings class
        /// </summary>
        private void LoadSettings()
        {
            _currentPlayer = LoadSetting<Player>("CurrentPlayer", null);
            _isMute = LoadSetting<bool>("IsMute", _isMute);
        }

        // Settings
        private bool _isMute = false;
        private Player _currentPlayer;

        public Player CurrentPlayer 
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    UpdateSetting<Player>("CurrentPlayer", _currentPlayer);
                }
            }
        }

        public bool IsMute
        {
            get { return _isMute; }
            set
            {
                if (_isMute != value)
                {
                    _isMute = value;
                    UpdateSetting<bool>("IsMute", _isMute); 
                }
            }
        }
        
    }
}
