// file=""PlataformController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 14/10/2021

#region usings
using Game.Controller.Game;
using UnityEngine;
#endregion usings

public class PlataformController : MonoBehaviour
{
    [SerializeField] internal GameController gameController;

    void Update()
    {
        if (gameController.mapCurrentSpeed > 0)
        {
            transform.position -= Vector3.forward * gameController.mapCurrentSpeed * Time.deltaTime;

            if (transform.position.z <= gameController.plataformEndPos)
            {
                Destroy(gameObject);

                if (this.CompareTag("Map"))
                    gameController.InstanciateNewPlatform(Vector3.zero, true);
            }
        }
    }
}
