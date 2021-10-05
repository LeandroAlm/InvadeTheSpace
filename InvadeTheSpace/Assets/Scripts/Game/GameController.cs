// file=""GameController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Player;
using Game.Design.Juntion;
using Game.Design.Level;
using Game.Loader.Level;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#endregion usings

namespace Game.Controller.Game
{
    public class GameController : MonoBehaviour
    {
        #region vars
        [SerializeField]
        [Tooltip("Player object reference")]
        public GameObject player;
        [SerializeField]
        [Tooltip("Settings button object reference")]
        private GameObject settingsBtt;
        [SerializeField]
        [Tooltip("Lose Panel object reference")]
        private GameObject losePanel;
        #endregion vars

        #region internal vars
        private LevelDesign currentLevelDesign;
        #endregion internal vars

        #region custom methods
        public void InitLevel()
        {
            // Clear map
            Transform map = GameObject.Find("Map").transform;
            for (int i = 3; i < map.childCount; i++)
            {
                Destroy(map.GetChild(i).gameObject);
            }

            Material playersMat = Resources.Load<Material>("Materials/Mat_Box_" + Menu.MenuController.settingsController.currentBoxID);
            LevelLoader levelLoader = gameObject.AddComponent<LevelLoader>();

            if (Menu.MenuController.settingsController.musicTrigger == (int)Settings.SettingsController.settingsTrigger.On)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.clip = Resources.Load<AudioClip>("Audio/musicGame") as AudioClip;
                audio.volume = 0.025f;
                audio.loop = true;
                audio.Play();
            }

            settingsBtt.SetActive(false);
            player.GetComponent<PlayerController>().PlayerInit(playersMat);
        }
        
        public void DisableSettingsButton()
        {
            settingsBtt.SetActive(false);
        }
        #endregion custom methods
    }
}

