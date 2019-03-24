using UnityEngine;

namespace PUN
{
    /// <summary>
    /// Создание хвоста по прямой
    /// </summary>
    public class TailCreated : Photon.PunBehaviour
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
        public Vector3 moveVector;              

        /// <summary>
        /// Время жизни блока хвоста
        /// </summary>
        public float liveTime = 1f;

        /// <summary>
        /// Скорость байка
        /// </summary>
        private float speedMove = 2f;                

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
        }

        void FixedUpdate()
        {
            if (IsAbleTail)
            {
                photonView.RPC("RPC_AddHeadTail", PhotonTargets.All, 0.1f);
            }
        }        
        
        [PunRPC]
        private void RPC_AddHeadTail(float density)
        {
            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal") * speedMove * Time.deltaTime;
            moveVector.z = Input.GetAxis("Vertical") * speedMove * Time.deltaTime;            
                 
            Vector3 bikeCirclePos = bikeCircle.transform.position;
            
            Vector3 headPos;

            headPos.x = bikeCirclePos.x - moveVector.x * density; 
            headPos.y = bikeCirclePos.y;
            headPos.z = bikeCirclePos.z - moveVector.z * density;                

            var tail = Instantiate(
                    prefabTail,
                    headPos,
                    Quaternion.FromToRotation(new Vector3(0, 0, 1), transform.forward)
                    );                

            Destroy(tail, liveTime);            
        }    
    }
}