// file=""MenuController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Settings;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#endregion usings

namespace Game.Controller.Menu
{
    public class MenuController : MonoBehaviour
    {
        #region variables
        [SerializeField]
        [Tooltip("3d Camera reference")]
        private Camera gameCamera;
        [SerializeField]
        [Tooltip("Coin text object reference")]
        public GameObject[] textCoin;
        [SerializeField]
        [Tooltip("Shop container object reference")]
        private GameObject shopContainerGO;
        [SerializeField]
        [Tooltip("Cost of new materials")]
        public int shopCost;
        #endregion variables

        #region internal variables
        private static SettingsController _settingsController;
        public static SettingsController settingsController
        {
            get 
            { 
                if (_settingsController == null)
                    SettingsInit();

                return _settingsController;
            }
            set { _settingsController = new SettingsController(); }
        }
        #endregion internal variables

        #region base methods
        private void Awake()
        {
            gameCamera.gameObject.SetActive(false);
            SettingsInit();

            foreach (GameObject cointText in textCoin)
            {
                cointText.GetComponent<TextMeshProUGUI>().text = settingsController.Coins.ToString();
            }

            GetComponent<UI.UIController>().enabled = true;
        }
        #endregion base methods

        #region custom methods
        private static void SettingsInit()
        {
            settingsController = new SettingsController();
            settingsController.InitSettings();
        }
        #endregion custom methods
    }
}
