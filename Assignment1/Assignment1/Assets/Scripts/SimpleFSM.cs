using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class SimpleFSM : MonoBehaviour
{
    public State currentState;
    public GameObject enemy;
    public float FOV = 89; //degrees
    private float cosFOVover2InRAD;
    public int speed;
    public float health;

    public float closeEnoughCutoff = 5; //if (d(G,E) <=5 m) => close enough
                                        // Start is called before the first frame update

    public Transform[] waypoints;
    public int nextWaypointIndex = 0;

    public enum State
    {
        Walk, Chase, Attack, RunAway, Rest
    }

    // Start is called before the first frame update
    void Start()
    {
        FSM();
    }

    private void FSM()
    {
        switch (currentState)
        {
            case State.Walk:
                HandleWalk();
                break;
            case State.Chase:
                HandleChase();
                break;
            case State.Attack:
                HandleAttack();
                break;
            case State.RunAway:
                HandleRunAway();
                break;
            case State.Rest:
                HandleRest();
                break;
            default:
                break;

        }
    }


    // Update is called once per frame
    void Update()
    {
        FSM();
    }


    private void HandleWalk()
    {
        if (SenseEnemy())
        {
            ChangeState(State.Chase);
        }
    }

    private void HandleChase()
    {

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;

        enemyHeading.Normalize();
        //rb.velocity=enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s *s/frame=  m/frame 
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;

        if (EnemyCloseEnough()) //go to combat
        {
            ChangeState(State.Attack);
        }
    }



    private void HandleAttack()
    {
        Debug.Log("Attacking...");

        if (!SenseEnemy()) //go to combat
        {
            ChangeState(State.Walk);
        }
        if (!EnemyCloseEnough()) //go to combat
        {
            ChangeState(State.Chase);
        }
        if (health <= 0)
        {
            ChangeState(State.RunAway);
        }

    }

    private void HandleRunAway()
    {
       
        float run = speed + (speed / 2);
        Vector3 runDir = (enemy.transform.position - this.transform.position);
        float fleeDistance = runDir.magnitude;

        runDir.Normalize();
        Vector3 movement = runDir * run * Time.deltaTime; //m/s * 2/frame = m/frame
        Vector3.ClampMagnitude(movement, fleeDistance);
        this.transform.position -= movement;

      
        if (health <= 0)
        {
            health = 100;
            ChangeState(State.Rest);
        }
        Debug.Log("Healing");
    }

    private void HandleRest()
    {
        if (!SenseEnemy()) //go to combat
        {
            ChangeState(State.Walk);
            Debug.Log("Getting back to walking");
        }
    }


    private void ChangeState(State state)
    {
        currentState = state;
    }

    private bool SenseEnemy()
    {

        //Case1: Enemy in front and close enough
        if (EnemyInFront() && EnemyCloseEnough())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool EnemyCloseEnough()
    {
        //throw new NotImplementedException();
        if (Vector3.Distance(this.transform.position, enemy.transform.position) <= closeEnoughCutoff)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool EnemyInFront()
    {
        //Angle(Guard.Fwd, E.heading) < FOV/2 => true, else false
        // <=> cos(angle)>cos(Guardfov/2)
        //throw new NotImplementedException();
        //E.heading = (E - G).
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position).normalized;
        //if(Vector3.Angle(enemyHeading, this.transform.forward))
        //{
        //    return true;
        //}
        float cosAngle = Vector3.Dot(enemyHeading, this.transform.forward);
        if (cosAngle > cosFOVover2InRAD)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length; i++)
        {
            int i1 = (i + 1) % waypoints.Length;
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i1].transform.position);
        }

        Gizmos.color = Color.cyan;
        //Want to see a cone from fwd-FOV/2 to fwd+FOV/2
        //Gizmos.DrawFrustum(this.transform.forward, GuardFOV/10f, closeEnoughSenseCutoff, 0.5f, 10f);
        Vector3[] pointsArray = new Vector3[20];
        float dAlpha = FOV / pointsArray.Length;
        Vector3 fwdInWorldSpace = this.transform.TransformDirection(this.transform.forward);

        for (int i = 0; i < pointsArray.Length / 4; i++)
        {
            float dAlphaPlus = dAlpha * i * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaPlus), 0, Mathf.Sin(dAlphaPlus));
            Vector3 v = Vector3.RotateTowards(fwdInWorldSpace, target, dAlphaPlus, 10);

            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;
        }
        for (int i = pointsArray.Length / 4; i < pointsArray.Length / 2; i++)
        {
            float dAlphaPlus = dAlpha * (i - pointsArray.Length / 4) * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaMinus), 0, Mathf.Sin(dAlphaMinus));
            Vector3 v = Vector3.RotateTowards(fwdInWorldSpace, target, dAlphaMinus, 10);

            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;
        }
        pointsArray[0] = this.transform.position;
        pointsArray[1] = this.transform.position + fwdInWorldSpace * 10;
        ReadOnlySpan<Vector3> points = new ReadOnlySpan<Vector3>(pointsArray);

        Gizmos.DrawLineList(points);
    }
}
