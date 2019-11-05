using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUNTutorial
{
    public class PlayerShoot : Photon.PunBehaviour
    {
        public Missile missilePefab;
        float nextFireTime;
        float fireDelay = 0.25f;

        void Awake()
        {
            /// Тест
            if (photonView.isMine)
            {
                Debug.Log("Из PlayerShoot сообщаю: мы - свой экземпляр, мы isMine");
            }

            enabled = photonView.isMine;
        }

        void Update()
        {
            if (Time.time < nextFireTime) return;

            if (Input.GetAxis("Fire1") > 0)
            {

                /*вызываемая функция запускается на одном или нескольких удаленных клиентах, 
                 * а также на наших собственных (в зависимости от предоставленных аргументов), 
                 * и мы можем использовать это для запуска ракеты на всех клиентах.
                 */
                nextFireTime = Time.time + fireDelay;
                photonView.RPC("RPC_FireMissile", PhotonTargets.All);
            }
        }

        [PunRPC]
        void RPC_FireMissile(PhotonMessageInfo info)
        {
            Vector3 missilePosition = transform.position + transform.forward*2f;
            missilePosition.y += 0.3f;

            //var missile = Instantiate(missilePefab, transform.position, transform.rotation);
            var missile = Instantiate(missilePefab, missilePosition, transform.rotation);
            // чтобы узнать, кто запустил ракету
            missile.SetOwner(info.photonView);
            // активируем ракету        
            missile.gameObject.SetActive(true);
        }
    }
}
