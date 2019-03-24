using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace PUNTutorial
{
    public class GameManager : Photon.PunBehaviour
    {
        public static GameManager instance;         

        public static GameObject localPlayer;

        void Awake()
        {

            // Применяем паттерн Singlton
            if (instance != null) 
            {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;

            defaultSpawnPoint = new GameObject("Default SpawnPoint");
            defaultSpawnPoint.transform.position = new Vector3(0, 0.5f, 0);
            defaultSpawnPoint.transform.SetParent(transform, false);

            PhotonNetwork.automaticallySyncScene = true;            
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings("Version 1");
            ChangeTickRate();
            Debug.Log("sendRate" + PhotonNetwork.sendRate);
            Debug.Log("sendRateOnSerialize" + PhotonNetwork.sendRateOnSerialize);
        }

        void ChangeTickRate()
        {
            PhotonNetwork.sendRate = 60;
            PhotonNetwork.sendRateOnSerialize = 40;
        }

        /// <summary>
        /// Вызывается нажатием на кнопку "Join game" в UI
        /// </summary>
        public void JoinGame()
        {
            var roomOptions = new RoomOptions();            
            roomOptions.MaxPlayers = 6;
            PhotonNetwork.JoinOrCreateRoom("Default Room", roomOptions, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");

            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("Game Scene");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Поместите prefab PlayerObject в папку Resources
        /// </remarks>
        void OnLevelWasLoaded(int levelNumber)
        {
            if (!PhotonNetwork.inRoom) return;

            var spawnPoint = GetRandomSpawnPoint();

            localPlayer = PhotonNetwork.Instantiate(
                "PlayerTronWithPool",
                spawnPoint.position,
                spawnPoint.rotation, 0);
        }

        GameObject defaultSpawnPoint;

        Transform GetRandomSpawnPoint()
        {
            var spawnPoints = GetAllObjectsOfTypeInScene<SpawnPoints>();
            if (spawnPoints.Count == 0)
            {
                return defaultSpawnPoint.transform;
            }
            else
            {
                return spawnPoints[Random.Range(0, spawnPoints.Count)].transform;
            }
        }

        /*Это выполняет функцию, аналогичную функции Unity GameObject.FindObjectsOfType (), 
         * но в отличие от этой функции она также находит неактивные объекты. 
         * Причина, по которой нам нужно это сделать, заключается в том, 
         * что мы не можем быть уверены, что SpawnPoints не деактивируются к тому времени, 
         * когда мы начнем их искать.
         */

        public static List<GameObject> GetAllObjectsOfTypeInScene<T>()
        {
            List<GameObject> objectsInScene = new List<GameObject>();

            foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject))
                     as GameObject[])
            {
                if (go.hideFlags == HideFlags.NotEditable ||
                    go.hideFlags == HideFlags.HideAndDontSave)
                    continue;

                if (go.GetComponent<T>() != null)
                    objectsInScene.Add(go);
            }

            return objectsInScene;
        }
    }
}
