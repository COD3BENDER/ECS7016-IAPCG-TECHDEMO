using UnityEngine;
using UnityMovementAI;
//evade
// Reference: UnityMovementAI from LAB 2 and modfied to implement behaviours of wander, persuit and evade
public class AgentScript : MonoBehaviour
{
    public float cohesionWeight = 1.5f;
    public float separationWeight = 2f;
    public float velocityMatchWeight = 1f;

    SteeringBasics steeringBasics;
    Wander2 wander;
    Cohesion cohesion;
    Separation separation;
    VelocityMatch velocityMatch;

    NearSensor sensor;

    public float lookRadius = 10f;
    public float evadeRadius = 5f;
    public Transform targetPrey;

    public MovementAIRigidbody target;
    Evade evade;


    void Start()
    {
        steeringBasics = GetComponent<SteeringBasics>();
        wander = GetComponent<Wander2>();
        cohesion = GetComponent<Cohesion>();
        separation = GetComponent<Separation>();
        velocityMatch = GetComponent<VelocityMatch>();

        sensor = transform.Find("Sensor").GetComponent<NearSensor>();

        evade = GetComponent<Evade>();

    }

    void FixedUpdate()
    {
        Vector3 accel = Vector3.zero;
        float distance = Vector3.Distance(targetPrey.position, transform.position);

        if (distance > lookRadius) // wander around the map on patrol
        {
            // Wander
            accel += cohesion.GetSteering(sensor.targets) * cohesionWeight;
            accel += separation.GetSteering(sensor.targets) * separationWeight;
            accel += velocityMatch.GetSteering(sensor.targets) * velocityMatchWeight;

            if (accel.magnitude < 0.005f)
            {
                accel = wander.GetSteering();
            }

            steeringBasics.Steer(accel);
            steeringBasics.LookWhereYoureGoing();
        }
        else if (distance <= lookRadius && distance >= evadeRadius) // if the enemy is seen then pursue the enemy
        {
            // Move towards the player
            Vector3 findPrey = steeringBasics.Seek(targetPrey.position);
            steeringBasics.Steer(findPrey);
            steeringBasics.LookWhereYoureGoing();

        }
        else if (distance <= lookRadius && distance <= evadeRadius) // but if too close to the enemy evade 
        {
            Vector3 evadeAccel = evade.GetSteering(target);

            steeringBasics.Steer(evadeAccel);
            steeringBasics.LookWhereYoureGoing();

        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, evadeRadius);

    }

}


