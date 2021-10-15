// file=""UIController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Settings;
using Game.Controller.Menu;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Game.Controller.Player;
using System.Collections.Generic;
using System.Linq;
#endregion usings

namespace Game.Controller.UI
{
    public class UIController : MonoBehaviour
    {
        #region variables
        [SerializeField]
        [Tooltip("Menu UI object reference")]
        private GameObject menuUIObject;
        [SerializeField]
        [Tooltip("Game UI object reference")]
        public GameObject gameUIObject;
        [SerializeField]
        [Tooltip("Settings panel reference")]
        private GameObject settingsPanel;
        [SerializeField]
        [Tooltip("Games panels poarent refence")]
        private GameObject gamePanels;
        [SerializeField]
        [Tooltip("Shop coins object reference")]
        private GameObject shopCoinGO;
        [SerializeField]
        [Tooltip("3D camera reference")]
        private Camera gameCamera;
        [SerializeField]
        [Tooltip("UI Camera reference")]
        public Camera uiCamera;
        [SerializeField]
        [Tooltip("Play button reference")]
        private GameObject gamePlayBtt;
        [SerializeField]
        [Tooltip("Top UI reference")]
        private GameObject gameTopUI;
        [SerializeField]
        [Tooltip("Bottom UI reference")]
        private GameObject gameBottomUI;
        [SerializeField]
        [Tooltip("Sound button object, in settings, reference")]
        private GameObject menuSettingsSound;
        [SerializeField]
        [Tooltip("Music button object, in settings, reference")]
        private GameObject menuSettingsMusic;
        [SerializeField]
        [Tooltip("Vibration button object, in settings, reference")]
        private GameObject menuSettingsVibra;
        [SerializeField]
        [Tooltip("Player object, reference")]
        private PlayerController player;
        [SerializeField]
        [Tooltip("Game shoot button standard color")]
        private Color32 shootStandardColor;
        #endregion variables

        #region internal vars
        /// <summary>
        /// Shoot button reference
        /// </summary>
        private GameObject shootBtt;
        /// <summary>
        /// Bullet gameobject reference
        /// </summary>
        private List<GameObject> bulletsGO;
        /// <summary>
        /// Bullet text reference
        /// </summary>
        private TextMeshProUGUI bulletsText;
        /// <summary>
        /// Settings button object reference
        /// </summary>
        private GameObject settingsBtt;
        /// <summary>
        /// Lose panel reference
        /// </summary>
        private GameObject losePanel;
        /// <summary>
        /// Settings panel reference, in Game
        /// </summary>
        private GameObject gameSettingsPanel;
        #endregion internal vars

        #region base methods
        private void Awake()
        {
            SettingPanelInit();
            MenuUIInit();

            // Game
            // top
            bulletsGO = new List<GameObject>();
            for (int i = 0; i < gameTopUI.transform.GetChild(1).childCount - 1; i++)
            {
                bulletsGO.Add(gameTopUI.transform.GetChild(1).GetChild(i).gameObject);
            }
            bulletsText = gameTopUI.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
            settingsBtt = gameTopUI.transform.GetChild(2).gameObject;
            // bottom
            shootBtt = gameBottomUI.transform.GetChild(0).gameObject;
            // panels
            gameSettingsPanel = gamePanels.transform.GetChild(0).gameObject;
            losePanel = gamePanels.transform.GetChild(1).gameObject;

            shootBtt.SetActive(false);
            shootBtt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetColor("_OutlineColor", shootStandardColor);
            shootBtt.SetActive(true);

            GameUILayoutInit();
        }
        #endregion base methods

        #region on click buttons
        #region SETTINGS
        /// <summary>
        /// Set sound settings
        /// </summary>
        public void OnSoundClick()
        {
            if (MenuController.settingsController.soundTrigger == (int)SettingsController.settingsTrigger.On)
            {
                menuSettingsSound.GetComponent<Image>().sprite = Resources.Load("UI/soundOff", typeof(Sprite)) as Sprite;
                MenuController.settingsController.soundTrigger = (int)SettingsController.settingsTrigger.Off;
            }
            else
            {
                menuSettingsSound.GetComponent<Image>().sprite = Resources.Load("UI/soundOn", typeof(Sprite)) as Sprite;
                MenuController.settingsController.soundTrigger = (int)SettingsController.settingsTrigger.On;
            }

        }

