using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class AgentExam : Agent
{
    private Transform target;
    [SerializeField] private float agentSpeed;
    [SerializeField] private float jumpHeight;
    private float jump;
    //[SerializeField] private float initialRewardTarget;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider groundStart;

    [SerializeField] private ManagerGoals managerGoals;
    private MeshRenderer meshRender;
    private Rigidbody rb;
    //private float rewardTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshRender = GetComponent<MeshRenderer>();
        meshRender.material.color = Color.green;
        target = managerGoals.goals[managerGoals.index];

    }

    public override void OnEpisodeBegin()
    {
        //rewardTarget = initialRewardTarget;
        rb.velocity = rb.angularVelocity = Vector3.zero;
        //transform.localPosition = Vector3.up * 0.5f;
        float randX = Random.Range(groundStart.bounds.min.x, groundStart.bounds.max.x);
        float randZ = Random.Range(groundStart.bounds.min.z, groundStart.bounds.max.z);
        transform.position = new Vector3(randX, 0.6f, randZ);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //Thing that we want it to see
        sensor.AddObservation(target.position); //Sense the position of the target
        sensor.AddObservation(transform.position); //Sense its own position

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
        // actuatorDir.y = actions.ContinuousActions[2];
        jump = actions.ContinuousActions[2];
        //rb.AddForce(actuatorDir);
        actuatorDir.y = rb.velocity.y; //deja su actual

        //si NOOO esta en el piso 
        if (!Physics.Raycast(transform.position, Vector3.down, 0.51f, ground))
        {
            //actuatorDir.y = 0f;
            jump = 0f;

            Debug.DrawRay(transform.position, Vector3.down * 0.51f, Color.magenta );
        }
        else
        {
            actuatorDir.y = Mathf.Abs(Mathf.Sqrt(-2f * jumpHeight * Physics.gravity.y) * jump);// actuatorDir.y);
            rb.AddForce(actuatorDir.y * Vector3.up, ForceMode.Impulse); //fuerza tomando en cuenta la masa y un soli impilso
            Debug.DrawRay(transform.position, Vector3.down * 0.51f, Color.cyan);
        }

        //POLICY: The agents falls off the platform
        if (transform.localPosition.y < 0f)
        {
            SetReward(-8.0f);
            managerGoals.ResetGoals();
            EndEpisode();
        }
        rb.velocity = actuatorDir;
            //SetReward(-0.001f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Peligro"))
        {
            SetReward(-1f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            //mazeTarget.RestarAll();
            managerGoals.index++;
            target = managerGoals.goals[managerGoals.index];
            other.gameObject.SetActive(false);
            SetReward(15f);
        }
        if (other.CompareTag("Finish"))
        {
            SetReward(100f);
            managerGoals.ResetGoals(); 
            EndEpisode();
        }
    }

    public static Vector3 ClampMagnitude(Vector3 _vectorToClamp, float minMagnitude)
    {
        float vecMagnitude = _vectorToClamp.magnitude;
        if (vecMagnitude < minMagnitude)
        {
            Vector3 vecNormalized = _vectorToClamp / vecMagnitude; //equivalent to _vectorToClamp.normalized, but slightly faster in this case
            return vecNormalized * minMagnitude;
        }

        // No need to clamp at all
        return _vectorToClamp;
    }
}
