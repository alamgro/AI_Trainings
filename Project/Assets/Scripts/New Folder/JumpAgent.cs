using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class JumpAgent : Agent
{
    Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float agentSpeed;
    //[SerializeField] private float jumpForce;
    [SerializeField] private float jumpHeight;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform[] positions;
    private PosicionarTarget posTarget;
    private bool touchTarget;

    void Start()
    {
        touchTarget = false;
        rb = GetComponent<Rigidbody>();
        posTarget = target.GetComponent<PosicionarTarget>();
    }

    public override void OnEpisodeBegin()
    {
        if(touchTarget)
        {
            posTarget.ResetPosition();

        }
        //Reacomdar el target por cada iteracion
        rb.velocity = rb.angularVelocity = Vector3.zero;
            //Darle una de las posciones predeterminadas en una forma random
        transform.localPosition = positions[Random.Range(0, positions.Length)].localPosition;
        touchTarget = false;
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

        actuatorDir.x = agentSpeed * actions.ContinuousActions[0];
        actuatorDir.z = agentSpeed *  actions.ContinuousActions[1];
        actuatorDir.y = actions.ContinuousActions[2];

        //si NOOO esta en el piso 
        if (!Physics.Raycast(transform.position, Vector3.down, 0.51f, ground))
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
        if (targetDistance <= 1.7f)
        {
            touchTarget = true;
            SetReward(1.5f);
            EndEpisode();
        }
        //POLICY: The agents falls off the platform
        else if (transform.localPosition.y < 0f)
        {
            SetReward(-2.0f);
            EndEpisode();
        }

        //POLICY: Time punishment
        SetReward(-0.005f);
    }
}