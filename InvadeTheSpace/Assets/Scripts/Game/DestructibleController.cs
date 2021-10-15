// file=""DestructibleController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 12/10/2021

#region usings
using UnityEngine;
#endregion usings

namespace Game.Controller.Destructible
{
    public class DestructibleController : MonoBehaviour
    {
        #region internal vars
        [Tooltip("health of object")]
        [Range(1, 5)]
        [SerializeField] private int health = 1;
        #endregion internal vars

        /// <summary>
        /// Apply damage to a Destructible Object
        /// </summary>
        /// <param name="a_damage">Damage amount</param>
        public void ApplyDamage(int a_damage)
        {
            health -= a_damage;

            if (health <= 0)
                Destroy(gameObject, 0.2f);
        }
    }
}