        /// <summary>
        /// Set muisc settings
        /// </summary>
        public void OnMusicClick()
        {
            if (MenuController.settingsController.musicTrigger == (int)SettingsController.settingsTrigger.On)
            {
                if (GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Stop();

                menuSettingsMusic.GetComponent<Image>().sprite = Resources.Load("UI/soundOff", typeof(Sprite)) as Sprite;
                MenuController.settingsController.musicTrigger = (int)SettingsController.settingsTrigger.Off;
            }
            else
            {
                menuSettingsMusic.GetComponent<Image>().sprite = Resources.Load("UI/soundOn", typeof(Sprite)) as Sprite;
                MenuController.settingsController.musicTrigger = (int)SettingsController.settingsTrigger.On;
                StartBackgroundMusic();
            }
        }

        /// <summary>
        /// Set vibration settings
        /// </summary>
        public void OnVibrationClick()
        {
            if (MenuController.settingsController.vibrationTrigger == (int)SettingsController.settingsTrigger.On)
            {
                menuSettingsVibra.GetComponent<Image>().sprite = Resources.Load("UI/vibrationOff", typeof(Sprite)) as Sprite;
                MenuController.settingsController.vibrationTrigger = (int)SettingsController.settingsTrigger.Off;
            }
            else
            {
                menuSettingsVibra.GetComponent<Image>().sprite = Resources.Load("UI/vibrationOn", typeof(Sprite)) as Sprite;
                MenuController.settingsController.vibrationTrigger = (int)SettingsController.settingsTrigger.On;
            }
        }
        #endregion SETTINGS

        #region SHOP
        /// <summary>
        /// Buy and save option of materials
        /// </summary>
        /// <param name="a_ID"></param>
        public void OnShopItemBuyClick(GameObject a_BttReference)
        {
            //string current_shop = MenuController.settingsController.currentShop;

            //if (MenuController.settingsController.Coins - GetComponent<MenuController>().shopCost >= 0)
            //{
            //    current_shop += a_BttReference.name + ";";
            //    MenuController.settingsController.currentShop = current_shop;

            //    shopContainer.transform.Find(MenuController.settingsController.currentBoxID.ToString()).GetChild(0).gameObject.SetActive(false);
            //    MenuController.settingsController.currentBoxID = int.Parse(a_BttReference.name);

            //    UpdateUICoinsAmout();

            //    MenuController.settingsController.Coins -= GetComponent<MenuController>().shopCost;
            //    foreach (GameObject cointText in GetComponent<MenuController>().textCoin)
            //    {
            //        cointText.GetComponent<TextMeshProUGUI>().text = MenuController.settingsController.Coins.ToString();
            //    }

            //    a_BttReference.transform.GetChild(1).GetComponent<Image>().color = Color.white;
            //    a_BttReference.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            //    a_BttReference.transform.GetChild(1).GetComponent<Button>()?.onClick.AddListener(() => { GetComponent<UIController>().OnShopItemChangeClick(a_BttReference); });
            //    a_BttReference.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);

            //    a_BttReference.transform.GetChild(0).gameObject.SetActive(true);
            //}
            //else
            //{
            //    StopAllCoroutines();
            //    StartCoroutine("ShopCoinsError");
            //}
        }

        /// <summary>
        /// Change and save option of materials
        /// </summary>
        /// <param name="a_ID"></param>
        public void OnShopItemChangeClick(GameObject a_BttReference)
        {
            //if (MenuController.settingsController.currentBoxID != int.Parse(a_BttReference.name))
            //{
            //    string current_shop = MenuController.settingsController.currentShop;
            //    string[] allMaterials = current_shop.Split(';');

            //    if (allMaterials.ToList().Contains(a_BttReference.name))
            //    {
            //        shopContainer.transform.Find(MenuController.settingsController.currentBoxID.ToString()).GetChild(0).gameObject.SetActive(false);
            //        MenuController.settingsController.currentBoxID = int.Parse(a_BttReference.name);
            //        a_BttReference.transform.GetChild(0).gameObject.SetActive(true);
            //    }
            //}
        }
        #endregion SHOP

        #region GAME
        public void OnGameEnterMenu()
        {
            gameCamera.gameObject.SetActive(true);
            uiCamera.clearFlags = CameraClearFlags.Depth;

            player.Init();

            GameUILayoutInit();
        }

        /// <summary>
        /// Restart current level
        /// </summary>
        public void OnRestartClick()
        {
            GameUILayoutInit();
            player.Init();
        }

        /// <summary>
        /// Start the level
        /// </summary>
        public void OnStartClick()
        {
            Time.timeScale = 1.0f;
            player.PlayerStart();
        }

        /// <summary>
        /// Open settings panel
        /// </summary>
        public void OnSettingsClick()
        {
            if (!gameSettingsPanel.activeSelf)
            {
                gameSettingsPanel.SetActive(true);
                gameBottomUI.SetActive(false);
                player.PlayerPause();
                Time.timeScale = 0.0f;
            }
            else
            {
                gameSettingsPanel.SetActive(false);
                gameBottomUI.SetActive(true);
                player.PlayerStart();
                Time.timeScale = 1.0f;
            }
        }

        /// <summary>
        /// Go to menu
        /// </summary>
        public void OnMenuClick()
        {
            // Back to Menu
            gameCamera.gameObject.SetActive(false);
            uiCamera.clearFlags = CameraClearFlags.Skybox;

            UpdateUICoinsAmout();
            MenuUIInit();
        }
        
        public void OnShootClick()
        {
            player.PlayerShoot(shootBtt);
        }
        #endregion GAME

        /// <summary>
        /// Close the game
        /// </summary>
        public void OnExitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
        #endregion on click buttons

        #region custom methods
        /// <summary>
        /// Set and play background music
        /// </summary>
        public void StartBackgroundMusic()
        {
            if (MenuController.settingsController.musicTrigger == (int)SettingsController.settingsTrigger.On)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.clip = Resources.Load<AudioClip>("Audio/musicMenu") as AudioClip;
                audio.volume = 0.5f;
                audio.loop = true;
                audio.Play();
            }
        }

