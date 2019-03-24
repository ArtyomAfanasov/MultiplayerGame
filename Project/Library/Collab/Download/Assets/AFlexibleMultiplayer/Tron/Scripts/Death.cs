using PUNTutorial;
using UnityEngine;

public class Death : Photon.PunBehaviour {

    Player _player;
    Player player
    {
        get
        {
            if (_player == null) _player = GetComponent<Player>();
            return _player;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = Vector3.zero;
    }

    void FixedUpdate ()
    {
		
	}
}
