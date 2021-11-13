using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    [SerializeField] private Transform[] pos;
    private int index;
    [SerializeField] float speed;
    

    void Start()
    {
        index = Random.Range(0, pos.Length);

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, pos[index].position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos[index].position, speed);

        }
        else
        index = Random.Range(0, pos.Length);

    }

    void Move()
    {
        print("Moviendo");
        //index = Random.Range(0, pos.Length);
        transform.position = Vector3.MoveTowards(transform.position, pos[index].position, speed);
        print(pos.Length + " lenght");
        print(index + " index");
    }
}
