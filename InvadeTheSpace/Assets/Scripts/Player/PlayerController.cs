// file=""PlayerController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Menu;
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
        [Tooltip("Forward speed")]
        private float speed = 1.0f;
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
        /// Player accelaration when move for sides
        /// </summary>
        private float acceleration;
        /// <summary>
        /// Flag if touch is drag to right
        /// </summary>
        private bool isMovingRigth;
        /// <summary>
        /// Current direction of Player
        /// </summary>
        private Vector3 direction;
        #endregion internal vars

        #region base methods
        private void Awake()
        {
            SetCameraPostion();
        }

        private void Update()
        {
            if (currentStatus == playerStatus.Play)
            {
                // Go forward
                transform.position += transform.forward * speed * Time.deltaTime;

                // Go sides
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved)
                    {
                        // Accelarate, reset on direction change, no easefunction, just Move_Linear to the 1.0f
                        if (touch.deltaPosition.x > 0 && !isMovingRigth)
                        {
                            isMovingRigth = true;
                            acceleration = 0.0f;
                        }
                        else if (touch.deltaPosition.x < 0 && isMovingRigth)
                        {
                            isMovingRigth = false;
                            acceleration = 0.0f;
                        }

                        if (acceleration < 1)
                            acceleration += Time.deltaTime * accelerationMultiplier;
                        else
                            acceleration = 1.0f;

                        //if (currentLimitControl + (isMovingRigth ? 1 : -1) * speed_side * Time.deltaTime * acceleration <= 2
                        //    && currentLimitControl + (isMovingRigth ? 1 : -1) * speed_side * Time.deltaTime * acceleration >= -2)
                        if (currentLimitControl >= -2 && currentLimitControl <= 2)
                        {
                            float move = (isMovingRigth ? 1 : -1) * speed_side * Time.deltaTime * acceleration;
                            float diff = move;

                            // I case o limite reach
                            if (currentLimitControl + move > 2)
                            {
                                diff = 2 - currentLimitControl;
                                transform.position += transform.right * diff;
                                currentLimitControl = 2;
                            }
                            else if(currentLimitControl + move < -2)
                            {
                                diff = -2 - currentLimitControl;
                                transform.position += transform.right * diff;
                                currentLimitControl = -2;
                            }
                            else
                            {
                                transform.position += transform.right * diff;
                                currentLimitControl += diff;
                            }

                        }
                    }
                }
                else
                    acceleration = 0.0f;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Obstaclle");
            }
        }
        #endregion base methods

        #region custom methods
        public void PlayerInit(Material a_material)
        {
            currentLimitControl = 0;
            direction = Vector3.forward;

            transform.position = initialPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);

            coinsTextGO.GetComponent<TextMeshProUGUI>().text = MenuController.settingsController.Coins.ToString();

            if (!GetComponent<Rigidbody>())
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.freezeRotation = true;
            }
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
        
        public void SetCameraPostion(Vector3 ? a_pos = null)
        {
            cameraObject.transform.localPosition = a_pos ?? initialPositionCamera;
        }
        #endregion custom methods
    }
}

