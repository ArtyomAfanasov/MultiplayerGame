using UnityEngine;

namespace PUN
{
    public class PlayerShoot : Photon.PunBehaviour
    {
        public Missile missilePefab;
        float nextFireTime;
        float fireDelay = 0.25f;
        internal bool accessToShooting = false;

        void Awake()
        {            
            enabled = photonView.isMine;
        }

        void Update()
        {
            if (accessToShooting)
            {
                Shooting();
            }
        }

        void Shooting()
        {
            if (Time.time < nextFireTime) return;

            if (Input.GetAxis("Fire1") > 0)
            {                
                nextFireTime = Time.time + fireDelay;
                photonView.RPC("RPC_FireMissile", PhotonTargets.All);
            }
        }

        [PunRPC]
        void RPC_FireMissile(PhotonMessageInfo info)
        {
            Vector3 missilePosition = transform.position + transform.forward*2f;
            missilePosition.y += 0.3f;
            
            var missile = Instantiate(missilePefab, missilePosition, transform.rotation);            
            missile.SetOwner(info.photonView);                  
            missile.gameObject.SetActive(true);
        }
    }
}