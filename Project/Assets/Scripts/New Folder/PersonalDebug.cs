using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersonalDebug : MonoBehaviour
{
    public RandomMAze ponElMazeAqui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ponElMazeAqui.RestarAll();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ponElMazeAqui.ResetTarget();
        }
    }
}
