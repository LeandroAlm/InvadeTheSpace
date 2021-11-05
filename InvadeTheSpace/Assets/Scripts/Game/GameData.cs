// file=""GameData.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 18/10/2021

#region usings
using Game.Controller.UI;
using Game.Data.Game.SO;
using UnityEngine;
#endregion usings

namespace Game.Data.Game
{
    public class GameData
    {
        #region internal vars
        /// <summary>
        /// Total coins collected in level
        /// </summary>
        private int _coins;
        /// <summary>
        /// current plataform count, resets when >= upgradeSpeedCount
        /// </summary>
        private int _plataforms;

        /// <summary>
        /// Speed of objects in scene while is playing
        /// </summary>
        private float _speed;
        /// <summary>
        /// Current speed, depending of player status
        /// </summary>
        private float _currentSpeed;

        /// <summary>
        /// UIControlller reference
        /// </summary>
        private UIController uiController;
        /// <summary>
        /// Current plataform speed update trigger value
        /// </summary>
        private int upgradeSpeedCount;
        /// <summary>
        /// Current percentage multiplier of speed increment
        /// </summary>
        private float percentageToApply;
        /// <summary>
        /// GameDataSO refernece, passsed by constructor
        /// </summary>
        private GameDataSO gameDataSO;
        #endregion internal vars

        #region vars references
        internal int coins
        {
            get { return _coins; }
            set
            {
                _coins = value;
                uiController.CoinsUpdate(_coins);
            }
        }
        internal int plataforms
        {
            get { return _plataforms; }
            set
            {
                _plataforms = value;

                if (_plataforms >= upgradeSpeedCount && _speed > 0.0f)
                {
                    _speed += gameDataSO.baseIncrementSpeed * percentageToApply;
                    _currentSpeed = _speed;
                    _plataforms = 0;

                    if (upgradeSpeedCount < gameDataSO.maxToUpgradeSpeed)
                    {
                        upgradeSpeedCount++;
                        percentageToApply -= gameDataSO.percentageBaseDecrement;
                    }
                }
                else if (_plataforms == 0)
                {
                    percentageToApply = 1.0f;
                    upgradeSpeedCount = gameDataSO.minToUpgradeSpeed;
                }

            }
        }
        internal float speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        internal float currentSpeed
        {
            get { return _currentSpeed; }
            set { _currentSpeed = value; }
        }
        #endregion vars references

        #region constructor
        public GameData(UIController a_UIController, GameDataSO a_GameDataSO)
        {
            uiController = a_UIController;
            gameDataSO = a_GameDataSO;
        }
        #endregion constructor
    }
}
