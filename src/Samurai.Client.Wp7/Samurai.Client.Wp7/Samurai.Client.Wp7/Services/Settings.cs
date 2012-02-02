using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using SamuraiServer.Data;
using System.IO;

namespace Samurai.Client.Services
{
    public class Settings
    {
        const int SETTINGSVERSION = 1;
        private IsolatedStorageSettings SettingsStore { get; set; }
        private bool _isMute = false;
        private static volatile Settings _instance = null;

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

            if (LoadSetting<int>("Version", 0) != SETTINGSVERSION)
                RunSettingUpdate(LoadSetting<int>("Version", 0));
        }

        private void RunSettingUpdate(int currentVersion)
        {
            switch (currentVersion)
            {
                case 0:
                    if (CurrentPlayer != null)
                        return;

                    using (var iso = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        if (iso.FileExists("player.dat"))
                        {
                            using (var file = iso.OpenFile("player.dat", FileMode.Open, FileAccess.Read))
                            using (var br = new BinaryReader(file))
                            {
                                CurrentPlayer = new Player();
                                CurrentPlayer.Id = Guid.Parse(br.ReadString());
                                CurrentPlayer.Name = br.ReadString();
                                CurrentPlayer.ApiKey = br.ReadString();
                            }

                            iso.DeleteFile("player.dat");
                        }
                    }
                    SaveSetting<Player>("CurrentPlayer", CurrentPlayer);

                    SaveSetting<int>("Version", 1);
                    return;

                case 1:
                    return;

                default:
                    return;
            }
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
