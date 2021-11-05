// file=""Juntion.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Data.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
#endregion usings

namespace Game.Data.Plataform
{
    public class Junction : MonoBehaviour
    {
        [Tooltip("Difficulty of plataform")]
        [Range(1, 5)]
        [SerializeField] public int difficulty = 1;

        [Tooltip("Possible interactiveObjects in plataform")]
        [SerializeField] public InteractiveObject[] interactiveObjects;
    }
}
