// file=""GameController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
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

        #region custom methods
        public void DisableSettingsButton()
        {
            settingsBtt.SetActive(false);
        }
        #endregion custom methods
    }
}

