using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;
// Refernce: UnityMovementAI and modified by implementing behaviours to wander around the map and also chase the player if the player gets too close
public class PlayerScript : MonoBehaviour
{
    SteeringBasics steeringBasics;
    Wander2 wander;
    CollisionAvoidance colAvoid;

    NearSensor colAvoidSensor;

    public float lookRadius = 10f;
    public Transform targetPred;


    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        colAvoid = GetComponent<CollisionAvoidance>();

        colAvoidSensor = transform.Find("ColAvoidSensor").GetComponent<NearSensor>();

    }

    void FixedUpdate()
    {
        Vector3 accel = colAvoid.GetSteering(colAvoidSensor.targets);
        float distance = Vector3.Distance(targetPred.position, transform.position);


        if (distance > lookRadius)
        {
            // Wander
            if (accel.magnitude < 0.005f)
            {
                accel = wander.GetSteering();
            }

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();

        }
        else if (distance <= lookRadius)
        {
            Vector3 findPrey = steeringBasics.Seek(targetPred.position);
            steeringBasics.Steer(findPrey);
            steeringBasics.LookWhereYoureGoing();

        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.blue;
    }
}
