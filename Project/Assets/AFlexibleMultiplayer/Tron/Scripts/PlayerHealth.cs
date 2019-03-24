using System.Collections;
using UnityEngine;

namespace PUN
{
    public class PlayerHealth : Photon.PunBehaviour
    {
        const int MAX_HP = 100;
        int setHitPoints = MAX_HP;

        int hitPoints
        {
            get
            {
                return setHitPoints;
            }
            set
            {
                setHitPoints = value;
                if (photonView.isMine)
                {
                    GameUI.SetHealth(setHitPoints);
                }
            }
        }

        Player _player;
        Player player
        {
            get
            {
                if (_player == null) _player = GetComponent<Player>();
                return _player;
            }
        }

        private void Awake()
        {
            hitPoints = MAX_HP;
        }
      
        bool IsTailAvailble
        {
            set
            {
                GetComponent<CreatorTail>().IsAbleTail = value ;
            }
        }

        public void DoDamage(Missile missile)
        {
            hitPoints = Mathf.Clamp(hitPoints - missile.damage, 0, MAX_HP);
            if (hitPoints == 0)
            {
                missile.missileOwner.photonView.RPC("RPC_AddScore", missile.missileOwner.photonView.owner, 1);
                StartCoroutine(BikeDestroyer());                
            }
            //Debug.Log(GetHealthString());
        }

        public void KillByTail(Death death, bool IsSuicide)
        {
            hitPoints = 0;
            if (IsSuicide)
            {                
                death.tailOwner.photonView.RPC("RPC_AddScore", death.tailOwner.photonView.owner, -1);
                StartCoroutine(BikeDestroyer());
            }
            else
            {
                death.tailOwner.photonView.RPC("RPC_AddScore", death.tailOwner.photonView.owner, 1);
                StartCoroutine(BikeDestroyer());
            }
            
        }

        IEnumerator BikeDestroyer()
        {
            photonView.RPC("RPC_DestroyBike", PhotonTargets.All);
            //Debug.Log("Ждём две секунды с " + Time.time);
            yield return new WaitForSeconds(4f);
            
            //Debug.Log("Подождали 2 секунды, время - " + Time.time);
            var spawnPoint = GameManager.instance.GetRandomSpawnPoint();
            photonView.RPC("RPC_RespawnBike", PhotonTargets.All, spawnPoint.position, (short)spawnPoint.rotation.eulerAngles.y);
        }

        IEnumerator ShowBike(float delay)
        {
            yield return new WaitForSeconds(delay);
            transform.GetChild(0).gameObject.SetActive(true);
        }

        [PunRPC]
        void RPC_DestroyBike()
        {
            player.playerMovement.enabled = false;
            player.playerShoot.enabled = false;
            player.bikeCollider.enabled = false;
            IsTailAvailble = false;
            transform.GetChild(0).gameObject.SetActive(false);                              
        }
        
        [PunRPC]
        void RPC_RespawnBike(Vector3 respawnPos, short rot)
        {
            //Debug.Log("Пытаемся восстановиться");
            if (photonView.isMine)
            {
                player.playerMovement.enabled = true;
                player.playerShoot.enabled = true;
            }
            player.bikeCollider.enabled = true;
            transform.position = respawnPos;
            transform.rotation = Quaternion.Euler(0, rot, 0);
            hitPoints = MAX_HP;
            StartCoroutine(ShowBike(photonView.isMine ? 0f : 0.25f));                       
        }

        string GetHealthString()
        {
            return new System.Text.StringBuilder()
                .Append("Health = ")
                .Append(hitPoints / (float)MAX_HP * 100)
                .Append("%")
                .ToString();
        }        
    }
}