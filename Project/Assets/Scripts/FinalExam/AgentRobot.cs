using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class AgentRobot : Agent
{
    [SerializeField] private float agentSpeed;
    [SerializeField] private Collider groundStart;
    [SerializeField] private Transform player;
    private float distance;
    private MeshRenderer mesh;
    private Rigidbody rb;
    private GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        float randX = Random.Range(groundStart.bounds.min.x, groundStart.bounds.max.x);
        float randZ = Random.Range(groundStart.bounds.min.z, groundStart.bounds.max.z);
        transform.position = new Vector3(randX, 1f, randZ);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
        sensor.AddObservation(player.position);
        sensor.AddObservation(distance);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 actuador = Vector3.zero;
        actuador.x = actions.ContinuousActions[0];
        actuador.z = actions.ContinuousActions[1];
        actuador = actuador.normalized * agentSpeed;

        distance = Vector3.Distance(transform.position, player.position);
        //print(distance);

        actuador.y = 0f; //set 0f so it does not affect the rotation vector
        if (actuador != Vector3.zero)
        {
            //transform.rotation = Quaternion.LookRotation(actuador); //Rotate
        }

        actuador.y = rb.velocity.y;
        rb.velocity = actuador;

        if (transform.position.y < 0f)
        {
            SetReward(-25f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //touch other robot
        if (collision.gameObject.CompareTag("Ally"))
        {
            SetReward(-0.5f);
            target = collision.gameObject;
            target.GetComponent<DummyTarget>().Move();
        }
        if (collision.gameObject.CompareTag("Collectable"))
        {
            SetReward(10f);
            target = collision.gameObject;
            target.GetComponent<DummyTarget>().Move();
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SetReward(25f);
            target = collision.gameObject;
            target.GetComponent<DummyTarget>().Move();
        }

        if (collision.gameObject.CompareTag("Peligro"))
        {
            SetReward(-3f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Peligro"))
        {
            SetReward(-0.1f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        mesh.material.color = Color.green;

        if (other.CompareTag("Player")) //left follow player
        {
            SetReward(-2f);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //follow player
        if (other.CompareTag("Player")) //while follow player
        {
            SetReward(0.01f);
            mesh.material.color = Color.yellow;
        }
    }
}
