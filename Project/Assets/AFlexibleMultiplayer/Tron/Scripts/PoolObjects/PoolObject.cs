using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Pool/PoolObject")]
/*public class PoolObject : MonoBehaviour
{
    #region Interface
    public void ReturnToPool()
    {
        Debug.Log("Началось убираение хвостика в пул");
        StartCoroutine(wait());
    }
    #endregion

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Корутин подождал и убирает префаб в пул");
        gameObject.SetActive(false);
    }
}*/

public class PoolObject : MonoBehaviour
{    
    public void ReturnToPool()
    {
        Debug.Log("Пытаемся убрать объект в пул");
        gameObject.SetActive(false);
    }    
}
 
