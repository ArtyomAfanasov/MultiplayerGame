using UnityEngine;

namespace PUN
{
    public class Player : Photon.PunBehaviour
    {        
        Camera playerCam;

        public PlayerMovement playerMovement { get; private set; }
        public PlayerShoot playerShoot { get; private set; }
        public PlayerHealth playerHealth { get; private set; }
        public Collider bikeCollider { get; private set; }
        
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            playerCam = GetComponentInChildren<Camera>();

            playerMovement = GetComponent<PlayerMovement>();
            playerShoot = GetComponent<PlayerShoot>();
            playerHealth = GetComponent<PlayerHealth>();
            bikeCollider = GetComponent<Collider>();

            if (!photonView.isMine)
            {
                playerCam.gameObject.SetActive(false);
                
            }            
        }
    }
}