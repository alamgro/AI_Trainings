using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMAze : MonoBehaviour
{
    //public Maze[] maze;
    private int index;
    private int touch;

    private float maxTouch;

    //public GameObject[] allMaze;
    //private GameObject currentMaze;
    public GameObject[] allPositions;

    public GameObject target;
    public GameObject agent;
    private GameObject posObjec;

    private Transform pos;

    void Start()
    {
        //Use same index but be carefull both array needs be equily about thge object
        RestarAll();
        ResetTarget();
    }

    private void Update()
    {
        if(Vector3.Distance(target.transform.position, agent.transform.position) <= 1.5f && touch < maxTouch)
        {
            Move();
        }
    }

    //move target a new position 
    public void Move()
    {
        pos = posObjec.transform.GetChild(touch).transform;
        target.transform.position = pos.position;
        touch++;
    }

    public void ResetTarget()
    {
        touch = 0;
        maxTouch = posObjec.transform.childCount;
        Move();
    }
    public void RestarAll()
    {
        index = Random.Range(0, allPositions.Length);
        posObjec = allPositions[index];
        ResetTarget(); 
    }
}
