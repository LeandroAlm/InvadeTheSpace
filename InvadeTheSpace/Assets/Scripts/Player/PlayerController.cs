// file=""PlayerController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Menu;
using System.Collections;
using TMPro;
using UnityEngine;
#endregion usings

namespace Game.Controller.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region vars
        [SerializeField]
        [Tooltip("Camera object reference")]
        private GameObject cameraObject;
        [SerializeField]
        [Tooltip("Gamecontroller object reference")]
        private GameObject gameControllerGO;
        [SerializeField]
        [Tooltip("Game coin amout object reference")]
        private GameObject coinsTextGO;
        [SerializeField]
        [Tooltip("Map object reference")]
        private GameObject mapGO;
        [SerializeField]
        [Tooltip("Side speed")]
        private float speed_side = 1.0f;
        [SerializeField]
        [Tooltip("Acceleration multiplier for side move, when drag direction change Acceleration goes 0.0f - 1.0f with this multiplier. Created to smooth direction change")]
        private float accelerationMultiplier;
        [SerializeField]
        [Tooltip("Player initial possition")]
        private Vector3 initialPosition;
        [SerializeField]
        [Tooltip("Camera initial possition")]
        private Vector3 initialPositionCamera;
        #endregion vars

        #region internal vars
        private enum playerStatus
        {
            Play,
            Pause,
            Finish,
        }
        private playerStatus currentStatus = playerStatus.Pause;

        /// <summary>
        /// Uses to update and limit max side position (keeping player inside of platform)
        /// </summary>
        private float currentLimitControl;
        /// <summary>
        /// Flag to know if ship is moving
        /// </summary>
        private bool isMoving;
        #endregion internal vars

        #region base methods
        private void Awake()
        {
            PlayerInit();
        }

        private void Update()
        {
            if (currentStatus == playerStatus.Play)
            {
                PlayerMove();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Obstaclle");
                PlayerLose();
            }
        }
        #endregion base methods

        #region custom methods
        public void PlayerInit()
        {
            currentLimitControl = 0;
            isMoving = false;

            transform.position = initialPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);

            coinsTextGO.GetComponent<TextMeshProUGUI>().text = MenuController.settingsController.Coins.ToString();
            SetCameraPostion();
        }
        
        public void PlayerStart()
        {
            currentStatus = playerStatus.Play;
        }
        
        public void PlayerPause()
        {
            currentStatus = playerStatus.Pause;
        }

        private void PlayerLose()
        {
            currentStatus = playerStatus.Finish;
            gameControllerGO.GetComponent<UI.UIController>().LoseLevel();
        }
        
        private void PlayerMove()
        {
            // Go sides
            if (!isMoving && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved )
                {
                    // Accelarate, reset on direction change, no easefunction, just Move_Linear to the 1.0f
                    if (touch.deltaPosition.x > 0.0f)
                        currentLimitControl++;
                    else if (touch.deltaPosition.x < 0.0f)
                        currentLimitControl--;
                    else
                        return;

                    if (currentLimitControl >= -2 && currentLimitControl <= 2)
                    {
                        isMoving = true;
                        StartCoroutine(PlayerMoveOverTime(touch.deltaPosition.x > 0 ? 1 : -1));
                    }
                    else
                    {
                        if (touch.deltaPosition.x > 0)
                            currentLimitControl--;
                        else if (touch.deltaPosition.x < 0)
                            currentLimitControl++;
                    }
                }
            }
        }

        private IEnumerator PlayerMoveOverTime(int a_direction)
        {
            float accelarate = 0.0f;
            float destiny = transform.position.x + a_direction;

            while ((a_direction < 0 && transform.position.x > destiny) || (a_direction > 0 && transform.position.x < destiny))
            {
                if (accelarate < 1.0f)
                    accelarate += Time.deltaTime;

                transform.position += (transform.right * a_direction) * speed_side * Time.deltaTime * accelarate;

                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(destiny, transform.position.y, transform.position.z);

            isMoving = false;
        }

        public void SetCameraPostion(Vector3 ? a_pos = null)
        {
            cameraObject.transform.localPosition = a_pos ?? initialPositionCamera;
        }
        #endregion custom methods
    }
}

