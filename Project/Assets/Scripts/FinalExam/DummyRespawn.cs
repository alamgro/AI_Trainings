using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyRespawn : MonoBehaviour
{
    [SerializeField] private GameObject loot;
    //[SerializeField] private GameObject enmy;
    [SerializeField] private Collider ground;

    private float randX;
    private float randZ;
    private float timer;
    [SerializeField] private float y;
    [SerializeField] private float timeToLoot;

    void Start()
    {

        Respawn();
    }

  

    void Respawn()
    {
         timer = 0f;
         randX = Random.Range(ground.bounds.min.x, ground.bounds.max.x);
         randZ = Random.Range(ground.bounds.min.z, ground.bounds.max.z);
         Instantiate(loot, new Vector3(randX, y, randZ), Quaternion.identity);
    }
}
