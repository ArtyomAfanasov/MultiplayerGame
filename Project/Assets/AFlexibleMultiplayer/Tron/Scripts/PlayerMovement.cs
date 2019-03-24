using UnityEngine;

namespace PUN
{
    public class PlayerMovement : Photon.PunBehaviour
    {
        public float speed = 12f;                
        public float turnSpeed = 180f;            
        private Rigidbody myRigidbody;              
        private float movementInputValue;         
        private float turnInputValue;             
               
        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();

            if (!photonView.isMine)
            {
                enabled = false;
            }                   
        }

        private void OnEnable()
        {            
            myRigidbody.isKinematic = false;
           
            movementInputValue = 0f;
            turnInputValue = 0f;
        }


        private void OnDisable()
        {            
            myRigidbody.isKinematic = true;
        }

        private void Update()
        {           
            movementInputValue = Input.GetAxis("Vertical");
            turnInputValue = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {           
            Move();
            Turn();
            myRigidbody.velocity = Vector3.zero;
            myRigidbody.angularVelocity = Vector3.zero;
        }

        private void Move()
        {            
            Vector3 movement = transform.forward * movementInputValue * speed * Time.deltaTime;
            
            myRigidbody.MovePosition(myRigidbody.position + movement);
        }

        private void Turn()
        {          
            float turn = turnInputValue * turnSpeed * Time.deltaTime;
           
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            
            myRigidbody.MoveRotation(myRigidbody.rotation * turnRotation);
        }                
    }
}