// file=""Juntion.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using UnityEngine;
#endregion usings

namespace Game.Design.Junction
{
    //[CreateAssetMenu(fileName = "LevelDesign", menuName = "ScriptableObjects/Junction")]
    [System.Serializable]
    public class Junction
    {
        public JunctionType JuntionType;
        public bool[] CoinsPosition;
        public int[] BlockWinPosition;
        public int[] BlockLosePosition;

        public enum JunctionType
        {
            Straight,
        }
        public enum JunctionPositions
        {
            Left,
            CenterLeft,
            Center,
            CenterRight,
            Right,
        }

        public void Init(JunctionType a_junctionType, bool[] a_CoinsPosition = null, int[] a_BlockWinPosition = null, int[] a_BlockLosePosition = null)
        {
            JuntionType = a_junctionType;
            if (a_CoinsPosition != null)
                CoinsPosition = a_CoinsPosition;
            if (a_BlockWinPosition != null)
                BlockWinPosition = a_BlockWinPosition;
            if (a_BlockLosePosition != null)
                BlockLosePosition = a_BlockLosePosition;
        }
    }
}
