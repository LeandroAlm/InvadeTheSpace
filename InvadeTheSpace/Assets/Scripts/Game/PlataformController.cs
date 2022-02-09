// file=""PlataformController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 14/10/2021

#region usings
using Game.Controller.Game;
using System.Collections;
using UnityEngine;
#endregion usings

public class PlataformController : MonoBehaviour
{
    [SerializeField] public ObjectType pieceType = ObjectType.None;
    internal GameController gameController;

    private bool setTransparency = true;

    public enum ObjectType
    {
        None,
        Map,
        Coin,
        Bullet,
        Meteoroids
    }
    public enum SubType
    {
        Collectable,
        Destructible
    }

    void Update()
    {
        if (gameController.gameData.currentSpeed > 0)
        {
            transform.position -= Vector3.forward * gameController.gameData.currentSpeed * Time.deltaTime;

            if (transform.position.z <= gameController.plataformEndPos)
            {
                Destroy(gameObject);

                if (pieceType == ObjectType.Map)
                {
                    gameController.junctionLoader.LoadPlaraform(Vector3.zero, 0, false, true);
                    gameController.gameData.plataforms++;
                }
            }
            else if (pieceType == ObjectType.Map && transform.position.z <= 0.25f && setTransparency)
            {
                if (transform.childCount > 0)
                {
                    StartCoroutine(ReduceAlpha(transform));
                }
                setTransparency = false;
            }
        }
    }

    private IEnumerator ReduceAlpha(Transform a_Parent)
    {
        while (true)
        {
            foreach (Transform child in a_Parent)
            {
                if (!ApplyAlphaToTransformAndChilds(child))
                    break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private bool ApplyAlphaToTransformAndChilds(Transform a_Parent)
    {
        MeshRenderer parentMesh = a_Parent.GetComponent<MeshRenderer>();
        bool isToRecall = true;

        if (parentMesh != null)
        {
            parentMesh.material.color = new Color(1.0f, 1.0f, 1.0f, parentMesh.material.color.a - 0.01f);

            if (parentMesh.material.color.a <= 0.25f)
                isToRecall = false;
        }

        if (a_Parent.childCount > 0)
            foreach (Transform child in a_Parent)
                isToRecall = ApplyAlphaToTransformAndChilds(child);

        return isToRecall;
    }
}
