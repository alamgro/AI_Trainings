using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [SerializeField] private Collider respawnColl;
    //[SerializeField] private GameObject enemy;

    private float randX;
    private float randZ;
    // private float timer;
    // [SerializeField] private float timerToMove;

    // Start is called before the first frame update
    /*void Start()
    {
        Move();
    }*/

    /*private void Update()
    {
        timer += Time.deltaTime;

        if(timer > timerToMove)
        {
            Move();
            timer = 0;
        }
    }*/
    private void Start()
    {
            Move();

    }
    private void OnCollisionEnter(Collision collision)
    {
        /*if(collision.gameObject.Equals(enemy))
        {
            Move();
        }*/
        if(collision.gameObject.CompareTag("Peligro"))
        {
            Move();

        }
    }
    private void Update()
    {
        if(transform.position.y < 0f)
        {
            Move();
        }
    }

    public void Move()
    {
        randX = Random.Range(respawnColl.bounds.min.x, respawnColl.bounds.max.x);
        randZ = Random.Range(respawnColl.bounds.min.z, respawnColl.bounds.max.z);
        transform.position = new Vector3(randX, 1f, randZ);
    }

}
