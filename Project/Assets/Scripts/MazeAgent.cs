using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class MazeAgent : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float agentSpeed;
    //[SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float initialRewardTarget;
    [SerializeField] private LayerMask ground;
    [SerializeField] private RandomMAze mazeTarget;
    //[SerializeField] private Transform[] positions;
    //private PosicionarTarget posTarget;
    private Rigidbody rb;
    private float rewardTarget;

   // private bool touchTarget;

    void Start()
    {
        //touchTarget = false;
        rb = GetComponent<Rigidbody>();
        //posTarget = target.GetComponent<PosicionarTarget>();
        //rewardTarget = initialRewardTarget;

    }

    public override void OnEpisodeBegin()
    {
        rewardTarget = initialRewardTarget;

        rb.velocity = rb.angularVelocity = Vector3.zero;

        transform.position = Vector3.up * 0.5f;

        /*
        if (touchTarget)
        {
            posTarget.ResetPosition();

        }
        */
        //Reacomdar el target por cada iteracion
        //Darle una de las posciones predeterminadas en una forma random
        // transform.localPosition = positions[Random.Range(0, positions.Length)].localPosition;
        //touchTarget = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Thing that we want it to see
        sensor.AddObservation(target.localPosition); //Sense the position of the target
        sensor.AddObservation(transform.localPosition); //Sense its own position

        //Agent velocity
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.y);
        sensor.AddObservation(rb.velocity.z);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //config of two actions
        Vector3 actuatorDir = Vector3.zero;

        actuatorDir.x = agentSpeed * actions.ContinuousActions[0];
        actuatorDir.z = agentSpeed * actions.ContinuousActions[1];
        actuatorDir.y = actions.ContinuousActions[2];

        //si NOOO esta en el piso 
        if (!Physics.Raycast(transform.localPosition, Vector3.down, 0.51f, ground))
        {
            actuatorDir.y = 0f;

        }
        else
        {
            actuatorDir.y = Mathf.Abs(Mathf.Sqrt(-2f * jumpHeight * Physics.gravity.y) * actuatorDir.y);
            rb.AddForce(actuatorDir.y * Vector3.up, ForceMode.Impulse); //fuerza tomando en cuenta la masa y un soli impilso
            actuatorDir.y = rb.velocity.y; //deja su actual
        }

        //Add speed
        rb.AddForce(actuatorDir); //

        //politicas o recompensas
        float targetDistance = Vector3.Distance(transform.localPosition, target.localPosition);

        //POLICY: Reach target
        if (targetDistance <= 1.5f)
        {
            //touchTarget = true;
            SetReward(rewardTarget);
            rewardTarget *= 2f;
            //EndEpisode();
        }
        //POLICY: The agents falls off the platform
        else if (transform.localPosition.y < 0f)
        {
            SetReward(-2.0f);
            mazeTarget.ResetTarget();
            EndEpisode();
        }

        //POLICY: Time punishment
        SetReward(-0.005f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Target"))
        {
            mazeTarget.RestarAll();
            SetReward(4f);
            EndEpisode();
        }
    }
}