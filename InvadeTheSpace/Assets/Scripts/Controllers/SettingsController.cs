// file=""SettingsController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using UnityEngine;
#endregion usings

namespace Game.Controller.Settings
{
    public class SettingsController
    {
        #region internal variables
        private int _coin = 0;
        private int _sound = 1;
        private int _music = 1;
        private int _vibration = 1;
        private int _boxID = 0;
        private string _Shop = "";
        public enum settingsTrigger
        {
            On = 1,
            Off = 2,
        }
        #endregion internal variables

        #region get/set variabels
        /// <summary> sound On/Off </summary>
        public int coins
        {
            get { return _coin; }
            set
            {
                _coin = value;
                PlayerPrefs.SetInt("COIN", _coin);
            }
        }
        /// <summary> sound On/Off </summary>
        public int soundTrigger
        {
            get { return _sound; }
            set
            {
                _sound = value;
                PlayerPrefs.SetInt("SOUND", _sound);
            }
        }
        /// <summary> music On/Off </summary>
        public int musicTrigger
        {
            get { return _music; }
            set
            {
                _music = value;
                PlayerPrefs.SetInt("MUSIC", _music);
            }
        }
        /// <summary> vibartion On/Off </summary>
        public int vibrationTrigger
        {
            get { return _vibration; }
            set
            {
                _vibration = value;
                PlayerPrefs.SetInt("VIBRATION", _vibration);
            }
        }
        /// <summary> vibartion On/Off </summary>
        public int currentBoxID
        {
            get { return _boxID; }
            set
            {
                _boxID = value;
                PlayerPrefs.SetInt("BOX", _boxID);
            }
        }
        /// <summary> shop Materials owned </summary>
        public string currentShop
        {
            get { return _Shop; }
            set
            {
                _Shop = value;
                PlayerPrefs.SetString("SHOP", _Shop);
            }
        }
        #endregion get/set variables

        #region custom methods
        public void InitSettings()
        {
            // if possible try load
            // else set default

            if (PlayerPrefs.GetInt("COIN") > 0)
                _coin = PlayerPrefs.GetInt("COIN");
            else
                coins = 0;

            if (PlayerPrefs.GetInt("SOUND") > 0)
                _sound = PlayerPrefs.GetInt("SOUND");
            else
                soundTrigger = (int)settingsTrigger.On;

            if (PlayerPrefs.GetInt("MUSIC") > 0)
                _music = PlayerPrefs.GetInt("MUSIC");
            else
                musicTrigger = (int)settingsTrigger.On;

            if (PlayerPrefs.GetInt("VIBRATION") > 0)
                _vibration = PlayerPrefs.GetInt("VIBRATION");
            else
                vibrationTrigger = (int)settingsTrigger.On;

            if (PlayerPrefs.GetInt("BOX") > 0)
                _boxID = PlayerPrefs.GetInt("BOX");
            else
                currentBoxID = 0;

            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("SHOP")))
                _Shop = PlayerPrefs.GetString("SHOP");
            else
                currentShop = "0;";
        }
        #endregion custom methods
    }
}
