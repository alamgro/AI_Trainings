using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    [SerializeField] private Transform[] pos;
    private int index;
    [SerializeField] float speed;
    private Rigidbody rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        index = Random.Range(0, pos.Length);
        transform.LookAt(pos[index].position);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, pos[index].position) > 0.1f)
        {
            rb.velocity = transform.forward * speed;
            transform.LookAt(pos[index].position);
        }
        else
        {
            index = Random.Range(0, pos.Length);
            transform.LookAt(pos[index].position);
            //print("Cambio ");
        }
    }
}
