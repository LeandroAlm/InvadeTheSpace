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
    [SerializeField] public Type pieceType = Type.None;
    internal GameController gameController;

    public enum Type
    {
        None,
        Map,
        Coin,
        Bullet
    }

    void Update()
    {
        if (gameController.gameData.currentSpeed > 0)
        {
            transform.position -= Vector3.forward * gameController.gameData.currentSpeed * Time.deltaTime;

            if (transform.position.z <= gameController.plataformEndPos)
            {
                Destroy(gameObject);

                if (pieceType == Type.Map)
                {
                    gameController.junctionLoader.LoadPlaraform(Vector3.zero, 0, false, true);
                    gameController.gameData.plataforms++;
                }
            }
        }
    }
}
