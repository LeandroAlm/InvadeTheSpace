// file=""GameController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Player;
using Game.Controller.UI;
using Game.Data.Ship;
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
        private GameObject map;
        [SerializeField]
        [Tooltip("Number of plataforms in game")]
        private int plataformShowingAtTime;
        [SerializeField]
        [Tooltip("Z position when plataform can be destroyed")]
        internal int plataformEndPos;
        #endregion vars

        #region internal vars
        private List<GameObject> plataformGO;
        internal float mapCurrentSpeed;
        private float mapSpeed;
        private int plataformsCount;
        private Vector3 plataformLastPos;
        #endregion internal vars

        #region base methods
        private void Start()
        {
            plataformsCount = 0;
            mapSpeed = 2.0f;
            mapCurrentSpeed = 0.0f;

            var allPlataforms = Resources.LoadAll("Map/Plataforms/", typeof(GameObject));
            plataformGO = new List<GameObject>();

            foreach (Object plataform in allPlataforms)
            {
                plataformGO.Add(plataform as GameObject);
            }

            InstanciateStartingMap();
        }
        #endregion base methods

        #region custom methods
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
                mapCurrentSpeed = mapSpeed;
            else
                mapCurrentSpeed = 0.0f;
        }

        /// <summary>
        /// Instanciate the starting map
        /// </summary>
        private void InstanciateStartingMap()
        {
            for (int i = -1; i <= plataformShowingAtTime-2; i++)
            {
                InstanciateNewPlatform(Vector3.forward * i * 3);
            }

            plataformLastPos = Vector3.forward * (plataformShowingAtTime-2) * 3;
        }

        /// <summary>
        /// Instanciate single plataform
        /// </summary>
        public void InstanciateNewPlatform(Vector3 a_PlataformPos, bool isLastPos = false)
        {
            GameObject go = Instantiate(plataformGO.First(), map.transform);
            go.GetComponent<PlataformController>().gameController = this;

            if (isLastPos)
                go.transform.localPosition = plataformLastPos;
            else
                go.transform.localPosition = a_PlataformPos;

            plataformsCount++;
        }
        #endregion custom methods
    }
}

