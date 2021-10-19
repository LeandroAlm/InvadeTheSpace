// file=""JunctionLoader.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Game;
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

            bool[] currentAvaibility = new bool[15];
            for (int i = 0; i < 15; i++)
            {
                currentAvaibility[i] = true;
            }


            if (a_Collectables)
            {
                currentAvaibility = SpawCoins(junction, currentAvaibility, go.transform.position);
                currentAvaibility = SpawnBullets(junction, currentAvaibility, go.transform.position);
            }
            if (a_Destructibles)
            {
                currentAvaibility = SpawnAsteroids(junction, currentAvaibility, go.transform.position);
            }

            go.transform.parent = mapParent;
            plataformCount++;
        }

        private bool[] InstanciateObejectsInRow(Junction a_junction, bool[] a_Avaibility, Vector3 a_MiddlePos, int a_RowsAmout, GameObject a_Object)
        {
            bool[] array = a_Avaibility;
            int[] rowsPos = new int[5];
            int[] rows = new int[5];
            int rowsCount = 0;

            for (int i = 0; i < 5; i++)
            {
                int count = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (a_junction.coinsPosition[i + j * 5] && array[i + j * 5])
                        count++;
                }
                if (count >= 3)
                {
                    rows[rowsCount] = i;
                    rowsCount++;
                }

                rowsPos[i] = i;
            }

            if (a_RowsAmout > rowsCount)
                a_RowsAmout = rowsCount;

            for (int i = 0; i < a_RowsAmout; i++)
            {
                int rand = Random.Range(0, rowsCount - 1);

                for (int j = 0; j < 3; j++)
                {
                    InstanciateAnObject(a_Object, a_MiddlePos + new Vector3(rowsPos[rows[rand]] - 2, 0.5f, j-1));
                    array[(rowsPos[rows[rand]]) + (j*5)] = false;
                }

                rows = rows.Where(t => t != rows[rand]).ToArray();
                rowsCount--;
                if (rowsCount < 0)
                    break;
            }

            return array;
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

            return pullPlataform[0];
        }

        #region objects function
        private bool[] SpawCoins(Junction a_Junction, bool[] a_Avaibility, Vector3 a_Position)
        {
            if (Random.Range(1, 101) <= a_Junction.probCoinsRow)
            {
                int rand_1 = Random.Range(1, 101);
                int loop = 1;
                int sume = 0;

                for (int i = 0; i < 5; i++)
                {
                    sume += a_Junction.probCoinsEachRow[i];
                    if (rand_1 <= sume)
                    {
                        loop = i + 1;
                        break;
                    }
                }

                a_Avaibility = InstanciateObejectsInRow(a_Junction, a_Avaibility, a_Position, loop, coinObject);
            }
            else
            {
                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 3; y++)
                    {
                        if (a_Junction.coinsPosition[(x + y * 5)] && a_Avaibility[x + y * 5])
                        {
                            if (Random.Range(1, 101) <= a_Junction.probSingleCoin)
                            {
                                InstanciateAnObject(coinObject, a_Position + new Vector3(x - 2, 0.5f, y - 1));
                                a_Avaibility[x + y * 5] = false;
                            }
                        }
                    }
            }

            return a_Avaibility;
        }
        private bool[] SpawnBullets(Junction a_Junction, bool[] a_Avaibility, Vector3 a_Position)
        {
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 3; y++)
                {
                    if (a_Junction.bulletsPosition[(x + y * 5)] && a_Avaibility[x + y * 5])
                    {
                        if (Random.Range(1, 101) <= a_Junction.probSingleBullet)
                        {
                            InstanciateAnObject(bulletObject, a_Position + new Vector3(x - 2, 0.15f, y - 1));
                            a_Avaibility[x + y * 5] = false;
                        }
                    }
                }

            return a_Avaibility;
        }
        private bool[] SpawnAsteroids(Junction a_Junction, bool[] a_Avaibility, Vector3 a_Position)
        {
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 3; y++)
                {
                    if (a_Junction.coinsPosition[(x + y * 5)] && a_Avaibility[x + y * 5])
                    {
                        if (Random.Range(1, 101) <= a_Junction.probSingleDestructible)
                        {
                            InstanciateAnObject(destructibles[Random.Range(0, destructibles.Count)], a_Position + new Vector3(x - 2, 0.3f, y - 1));
                            a_Avaibility[x + y * 5] = false;
                        }
                    }
                }

            return a_Avaibility;
        }
        #endregion objects function
        #endregion methods
    }
}
