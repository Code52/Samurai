using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;

namespace SamuraiServer.Data
{
    public class Settings
    {
        public IsolatedStorageSettings SettingsStore { get; set; }

        public Player CurrentPlayer { get; private set; }
        public bool IsMute 
        { 
            get { return _isMute;}
            set 
            { 
                _isMute = value;
                SaveSetting<bool>("IsMute", _isMute);
            } 
        }

        private bool _isMute = false;

        private static volatile Settings _instance = null;
        public static Settings Instance
        {
            get {
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
            set { _instance = value; }
        }

        public Settings()
        {
            SettingsStore = IsolatedStorageSettings.ApplicationSettings;
        }

        public void SetPlayer(Player player)
        {
            CurrentPlayer = player;
            SaveSetting<Player>("CurrentPlayer", CurrentPlayer);
        }

        private void SaveSetting<T>(string setting, T data)
        {
            SettingsStore[setting] = data;
            SettingsStore.Save();
        }

        private void LoadSettings()
        {
            CurrentPlayer = LoadSetting<Player>("CurrentPlayer", null);
            _isMute = LoadSetting<bool>("IsMute", _isMute);
        }

        private T LoadSetting<T>(string setting, T defaultValue)
        {
            T settingData;

            try
            {
                settingData = (T)SettingsStore[setting];
            }
            catch (KeyNotFoundException)
            {
                return defaultValue;
            }

            return settingData;
        }

    }
}
