using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUNTutorial
{
    public class PlayerHealth : Photon.PunBehaviour
    {
        const int MAX_HP = 100;
        int hitPoints = MAX_HP;

        Player _player;
        Player player
        {
            get
            {
                if (_player == null) _player = GetComponent<Player>();
                return _player;
            }
        }

        public void DoDamage(Missile missile)
        {
            hitPoints = Mathf.Clamp(hitPoints - missile.damage, 0, MAX_HP);
            if (hitPoints == 0)
            {
                missile.missileOwner.photonView.RPC("RPC_AddScore", missile.missileOwner.photonView.owner, 1);
                StartCoroutine(BikeDestroyer());



                //photonView.RPC("RPC_DestroyBike", PhotonTargets.All); ////  ---- этого не было в ге
            }
            Debug.Log(GetHealthString());
        }

        IEnumerator BikeDestroyer()
        {
            photonView.RPC("RPC_DestroyBike", PhotonTargets.All);
            Debug.Log("Ждём две секунды с " + Time.time);
            yield return new WaitForSeconds(4f);
            
            Debug.Log("Подождали 2 секунды, время - " + Time.time);
            var spawnPoint = GameManager.instance.GetRandomSpawnPoint();
            photonView.RPC("RPC_RespawnBike", PhotonTargets.All, spawnPoint.position, (short)spawnPoint.rotation.eulerAngles.y);
        }

        IEnumerator ShowTank(float delay)
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
            transform.GetChild(0).gameObject.SetActive(false);                              
        }

        // ЕСЛИ ЧТО ПОМЕНЯТЬ ПЕРВЫЙ АРГУМЕНТ НЕ НА short
        [PunRPC]
        void RPC_RespawnBike(Vector3 respawnPos, short rot)
        {
            Debug.Log("Пытаемся восстановиться");
            if (photonView.isMine)
            {
                player.playerMovement.enabled = true;
                player.playerShoot.enabled = true;
            }
            player.bikeCollider.enabled = true;
            transform.position = respawnPos;
            transform.rotation = Quaternion.Euler(0, rot, 0);
            hitPoints = MAX_HP;
            StartCoroutine(ShowTank(photonView.isMine ? 0f : 0.25f));
            
            //transform.GetChild(0).gameObject.SetActive(true);


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

