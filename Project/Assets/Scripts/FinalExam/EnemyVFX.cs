using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVFX : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    Vector3 actuador = Vector3.zero;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actuador = rb.velocity;
        actuador.y = 0f; 
        if (actuador != Vector3.zero)
        {
            //transform.localRotation = Quaternion.LookRotation(actuador); //Rotate
            Quaternion targetRotation = Quaternion.LookRotation(actuador, Vector3.up);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, speed * Time.deltaTime); //Rotate

        }

    }
}
