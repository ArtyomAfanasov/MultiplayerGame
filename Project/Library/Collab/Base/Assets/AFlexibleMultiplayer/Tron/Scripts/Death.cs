using UnityEngine;

public class Death : Photon.PunBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = Vector3.zero;
    }

    void FixedUpdate ()
    {
		
	}
}
