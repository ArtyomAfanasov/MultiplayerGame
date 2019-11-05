using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PUNTutorial
{
    public class Missile : MonoBehaviour
    {
        public ParticleSystem trail;
        public ParticleSystem explosion;
        public Rigidbody rbody;
        public int damage = 20;
        public PlayerShoot missileOwner; // изменил с прайвата на паблик
        float speed = 60f;

        void FixedUpdate()
        {
            // Скрипт вполняется на каждом клиенте
            rbody.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        public void SetOwner(PhotonView ownerView)
        {
            // Знаем, кто запустил ракету
            missileOwner = ownerView.GetComponent<PlayerShoot>();
            //Debug.Log("ID missileOwner - " + missileOwner.photonView.viewID);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerTronWithPool"))
            {
                //Debug.Log("Прошли проверку тэга PlayerTron");
                // кеш ссылку скрипта на поражённом игроке
                var hitPlayer = other.GetComponent<PlayerHealth>();

                // только в того, в которого попали, увечим. Эта часть скрипта
                // выполнится на клиенте, который будет поражён
                if (hitPlayer.photonView.isMine)
                {
                    //Debug.Log("Прошли проверку isMine");
                    // игнорирует попадание в себя
                    if (missileOwner.photonView.viewID == hitPlayer.photonView.viewID)
                    {
                        //Debug.Log("Прошли проверку, что себя не дамажим");
                        return;
                    }
                    hitPlayer.DoDamage(this);
                    // RPC выполняется только у выпустившего ракету (2ой аргумент)
                    // Очки отслеживаются только местным игроком
                    // 3ий аргумент - для функци RPC_AddScore                    
                }
            }

            DestroyMissile();
        }

        // без проверки photonView, которая была выше => все игроки видят взрыв
        void DestroyMissile()
        {
            // Чтобы проиграли медленно, независимо от уничтоженного родителя
            trail.transform.SetParent(null);
            // остановка излучения частицы
            trail.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            explosion.transform.SetParent(null);
            explosion.Play();
            Destroy(gameObject);
        }
    }
}
