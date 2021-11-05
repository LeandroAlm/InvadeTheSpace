// file=""GameController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Player;
using Game.Controller.UI;
using Game.Data.Game;
using Game.Data.Game.SO;
using Game.Data.Ship;
using Game.Loader.Plataform;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion usings

namespace Game.Controller.Game
{
    public class GameController : MonoBehaviour
    {
        #region vars
        [SerializeField]
        [Tooltip("Player object reference")]
        private PlayerController playerController;
        [SerializeField]
        [Tooltip("Settings button object reference")]
        private GameObject settingsBtt;
        [SerializeField]
        [Tooltip("Lose Panel object reference")]
        private GameObject losePanel;
        [SerializeField]
        [Tooltip("Map object reference")]
        internal GameObject map;
        [SerializeField]
        [Tooltip("Number of plataforms in game")]
        private int plataformShowingAtTime;
        [SerializeField]
        [Tooltip("Z position when plataform can be destroyed")]
        internal int plataformEndPos;
        [SerializeField]
        [Tooltip("Game data scriptableObject with info")]
        internal GameDataSO gameDataSO;
        #endregion vars

        #region internal vars
        [HideInInspector]public GameObject explosionParticle;
        internal GameData gameData;

        internal Vector3 plataformLastPos;
        internal JunctionLoader junctionLoader;
        #endregion internal vars

        #region base methods
        private void Start()
        {
            junctionLoader = new JunctionLoader();
            junctionLoader.Init(this);

            gameData = new GameData(gameObject.GetComponent<UIController>(), gameDataSO);

            explosionParticle = Resources.Load("GFX/Explosion", typeof(GameObject)) as GameObject;

            Init();
        }
        #endregion base methods

        #region custom methods
        public void Init()
        {
            gameData.plataforms = 0;
            
            gameData.coins = 0;
            gameData.speed = 2.0f;
            gameData.currentSpeed = 0.0f;

            foreach (Transform child in map.transform)
            {
                Destroy(child.gameObject);
            }

            InstanciateStartingMap();
        }

        public void DisableSettingsButton()
        {
            settingsBtt.SetActive(false);
        }

        /// <summary>
        /// Controls map movement
        /// </summary>
        /// <param name="a_IsPlaying">status is playing</param>
        public void OnPlayerStatusChange(bool a_IsPlaying)
        {
            if (a_IsPlaying)
                gameData.currentSpeed = gameData.speed;
            else
                gameData.currentSpeed = 0.0f;
        }

        /// <summary>
        /// Instanciate the starting map
        /// </summary>
        private void InstanciateStartingMap()
        {
            junctionLoader.LoadPlaraform(Vector3.forward * -1 * 3, 1, true, false, false, false);

            for (int i = 0; i <= plataformShowingAtTime-2; i++)
            {
                if (i <= 2)
                    junctionLoader.LoadPlaraform(Vector3.forward * i * 3, 1, false, false, false, false);
                else
                    junctionLoader.LoadPlaraform(Vector3.forward * i * 3);
            }

            plataformLastPos = Vector3.forward * (plataformShowingAtTime-2) * 3;
        }
        #endregion custom methods
    }
}

