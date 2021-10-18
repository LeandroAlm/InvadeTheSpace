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
        private GameObject coinObject;

        private GameController gameController;
        private Transform mapParent;
        private int plataformCount;
        #endregion vars

        #region methods
        internal void Init(GameController a_GameControllerRef)
        {
            var _allPlataforms = Resources.LoadAll("Map/Plataforms/", typeof(GameObject));
            allPlataforms = new List<GameObject>();

            foreach (Object plataform in _allPlataforms)
                allPlataforms.Add(plataform as GameObject);

            gameController = a_GameControllerRef;
            mapParent = gameController.map.transform;

            coinObject = Resources.Load("Map/Collectables/Coin") as GameObject;
        }

        public void LoadPlaraform(Vector3 a_CurrentPosition, int a_forceDiff = 0, bool resetCount = false, bool a_IsInLastPostion = false, bool a_Collectables = true)
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

            if (a_Collectables)
            {
                if (Random.Range(1, 100) <= junction.probCoinRow)
                {
                    int rand_1 = Random.Range(1, 100);
                    int loop = 1;
                    int sume = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        sume += junction.probCoinsEachRow[i];
                        if (rand_1 <= sume)
                        {
                            loop = i + 1;
                            break;
                        }
                    }

                    InstanciateObejectsInRow(junction, go.transform.position, loop, coinObject);
                }
                else
                {
                    for (int x = 0; x < 5; x++)
                        for (int y = 0; y < 3; y++)
                        {
                            if (junction.coinsPosition[(x + y * 5)])
                            {
                                if (Random.Range(1, 100) <= junction.probSingleCoin)
                                    InstanciateAnObject(coinObject, go.transform.position + new Vector3(x-2, 0.5f, y-1));
                            }
                        }
                }
            }
            
            go.transform.parent = mapParent;
            plataformCount++;
        }

        private void InstanciateObejectsInRow(Junction a_junction, Vector3 a_MiddlePos, int a_RowsAmout, GameObject a_Object)
        {
            int[] rowsPos = new int[5];
            int[] rows = new int[5];
            int rowsCount = 0;

            for (int i = 0; i < 5; i++)
            {
                int count = 0;
                for (int j = 0; j < 3; j++)
                {
                    if (a_junction.coinsPosition[i + j * 5])
                        count++;
                }
                if (count >= 3)
                {
                    rows[rowsCount] = i;
                    rowsCount++;
                }

            }

            if (a_RowsAmout > rowsCount)
                a_RowsAmout = rowsCount;

            for (int i = 0; i < a_RowsAmout; i++)
            {
                int rand = Random.Range(0, rowsCount - 1);

                for (int j = -1; j < 2; j++)
                    InstanciateAnObject(a_Object, a_MiddlePos + new Vector3(rowsPos[rand] - 2, 0.5f, j));

                rowsCount--;
                if (rowsCount < 0)
                    break;
            }
        }

        /// <summary>
        /// Instanciate single object
        /// </summary>
        /// <param name="a_Object">Oject to spawn</param>
        /// <param name="a_Position">Position to spawn</param>
        /// <param name="a_SetGameController">Flag to set GameController reference</param>
        private void InstanciateAnObject(GameObject a_Object, Vector3 a_Position, bool a_SetGameController = true)
        {
            GameObject go = Instantiate(a_Object, mapParent);
            go.name = "Coin";
            go.transform.position = a_Position;
            
            if (a_SetGameController)
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
        #endregion methods
    }
}
