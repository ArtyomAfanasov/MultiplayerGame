using PUN;
using UnityEngine;

public class Death : MonoBehaviour {
    
    public CreatorTail tailOwner;

    public void SetOwner(PhotonView ownerView)
    {
        tailOwner = ownerView.GetComponent<CreatorTail>();
        //Debug.Log("ID tailOwner - " + tailOwner.photonView.viewID);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTronWithPool"))
        {
            //Debug.Log("Да, в хвост врезался какой-то трон"); // - Пройдено |/
            // кеш ссылку скрипта на поражённом игроке
            var hitPlayer = other.GetComponent<PlayerHealth>();            

            // только в того, в которого попали, увечим. Эта часть скрипта
            // выполнится на клиенте, который будет поражён
            if (hitPlayer.photonView.isMine)
            {                
                //Debug.Log("Умираем только мы, и никто кроме нас"); // - Пройдено |/
                // игнорирует попадание в себя
                if (tailOwner.photonView.viewID == hitPlayer.photonView.viewID)
                {
                    
                    //Debug.Log("Прошли проверку, что щас убили своим хвостом себя");
                    hitPlayer.KillByTail(this, true);
                    return;
                }
                hitPlayer.KillByTail(this, false);                             
            }
        }
    }    
}