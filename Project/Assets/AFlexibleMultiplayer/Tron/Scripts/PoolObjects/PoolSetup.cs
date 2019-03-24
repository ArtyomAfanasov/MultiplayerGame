using UnityEngine;
using System.Collections;

[AddComponentMenu("Pool/PoolSetup")]
public class PoolSetup : MonoBehaviour
{       
    public PoolManager.PoolPart[] pools;    
    
    void OnValidate()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i].name = pools[i].prefab.name;
        }
    }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        PoolManager.Initialize(pools);
    }   
}
