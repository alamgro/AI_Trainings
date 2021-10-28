using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMAze : MonoBehaviour
{
    //public Maze[] maze;
    private int index;
    private int touch;

    private int maxTouch;

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
        if(Vector3.Distance(target.transform.localPosition, agent.transform.localPosition) <= 1.5f && touch < maxTouch)
        {
            Move();
        }
        //print(Vector3.Distance(target.transform.localPosition, agent.transform.localPosition));

    }

    //move target a new position 
    public void Move()
    {
        pos = posObjec.transform.GetChild(touch).transform;
        target.transform.localPosition = pos.localPosition;
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

    //formato alets
    public int MaxTouch 
    {
        get { return maxTouch; }

        //set { maxTouch = value; } 
    }

    //formato mamon
    public int Touch { get { return touch; } set { Touch = value; } }
}
