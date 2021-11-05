using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        //isRight = Random.Range(0, 2) == 0;
        isRight = Random.value > 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isRight)
        {
            if(transform.localPosition.x < 5f)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else
            {
                isRight = true;
            }

        }
        else
        {
            if (transform.localPosition.x > -5f)
            {
                transform.position += Vector3.right * -speed * Time.deltaTime;
            }
            else
                isRight = false;
        }
    }
}