        public void UpdateUICoinsAmout()
        {
            foreach (GameObject cointText in GetComponent<MenuController>().textCoin)
            {
                cointText.GetComponent<TextMeshProUGUI>().text = MenuController.settingsController.Coins.ToString();
            }
        }

        /// <summary>
        /// Update settings painel buttons with saves
        /// </summary>
        private void SettingPanelInit()
        {
            Sprite soundOn = Resources.Load("UI/soundOn", typeof(Sprite)) as Sprite;
            Sprite soundOff = Resources.Load("UI/soundOff", typeof(Sprite)) as Sprite;

            if (MenuController.settingsController.soundTrigger == (int)SettingsController.settingsTrigger.On)
                settingsPanel.transform.Find("Sound").GetChild(0).GetComponent<Image>().sprite = soundOn;
            else
                settingsPanel.transform.Find("Sound").GetChild(0).GetComponent<Image>().sprite = soundOff;

            if (MenuController.settingsController.musicTrigger == (int)SettingsController.settingsTrigger.On)
                settingsPanel.transform.Find("Music").GetChild(0).GetComponent<Image>().sprite = soundOn;
            else
                settingsPanel.transform.Find("Music").GetChild(0).GetComponent<Image>().sprite = soundOff;

            if (MenuController.settingsController.vibrationTrigger == (int)SettingsController.settingsTrigger.On)
                settingsPanel.transform.Find("Vibration").GetChild(0).GetComponent<Image>().sprite = Resources.Load("UI/vibrationOn", typeof(Sprite)) as Sprite;
            else
                settingsPanel.transform.Find("Vibration").GetChild(0).GetComponent<Image>().sprite = Resources.Load("UI/vibrationOff", typeof(Sprite)) as Sprite;
        }

        /// <summary>
        /// Menu UI starting
        /// </summary>
        public void MenuUIInit()
        {
            for (int i = 0; i < menuUIObject.transform.childCount; i++)
            {
                if (i <= 1)
                    menuUIObject.transform.GetChild(i).gameObject.SetActive(true);
                else
                    menuUIObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            StartBackgroundMusic();
            menuUIObject.SetActive(true);
            gameUIObject.SetActive(false);
        }

        /// <summary>
        /// Game UI starting
        /// </summary>
        public void GameUILayoutInit()
        {
            // UI start layout
            gamePlayBtt.SetActive(true);
            losePanel.SetActive(false);
            gameSettingsPanel.SetActive(false);
            gameBottomUI.SetActive(false);
            gameTopUI.SetActive(false);
        }

        /// <summary>
        /// Open Lose panel when finish game
        /// </summary>
        public void LoseLevel()
        {
            losePanel.SetActive(true);
            settingsBtt.SetActive(false);
        }

        #region bullets
        public void BulletsUpdate(int a_BulletAmount)
        {
            foreach (GameObject bullet in bulletsGO)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Image>().color = shootStandardColor;
                shootBtt.GetComponent<Image>().color = shootStandardColor;
                shootBtt.GetComponent<Button>().enabled = true;
            }

            shootBtt.SetActive(false);
            shootBtt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetColor("_OutlineColor", shootStandardColor);
            shootBtt.SetActive(true);


            if (a_BulletAmount <= 2)
                bulletsGO[0].SetActive(false);
            if (a_BulletAmount <= 1)
                bulletsGO[1].SetActive(false);
            if (a_BulletAmount <= 0)
            {
                bulletsGO[2].GetComponent<Image>().color = Color.red;
                shootBtt.GetComponent<Image>().color = Color.red;
                shootBtt.GetComponent<Button>().enabled = false;

                shootBtt.SetActive(false);
                shootBtt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSharedMaterial.SetColor("_OutlineColor", Color.red);
                shootBtt.SetActive(true);
            }

            bulletsText.text = a_BulletAmount.ToString();
        }
        #endregion bullets

        private IEnumerator ShopCoinsError()
        {
            Color color = Color.red;
            while (color.g < 1.0f && color.b < 1.0f)
            {
                color.g += Time.deltaTime;
                color.b += Time.deltaTime;
                shopCoinGO.transform.GetComponent<TextMeshProUGUI>().color = color;
                yield return null;
            }

            shopCoinGO.transform.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        #endregion custom methods
    }
}
