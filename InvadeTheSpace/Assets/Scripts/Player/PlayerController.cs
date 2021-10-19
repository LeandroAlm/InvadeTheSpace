// file=""PlayerController.cs" company=""
// Copyright (c) 2021 All Rights Reserved
// Author: Leandro Almeida
// Date: 05/10/2021

#region usings
using Game.Controller.Game;
using Game.Controller.Menu;
using Game.Controller.UI;
using Game.Data.Ship;
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
        private float startingSpeed;
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
        /// <summary>
        /// Ship object reference
        /// </summary>
        private GameObject shipGO;
        /// <summary>
        /// ShipData stores all INFO of ship
        /// </summary>
        private ShipData shipData;
        /// <summary>
        /// Reference of GameController
        /// </summary>
        private GameController gameController;
        /// <summary>
        /// Reference of UIController
        /// </summary>
        private UIController uiController;
        #endregion internal vars

        #region base methods
        private void Start()
        {
            gameController = gameControllerGO.GetComponent<GameController>();
            uiController = gameControllerGO.GetComponent<UIController>();

            shipGO = transform.GetChild(0).gameObject;
            shipData = new ShipData(uiController);
            //Init();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (currentStatus == playerStatus.Play)
            {
                PlayerMove();
            }

#if UNITY_EDITOR
            Debug.DrawLine(new Vector3(transform.position.x, 0.5f, transform.position.z + 0.4f), Vector3.forward * 100, Color.red);
#endif
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.CompareTag("Collectable"))
            {
                collision.tag = "Untagged";

                if (collision.GetComponent<PlataformController>().pieceType == PlataformController.Type.Coin)
                    gameController.gameData.coins++;
                else if (collision.GetComponent<PlataformController>().pieceType == PlataformController.Type.Bullet)
                    shipData.bullets++;

                GameObject.Destroy(collision.gameObject);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Destructible"))
            {
                PlayerLose();
            }
        }
        #endregion base methods

        #region custom methods
        public void Init()
        {
            shipGO.SetActive(true);
            gameObject.SetActive(true);
            shipGO.transform.rotation = Quaternion.Euler(0, 0, 0);
            shipData.ShipStartPlay(startingSpeed);
            currentLimitControl = 0;

            transform.position = initialPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);

            coinsTextGO.GetComponent<TextMeshProUGUI>().text = MenuController.settingsController.coins.ToString();
            SetCameraPostion();
        }
        
        public void PlayerStart()
        {
            SetPlayerStatus(playerStatus.Play);
        }
        
        public void PlayerPause()
        {
            SetPlayerStatus(playerStatus.Pause);
        }

        private void PlayerLose()
        {
            StopAllCoroutines();
            shipGO.SetActive(false);
            Instantiate(gameController.explosionParticle, shipGO.transform.position, shipGO.transform.rotation);
            SetPlayerStatus(playerStatus.Finish);
            uiController.GameLose(gameController.gameData.coins);
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
                        SetPlayerStatus(playerStatus.Move);
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
            if (currentStatus == playerStatus.Play && shipData.bullets > 0)
            {
                SetPlayerStatus(currentStatus = playerStatus.Shoot);
                shipData.bullets--;

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

            if (Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z + 0.4f), Vector3.forward, out RaycastHit hit, 50, 1 << 3))
            {
                if (hit.collider.CompareTag("Destructible"))
                {
                    hit.collider.GetComponent<Destructible.DestructibleController>().ApplyDamage(1, gameController);
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
            SetPlayerStatus(playerStatus.Play);
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

                transform.position += (transform.right * a_direction) * shipData.speed * Time.deltaTime * accelarate;

                yield return new WaitForEndOfFrame();
            }

            transform.position = new Vector3(destiny, transform.position.y, transform.position.z);

            shipGO.GetComponent<Animator>().SetInteger("direction", 0);
            SetPlayerStatus(playerStatus.Play);
        }

        public void SetCameraPostion(Vector3 ? a_pos = null)
        {
            cameraObject.transform.localPosition = a_pos ?? initialPositionCamera;
        }
        
        private void SetPlayerStatus(playerStatus a_PlayerStatus)
        {
            currentStatus = a_PlayerStatus;

            if (currentStatus == playerStatus.Pause || currentStatus == playerStatus.Finish)
                gameController.OnPlayerStatusChange(false);
            else
                gameController.OnPlayerStatusChange(true);
        }
        #endregion custom methods
    }
}

