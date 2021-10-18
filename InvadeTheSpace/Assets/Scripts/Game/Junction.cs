// file=""Juntion.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using UnityEngine;
#endregion usings

namespace Game.Data.Plataform
{
    public class Junction : MonoBehaviour
    {
        [Tooltip("Difficulty of plataform")]
        [Range(1, 5)]
        [SerializeField] public int difficulty = 1;
        [Tooltip("Difficulty of plataform")]
        [Range(1, 100)]
        [SerializeField] public int probCoinRow = 1;
        [Tooltip("Where coins can spawn")]
        [SerializeField] public bool[] coinsPosition = new bool[15];
        [Tooltip("Probability of spawn a coin")]
        [Range(1, 100)]
        [SerializeField] public int probSingleCoin = 1;
        [Tooltip("Probability of spawn coins row")]
        [Range(1, 100)]
        [SerializeField] public int probCoinsRow = 1;
        [Tooltip("Probability of spawn coins row")]
        [Range(1, 100)]
        [SerializeField] public int[] probCoinsEachRow = new int[5];
    }
}
