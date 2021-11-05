// file=""InteractiveObject.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 22/10/2021

#region usings
using System;
using UnityEngine;
#endregion usings

namespace Game.Data.Objects
{
    [Serializable]
    public class InteractiveObject
    {
        [SerializeField]
        [Tooltip("Identify Object type to do interections")]
        public PlataformController.ObjectType objectType = PlataformController.ObjectType.None;
        [SerializeField]
        [Tooltip("Identify Object sub-type to do interections")]
        internal PlataformController.SubType subType = PlataformController.SubType.Collectable;
        [SerializeField]
        [Tooltip("Array 0-4; 0-9, 10-14 lines, true means can spawn Object there")]
        public bool[] spawnsPositions = new bool[15];
        [SerializeField]
        [Tooltip("Prob spawn single object")]
        [Range(0, 100)]
        public int probSpawn = 0;
        [SerializeField]
        [Tooltip("Prob spawn row of object")]
        [Range(0, 100)]
        public int probInRow = 0;
        [SerializeField]
        [Tooltip("Prob to how many rows with objects, use this just if pass in 'probInRow'")]
        [Range(0, 100)]
        public int[] probEachRow = new int[5];
        
    }
}
