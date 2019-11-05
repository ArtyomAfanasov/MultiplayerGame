using UnityEngine;

namespace PUN
{
    public class HUD : MonoBehaviour
    {
        static HUD instance;

        void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}