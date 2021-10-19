// file=""GameData.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 18/10/2021

#region usings
using Game.Controller.UI;
using UnityEngine;
#endregion usings

namespace Game.Data.Game
{
    public class GameData
    {
        #region internal vars
        private int _coins, _plataforms;
        private float _speed, _currentSpeed;

        private float speedBaseIncrement;
        private UIController uiController;
        private int minToUpgradeSpeed, upgradeSpeedCount;
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
                    _speed += speedBaseIncrement;
                    _currentSpeed = _speed;
                    _plataforms = 0;
                    upgradeSpeedCount++;
                }
                else if (_plataforms == 0)
                    upgradeSpeedCount = minToUpgradeSpeed;

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

        public GameData(UIController a_UIController, float a_BaseIncrementSpeed, int a_MinToUpgradeSpeed)
        {
            uiController = a_UIController;
            speedBaseIncrement = a_BaseIncrementSpeed;
            minToUpgradeSpeed = a_MinToUpgradeSpeed;
        }
    }
}
