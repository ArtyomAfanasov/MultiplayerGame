using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUNTutorial
{
    public class PlayerScore : Photon.PunBehaviour
    {
        int score;

        [PunRPC]
        void RPC_AddScore(int amount)
        {
            score += amount;
            Debug.Log("Начислили очков: " + amount);
        }

        private void Update()
        {
            if (photonView.isMine)
            {
                // Открытие скилов
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    Debug.Log("Сейчас очков - " + score);
                }
            }
        }
    }
}
