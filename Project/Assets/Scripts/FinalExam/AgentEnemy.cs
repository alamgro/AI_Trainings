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
    [SerializeField] private Collider respownPosition;
    private GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //configurar lo que pasa de una iteracion-final del episodio
    public override void OnEpisodeBegin()
    {
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
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
        //without Y because never will jump
    }


    //acciones
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 actuador = Vector3.zero;
        actuador.x = actions.ContinuousActions[0];
        actuador.z = actions.ContinuousActions[1];
        actuador = actuador.normalized * speed;

        #region ROTATION
        actuador.y = 0f; //set 0f so it does not affect the rotation vector
        //Rotate player to the same direction it is moving to
        if (actuador != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(actuador); //Rotate
        }
        #endregion


        //menos si se cae el wei
        if (transform.position.y < 0f)
        {
            //parametro que valor recibe de recompensa en caso de ser negativa el sabe que esta perdiendo
            SetReward(-15f);
            EndEpisode();
        }
        SetReward(-0.0001f);
        actuador.y = rb.velocity.y; //deja su actual
        rb.velocity = actuador;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Robot"))
        {
            target = other.gameObject;
            target.GetComponent<DummyTarget>().Move();

            SetReward(30f);
            EndEpisode();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Peligro"))
        {
            SetReward(-0.1f);
        }
    }
}
