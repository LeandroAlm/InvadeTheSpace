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
            Move,
            Shoot,
            Finish,
        }
        private playerStatus currentStatus = playerStatus.Pause;

        /// <summary>
        /// Uses to update and limit max side position (keeping player inside of platform)
        /// </summary>
        private float currentLimitControl;
        private GameObject shipGO;
        #endregion internal vars

        #region base methods
        private void Awake()
        {
            shipGO = transform.GetChild(0).gameObject;
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
            if (currentStatus == playerStatus.Play && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved && UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
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
                        currentStatus = playerStatus.Move;
                        StartCoroutine(PlayerMoveOverSpeed(touch.deltaPosition.x > 0 ? 1 : -1));
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

        public void PlayerShoot(GameObject a_ShootBtt)
        {
            if (currentStatus == playerStatus.Play)
            {
                currentStatus = playerStatus.Shoot;

                // shoot animation
                StartCoroutine(PlayerShootDelay(a_ShootBtt));
            }
        }

        private IEnumerator PlayerShootDelay(GameObject a_ShootBtt)
        {
            float time = 0.0f;
            TextMeshProUGUI textMeshPro = a_ShootBtt.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Color savedColor = textMeshPro.fontSharedMaterial.GetColor("_OutlineColor");

            a_ShootBtt.SetActive(false);
            textMeshPro.fontSharedMaterial.SetColor("_OutlineColor", Color.red);
            a_ShootBtt.SetActive(true);

            textMeshPro.outlineColor = new Color32(255, 0, 0, 255);
            shipGO.GetComponent<Animator>().SetBool("shoot", true);

            if (Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z + 0.4f), Vector3.forward, out RaycastHit hit, 100))
            {
                if (hit.collider.CompareTag("Destructible"))
                {
                    hit.collider.GetComponent<Destructible.DestructibleController>().ApplyDamage(1);
                }
            }

            while (time < 0.5)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            a_ShootBtt.SetActive(false);
            textMeshPro.fontSharedMaterial.SetColor("_OutlineColor", savedColor);
            a_ShootBtt.SetActive(true);

            shipGO.GetComponent<Animator>().SetBool("shoot", false);
            currentStatus = playerStatus.Play;
        }

        private IEnumerator PlayerMoveOverSpeed(int a_direction)
        {
            float accelarate = 0.0f;
            float destiny = transform.position.x + a_direction;

            shipGO.GetComponent<Animator>().SetInteger("direction", a_direction);

            while ((a_direction < 0 && transform.position.x > destiny) || (a_direction > 0 && transform.position.x < destiny))
            {
                if (accelarate < 1.0f)
                    accelarate += Time.deltaTime;

                transform.position += (transform.right * a_direction) * speed_side * Time.deltaTime * accelarate;

                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(destiny, transform.position.y, transform.position.z);

            shipGO.GetComponent<Animator>().SetInteger("direction", 0);
            currentStatus = playerStatus.Play;
        }

        public void SetCameraPostion(Vector3 ? a_pos = null)
        {
            cameraObject.transform.localPosition = a_pos ?? initialPositionCamera;
        }
        #endregion custom methods
    }
}

