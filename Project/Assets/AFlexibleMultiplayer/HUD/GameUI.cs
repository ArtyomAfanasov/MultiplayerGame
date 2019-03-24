using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PUN
{
    public class GameUI : Photon.PunBehaviour
    { 
        static GameUI instance;
        GameObject ui;
        [SerializeField]
        TextMeshProUGUI healthText;
        [SerializeField]
        TextMeshProUGUI scoreText;
        [SerializeField]
        TextMeshProUGUI nameText;
 
        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            ui = transform.FindAnyChild<Transform>("UI").gameObject;
        }
 
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
 
        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
 
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ui.SetActive(PhotonNetwork.inRoom);
        }         
 
        public static void SetHealth(float value)
        {
            instance.healthText.text = instance.GetHealthString(value);
        }
 
        public static void SetScore(int value)
        {
            instance.scoreText.text = new System.Text.StringBuilder()
                .Append("Score: ")
                .Append(value)
                .ToString();
        }
 
        string GetHealthString(float normalisedHPPercent)
        {
            return new System.Text.StringBuilder()
                .Append("Health: ")
                .Append((int)(normalisedHPPercent))
                .Append("%")
                .ToString();
        }
    }
}