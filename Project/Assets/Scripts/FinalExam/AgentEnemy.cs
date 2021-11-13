using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentEnemy : Agent
{
    /* Alexander Iñiguez November 2021
     * Enemy type hunter, search a player and robot
     * Enemy
     */

    private Rigidbody rb;
    [SerializeField] private float speed;  
    //[SerializeField] private Transform hitCollider;
    [SerializeField] private Collider respownPosition;
    private GameObject target;
    private MeshRenderer robotMesh;
    //[SerializeField] private Transform[] closeTargets;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        robotMesh = GetComponent<MeshRenderer>();
    }

    //configurar lo que pasa de una iteracion-final del episodio
    public override void OnEpisodeBegin()
    {
        robotMesh.material.color = Color.green;

        //Velocity is 
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        
        float randX = Random.Range(respownPosition.bounds.min.x, respownPosition.bounds.max.x);
        float randZ = Random.Range(respownPosition.bounds.min.z, respownPosition.bounds.max.z);
        transform.position = new Vector3(randX, 1f, randZ);
    }

    //Sus sensores
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
        sensor.AddObservation(transform.rotation.y);
       /* sensor.AddObservation(closeTargets[0].position);
        sensor.AddObservation(closeTargets[1].position);
        sensor.AddObservation(closeTargets[2].position);
        sensor.AddObservation(closeTargets[3].position);
        sensor.AddObservation(closeTargets[4].position);
        sensor.AddObservation(closeTargets[5].position);*/
        //sensor.AddObservation(hitCollider.position);
        //without Y because never will jump
    }


    //acciones
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 actuador = Vector3.zero;
        actuador.x = actions.ContinuousActions[0];
        actuador.z = actions.ContinuousActions[1];
        actuador = actuador.normalized * speed;
        
        actuador.y = 0f; //set 0f so it does not affect the rotation vector
        //Rotate player to the same direction it is moving to
        if (actuador != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(actuador); //Rotate
        }

        actuador.y = rb.velocity.y; //deja su actual
        rb.velocity = actuador;
        //rb.AddForce(actuador, ForceMode.VelocityChange);

        //menos si se cae el wei
        if (transform.position.y < 0f)
        {
            //parametro que valor recibe de recompensa en caso de ser negativa el sabe que esta perdiendo
            SetReward(-8f);
            EndEpisode();
            print("callo");
        }

       //SetReward(-0.00001f);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robot"))
        {
            target = other.gameObject;
            target.GetComponent<DummyTarget>().Move();
           // robotMesh.material.color = Color.red;

            SetReward(20f);
            //EndEpisode();
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Robot"))
            robotMesh.material.color = Color.green;

    }*/

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            SetReward(-0.1f);
            robotMesh.material.color = Color.red;
            print("Pared");
        }

        if (collision.gameObject.CompareTag("Ally"))
        {
            target = collision.gameObject;
            target.GetComponent<DummyTarget>().Move();
            // robotMesh.material.color = Color.red;
            robotMesh.material.color = Color.black;
            print("Target");

            SetReward(15f);
            //EndEpisode();
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ally"))
        {
            robotMesh.material.color = Color.green;
        }
    }
}
