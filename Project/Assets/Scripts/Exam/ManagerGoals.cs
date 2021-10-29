using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGoals : MonoBehaviour
{

     public Transform[] goals;
     public int index;

    void Start()
    {
        index = 0;
    }

    void Update()
    {
        
    }

    public void ResetGoals()
    {
        index = 0;

        foreach (Transform goal in goals)
        {
            goal.gameObject.SetActive(true);
        }
    }
}
