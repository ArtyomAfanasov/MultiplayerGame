using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUNTutorial
{
    /// <summary>
    /// Создание хвоста по прямой
    /// </summary>
    public class CreatorTail : Photon.PunBehaviour
    {

        /// <summary>
        /// Префаб, используемый для создания частей хвоста
        /// </summary>
        public GameObject prefabTail;

        /// <summary>
        /// Колесо, из которого исходит хвост. Используй polySurface140 
        /// </summary>
        public GameObject bikeCircle;

        /// <summary>
        /// Вектор направления движения
        /// </summary>
        private Vector3 moveVector;

        /// <summary>
        /// Время жизни блока хвоста
        /// </summary>
        private float liveTime = 1f;

        /// <summary>
        /// Скорость байка
        /// </summary>
        private float speedMove = 2f;

        public PoolObject poolObject;

        // Use this for initialization
        void Start()
        {
            poolObject = GetComponent<PoolObject>();
        }

        private bool IsAbleTail = false;

        private void Update()
        {
            if (photonView.isMine)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    IsAbleTail = !IsAbleTail;
                }
            }

            if (IsAbleTail)
            {
                photonView.RPC("RPC_AddHeadTail", PhotonTargets.All, 0.1f);
            }
        }

        void FixedUpdate()
        {
            
        }

        // density - регулятор плотности хвоста

        [PunRPC]
        private void RPC_AddHeadTail(float density)
        {
            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal") * speedMove * Time.deltaTime;
            moveVector.z = Input.GetAxis("Vertical") * speedMove * Time.deltaTime;

            // Хвост не генерится во время стоянки байка

            // Координаты байка        
            Vector3 bikeCirclePos = bikeCircle.transform.position;

            // Координаты головы хвоста
            Vector3 headPos;

            headPos.x = bikeCirclePos.x - moveVector.x * density; // чем меньше - тем ближе друг к другу появляются
            headPos.y = bikeCirclePos.y;
            headPos.z = bikeCirclePos.z - moveVector.z * density;

            GameObject tail = PoolManager.GetObject(
                prefabTail.name, 
                headPos, 
                Quaternion.FromToRotation(new Vector3(0, 0, 1), transform.forward)
                );

            


            //tail.SetActive(false);
            //poolObject.ReturnToPool();

            StartCoroutine(wait(tail));


            /*var tail = Instantiate(
                    prefabTail,
                    headPos,
                    Quaternion.FromToRotation(new Vector3(0, 0, 1), transform.forward)
                    );*/

            //Destroy(tail, liveTime);
        }

        IEnumerator wait(GameObject gameObject)
        {
            Debug.Log("Ждём 2.5 секунды с " + Time.time);
            yield return new WaitForSeconds(2.5f);
            Debug.Log("Подождали 2.5 секунды, время - " + Time.time);
            Debug.Log("Корутин подождал и убирает префаб в пул");
            gameObject.SetActive(false);
        }
    }
    

    
}