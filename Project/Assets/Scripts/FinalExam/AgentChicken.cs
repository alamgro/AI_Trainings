using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentChicken : Agent
{
    [SerializeField] private float agentSpeed;
    [SerializeField] private Collider groundStart;
    [SerializeField] private Transform player;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        float randX = Random.Range(groundStart.bounds.min.x, groundStart.bounds.max.x);
        float randZ = Random.Range(groundStart.bounds.min.z, groundStart.bounds.max.z);
        transform.position = new Vector3(randX, 0.6f, randZ);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position); 
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
        sensor.AddObservation(player.position);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        Vector3 actuador = Vector3.zero;
        actuador.x = actions.ContinuousActions[0];
        actuador.z = actions.ContinuousActions[1];
        actuador = actuador.normalized * agentSpeed;

        actuador.y = 0f; //set 0f so it does not affect the rotation vector
        if (actuador != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(actuador); //Rotate
        }

        actuador.y = rb.velocity.y; 
        rb.velocity = actuador;

        if (transform.position.y < 0f)
        {
            SetReward(-8f);
            EndEpisode();
            //print("callo");
        }

        SetReward(0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetReward(-5f);
           // print("Collision");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SetReward(-0.001f);
            //print("Trigger");
        }
    }
}
