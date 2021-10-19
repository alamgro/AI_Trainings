using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Package de ml agents 
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FirstAgent : Agent
{
    /* Alexander Iñiguez October 2021
     * Behaviour of first agent
     * Agent
     */

    private Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float fallPosition; //0
    [SerializeField] private float multiply;  //10
    [SerializeField] private float rewardDistace; //1.42
    [SerializeField] private Vector3 respownPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //No se va usar el update
    /*void Update()
    {

    }*/

    //configurar lo que pasa de una iteracion-final del episodio
    public override void OnEpisodeBegin()
    {
        ///Si se cae darle un respawn
        if(transform.localPosition.y < fallPosition)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

            transform.localPosition = respownPosition;
        }

        target.localPosition = new Vector3(Random.Range(4, 8), 1f, Random.Range(4, 8));
    }

    //Sus sensores
    public override void CollectObservations(VectorSensor sensor)
    {
        //Que vea la pos del objetivo
        sensor.AddObservation(target.localPosition); //Si marcas mas ejes se agregan los ejen que tengan en este caso son 3 xzy
        sensor.AddObservation(transform.localPosition);

        //Velocida del agente
        sensor.AddObservation(rb.velocity.x);
        sensor.AddObservation(rb.velocity.z);
    }


    //acciones
    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 actuador = Vector3.zero;
        actuador.x = actions.ContinuousActions[0];
        actuador.z = actions.ContinuousActions[1];

        rb.AddForce(actuador * multiply);

        //politicas

        //dar una recompensa si se acerca mucho
        float distanceTarget = Vector3.Distance(transform.localPosition, target.localPosition);
        if(distanceTarget < rewardDistace) //1.42
        {
            SetReward(1f);
            EndEpisode();
            
        }

        //menos si se cae el wei
        else if(transform.localPosition.y < 0)
        {
            //parametro que valor recibe de recompensa en caso de ser negativa el sabe que esta perdiendo
            SetReward(-2f);
            EndEpisode();
        }


        //Por tiempo
        SetReward(-0.05f);
    }
}
