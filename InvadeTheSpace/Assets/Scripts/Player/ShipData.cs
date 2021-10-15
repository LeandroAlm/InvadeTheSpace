// file=""ShipData.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 15/10/2021

#region usings
using Game.Controller.UI;
#endregion usings

namespace Game.Data.Ship
{
    public class ShipData
    {
        #region internal vars
        private int _health, _bullets;
        private float _speed;
        private UIController uiController;
        #endregion internal vars

        #region vars references
        internal int bullets
        {
            get { return _bullets; }
            set
            {
                _bullets = value;
                uiController.BulletsUpdate(_bullets);
            }
        }
        internal int health
        {
            get { return _health; }
            set
            {
                _health = value;
                //uiController.BulletsUpdate(_bullets);
            }
        }
        internal float speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                //uiController.BulletsUpdate(_bullets);
            }
        }
        #endregion vars references

        #region constructor
        internal ShipData(UIController a_UIController, float a_StartingSpeed = 2.0f)
        {
            uiController = a_UIController;
        }

        internal void ShipStartPlay(float a_StartingSpeed = 2.0f)
        {
            bullets = 1;
            speed = a_StartingSpeed;
        }
        #endregion constructor
    }
}
