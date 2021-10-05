// file=""MenuController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Settings;
using Game.Design.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        [SerializeField]
        [Tooltip("Levels to start scrolling")]
        private int scrollMinimum;
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
            ShopLoad();
            foreach (GameObject cointText in textCoin)
            {
                cointText.GetComponent<TextMeshProUGUI>().text = settingsController.Coins.ToString();
            }

            GetComponent<UI.UIController>().enabled = true;
        }
        #endregion base methods

        #region custom methods
        #region SETTINGS
        private static void SettingsInit()
        {
            settingsController = new SettingsController();
            settingsController.InitSettings();
        }
        #endregion SETTINGS

        #region SHOP
        /// <summary>
        /// Loads all material for possible acquisition
        /// </summary>
        public void ShopLoad()
        {
            string current_shop = settingsController.currentShop;
            string[] allIDNames = current_shop.Split(';');
            List<int> allIDs = new List<int>();
            GameObject shopBtt = Resources.Load<GameObject>("Prefabs/UI/ShopBtt");
            var allTextures = Resources.LoadAll("UI/Box", typeof(Sprite));
            Color color = new Color(0.4f, 0.4f, 0.4f);

            for (int i = 0; i < allIDNames.Length; i++)
                if (!string.IsNullOrEmpty(allIDNames[i]))
                    allIDs.Add(int.Parse(allIDNames[i]));

            // Enable Scrollbar
            if (allTextures.Length > 9)
                shopContainerGO.transform.parent.GetComponent<ScrollRect>().enabled = true;

            RectTransform tempGO = null;
            for (int i = 0; i < allTextures.Length; i++)
            {
                tempGO = Instantiate(shopBtt, shopContainerGO.transform).GetComponent<RectTransform>();
                //tempGO.anchoredPosition = new Vector2(162 + 238 * (x - 1), current_Y);
                tempGO.transform.GetChild(1).GetComponent<Image>().sprite = (Sprite)allTextures[i];
                tempGO.name = i.ToString();

                // Need to create an int to fix i value, solved in stackOverflow
                GameObject spcialGO = tempGO.gameObject;
                if (!allIDs.Contains(i))
                {
                    tempGO.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
                    tempGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = shopCost.ToString();


                    tempGO.transform.GetChild(1).GetComponent<Image>().color = color;

                    int i_value = i;
                    tempGO.transform.GetChild(1).GetComponent<Button>()?.onClick.AddListener(() => { GetComponent<UI.UIController>().OnShopItemBuyClick(spcialGO); });
                }
                else
                    tempGO.transform.GetChild(1).GetComponent<Button>()?.onClick.AddListener(() => { GetComponent<UI.UIController>().OnShopItemChangeClick(spcialGO); });

                if (settingsController.currentBoxID == i)
                    tempGO.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        #endregion SHOP
        #endregion custom methods
    }
}
