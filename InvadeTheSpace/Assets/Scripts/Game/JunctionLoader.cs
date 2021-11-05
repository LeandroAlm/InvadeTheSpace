// file=""JunctionLoader.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Game;
using Game.Data.Objects;
using Game.Data.Plataform;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#endregion usings

namespace Game.Loader.Plataform
{
    public class JunctionLoader : MonoBehaviour
    {
        #region vars
        private List<GameObject> allPlataforms;
        private List<GameObject> destructibles;
        private GameObject coinObject, bulletObject;

        private GameController gameController;
        private Transform mapParent;
        private int plataformCount;
        private bool forceBase = false;
        #endregion vars

        #region methods
        internal void Init(GameController a_GameControllerRef)
        {
            // Plataforms
            var _allPlataforms = Resources.LoadAll("Map/Plataforms/", typeof(GameObject));
            allPlataforms = new List<GameObject>();

            foreach (Object plataform in _allPlataforms)
                allPlataforms.Add(plataform as GameObject);

            // Destructibles
            var _allDestructibles = Resources.LoadAll("Map/Destructible/", typeof(GameObject));
            destructibles = new List<GameObject>();

            foreach (Object destru in _allDestructibles)
                destructibles.Add(destru as GameObject);

            // Collect
            coinObject = Resources.Load("Map/Collectables/Coin") as GameObject;
            bulletObject = Resources.Load("Map/Collectables/Bullet") as GameObject;

            gameController = a_GameControllerRef;
            mapParent = gameController.map.transform;
        }

        public void LoadPlaraform(Vector3 a_CurrentPosition, int a_forceDiff = 0, bool resetCount = false, bool a_IsInLastPostion = false, bool a_Collectables = true, bool a_Destructibles = true)
        {
            GameObject go = Instantiate(GetRandomPlataform(a_forceDiff));
            Junction junction = go.GetComponent<Junction>();

            go.GetComponent<PlataformController>().gameController = this.gameController;

            if (resetCount)
                plataformCount = 0;

            if (a_IsInLastPostion)
                go.transform.position = gameController.plataformLastPos;
            else
                go.transform.position = a_CurrentPosition;

            go.name = "Junction_" + plataformCount.ToString();

            SpawnAllObjects(junction.interactiveObjects, go.transform.position, a_Collectables, a_Destructibles);

            go.transform.parent = mapParent;
            plataformCount++;
        }

        /// <summary>
        /// Instanciate single object
        /// </summary>
        /// <param name="a_Object">Oject to spawn</param>
        /// <param name="a_Position">Position to spawn</param>
        /// <param name="a_SetGameController">Flag to set GameController reference</param>
        private void InstanciateAnObject(GameObject a_Object, Vector3 a_Position)
        {
            GameObject go = Instantiate(a_Object, mapParent);
            go.transform.position = a_Position;
            
            go.GetComponent<PlataformController>().gameController = this.gameController;
        }

        /// <summary>
        /// Give Rangom plataform
        /// </summary>
        /// <param name="a_DiffForced">Force difficulty of plataform</param>
        /// <returns>Plataform Object</returns>
        private GameObject GetRandomPlataform(int a_DiffForced)
        {
            List<GameObject> pullPlataform = new List<GameObject>();
            GameObject plat = null;

            if (!forceBase)
            {
                if (a_DiffForced > 0)
                {
                    if (a_DiffForced > 5)
                        a_DiffForced = 5;

                    foreach (GameObject plataform in allPlataforms)
                    {
                        if (plataform.GetComponent<Junction>().difficulty == a_DiffForced)
                            pullPlataform.Add(plataform);
                    }
                }
                else
                    pullPlataform = allPlataforms;

                plat = pullPlataform[Random.Range(0, pullPlataform.Count)];

                if (plat != allPlataforms[0])
                    forceBase = true;
            }
            else
            {
                plat = allPlataforms[0];
                forceBase = false;
            }


            return plat;
        }

        #region objects function
        /// <summary>
        /// Spawn all objects if possible
        /// </summary>
        /// <param name="a_AllObjects">All Objects possible in plataform</param>
        /// <param name="a_Position">Junction position</param>
        /// <param name="a_Collectables">is to add collectables</param>
        /// <param name="a_Destructibles">is to add destructibles</param>
        private void SpawnAllObjects(InteractiveObject[] a_AllObjects, Vector3 a_Position, bool a_Collectables, bool a_Destructibles)
        {
            bool[] avaibility = new bool[15];
            int count_1 = 0;
            int rand_1 = 0;

            for (int i = 0; i < 15; i++)
                avaibility[i] = true;

            foreach (InteractiveObject child in a_AllObjects)
            {
                if ((child.subType == PlataformController.SubType.Collectable && !a_Collectables)
                    || (child.subType == PlataformController.SubType.Destructible && !a_Destructibles))
                    continue;

                rand_1 = Random.Range(1, 101);

                if (rand_1 <= child.probInRow)
                {
                    rand_1 = Random.Range(1, 101);
                    int temp_sum = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        temp_sum += child.probEachRow[i];

                        if (temp_sum <= rand_1)
                        {
                            int[] temp_array = new int[5];
                            
                            for (int j = 0; j < 5; j++)
                            {
                                temp_sum = 0; // use as counter

                                for (int h = 0; h < 3; h++)
                                    if (avaibility[j + h * 5])
                                        temp_sum++;

                                if (temp_sum >= 3)
                                {
                                    temp_array[count_1] = j;
                                    count_1++;
                                }
                            }

                            for (int j = 0; j < i; j++)
                            {
                                rand_1 = Random.Range(0, temp_sum);

                                for (int y = 0; y < 3; y++)
                                {
                                    InstanciateAnObject(coinObject, a_Position + new Vector3(temp_array[rand_1] - 2, 0.5f, y - 1));
                                    avaibility[(temp_array[rand_1] - 2) + ((y - 1) * 5)] = false;
                                }


                                temp_array = temp_array.Where(t => t != temp_array[rand_1]).ToArray();
                                temp_sum--;
                            }

                            break;
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < 5; x++)
                        for (int y = 0; y < 3; y++)
                            if (avaibility[x + y * 5])
                            {
                                rand_1 = Random.Range(1, 101);

                                if (rand_1 <= child.probSpawn)
                                {
                                    InstanciateAnObject(coinObject, a_Position + new Vector3(x - 2, 0.5f, y - 1));
                                    avaibility[x + (y * 5)] = false;
                                }
                            }
                }
            }
        }
        #endregion objects function
        #endregion methods
    }
}
