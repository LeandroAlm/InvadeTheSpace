// file=""GameDataSO.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 19/10/2021

#region usings
using UnityEngine;
#endregion usings

namespace Game.Data.Game.SO
{
    [CreateAssetMenu(fileName = "Game", menuName = "DataSO")]
    public class GameDataSO : ScriptableObject
    {
        [SerializeField]
        [Tooltip("How mutch speed increment each time")]
        [Range(0.01f, 1f)]
        internal float baseIncrementSpeed;
        [SerializeField]
        [Tooltip("Number of plataforms to upgrade speed, at start")]
        internal int minToUpgradeSpeed;
        [SerializeField]
        [Tooltip("Max number of plataforms to update speed")]
        internal int maxToUpgradeSpeed;
        [SerializeField]
        [Range(0.01f, 0.2f)]
        [Tooltip("Percentage to decrement to multiplier of speed increment, each speed update")]
        internal float percentageBaseDecrement;
    }
}
