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
        private int _coins;
        private float _speed, _currentSpeed;

        private UIController uiController;
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

        public GameData(UIController a_UIController)
        {
            uiController = a_UIController;
        }
    }
}
