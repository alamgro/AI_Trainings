using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class MyAgent : Agent
{
    Rigidbody rb;
    [SerializeField] private Transform target = null;
    private float agentSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        //If the agent falls
        if(transform.localPosition.y < 0f)
        {
            rb.velocity = rb.angularVelocity = Vector3.zero;
            transform.localPosition = new Vector3(0f, 0.5f, 0f);
        }

        //Move target to random place
        target.localPosition = new Vector3(Random.value * 8f - 4f, 0.5f, Random.value * 8f - 4f);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Thing that we want it to see
        sensor.AddObservation(target.localPosition); //Sense the position of the target
        sensor.AddObservation(transform.localPosition); //Sense its own position

        //Agent velocity
        sensor.AddObservation(rb.velocity.magnitude);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //config of two actions
        Vector3 actuatorDir = Vector3.zero;
        actuatorDir.x = actions.ContinuousActions[0];
        actuatorDir.z = actions.ContinuousActions[1];

        //Add speed
        rb.AddForce(actuatorDir * agentSpeed);

        //politicas o recompensas
        float targetDistance = Vector3.Distance(transform.localPosition, target.localPosition);

        //POLICY: Reach target
        if(targetDistance <= 1.42f)
        {
            SetReward(1f);
            EndEpisode();
        }
        //POLICY: The agents falls off the platform
        else if(transform.localPosition.y < 0f)
        {
            // SetReward(-2.0f);
            EndEpisode();
        }

        //POLICY: Time punishment
        // SetReward(-0.05f);

    }


}
