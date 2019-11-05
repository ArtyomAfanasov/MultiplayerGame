using System.Collections;
using UnityEngine;

namespace PUN
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
        private float liveTime = 2.5f;

        /// <summary>
        /// Скорость байка
        /// </summary>
        private float speedMove = 2f;
        
        public Death deathPrefab;   

        internal bool IsAbleTail = false;

        void Awake()
        {            
            enabled = photonView.isMine;
        }        

        private void Update()
        {
            SetTail();
        }      
        
        void SetTail()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IsAbleTail = !IsAbleTail;
            }

            if (IsAbleTail)
            {
                photonView.RPC("RPC_AddHeadTail", PhotonTargets.All);
            }
        }        

        [PunRPC]
        private void RPC_AddHeadTail(PhotonMessageInfo info)
        {
            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal") * speedMove * Time.deltaTime;
            moveVector.z = Input.GetAxis("Vertical") * speedMove * Time.deltaTime;          
               
            Vector3 bikeCirclePos = bikeCircle.transform.position;
           
            Vector3 headPos;
           
            headPos.x = bikeCirclePos.x - moveVector.x * 0.1f; 
            headPos.y = bikeCirclePos.y;
            headPos.z = bikeCirclePos.z - moveVector.z * 0.1f;            

            var death = Instantiate(
                    deathPrefab,
                    headPos,
                    Quaternion.FromToRotation(new Vector3(0, 0, 1), transform.forward)
                    );
            death.SetOwner(info.photonView);  
            Destroy(death.gameObject, liveTime);            
        }

        IEnumerator wait(GameObject gameObject)
        {
            //Debug.Log("Ждём 2.5 секунды с " + Time.time);
            yield return new WaitForSeconds(2.5f);
            //Debug.Log("Подождали 2.5 секунды, время - " + Time.time);
            //Debug.Log("Корутин подождал и убирает префаб в пул");
            gameObject.SetActive(false);
        }
    }       
}