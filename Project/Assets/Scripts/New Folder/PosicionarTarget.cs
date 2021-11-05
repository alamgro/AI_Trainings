using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosicionarTarget : MonoBehaviour
{
    /*Mover el cubito hasta que quede chido
     * 
     */
    public Collider CollGround;
    public LayerMask layer;

    private void Start()
    {
        ResetPosition();
    }

    private void Update()
    {
        //if (Physics.SphereCast(transform.position + Vector3.up, 0.5f,  Vector3.down, out RaycastHit hit, 0.9f, layer))
        if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, 0.9f, layer))
        {
            transform.localPosition += Vector3.up * 1.0f;
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.yellow);

        }
        else
            Debug.DrawRay(transform.position + Vector3.up, Vector3.down, Color.magenta);



    }

    public void ResetPosition()
    {
        transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        print("Me toco el pendejo del agente");
    }
}
