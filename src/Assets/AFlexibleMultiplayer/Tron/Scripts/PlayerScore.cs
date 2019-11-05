namespace PUN
{
    public class PlayerScore : Photon.PunBehaviour
    {        
        int score;        

        private void Awake()
        {
            if (photonView.isMine)
            {
                GameUI.SetScore(score);
            }            
        }

        [PunRPC]
        void RPC_AddScore(int amount)
        {
            score += amount;
            if (photonView.isMine)
            {
                GameUI.SetScore(score);

                AccessToShooting();
            }
            //Debug.Log("Начислили очков: " + amount);
        }
        
        void AccessToShooting()
        {
            if (score == 1)
            {
                GetComponent<PlayerShoot>().accessToShooting = true;
            }
            if (score == 0)
            {
                GetComponent<PlayerShoot>().accessToShooting = false;
            }
        }
    }
}