using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class GuardFSM : MonoBehaviour
{
    public enum GuardState
    {
        Patrol, Chase, Attack, RunAway
    }
    public GuardState currentState;
    public GameObject enemy;
    public float GuardFOV = 89; //degrees
    private float cosGuardFOVover2InRAD;

    public float closeEnoughAttackCutoff = 2; //if (d(G,E) <=2 m) => close enough to attack
    public float closeEnoughSenseCutoff = 15; //if (d(G,E) <=15 m) => close enough to start chasing
    // Start is called before the first frame update

    public float strength = 90; //[0,100] 

    public float speed=2; //2 m/s 

    public Transform[] waypoints;
    public int nextWaypointIndex = 0;
    void Start()
    {
        cosGuardFOVover2InRAD=Mathf.Cos(GuardFOV/2f*Mathf.Deg2Rad); //in Rad

    }

    // Update is called once per frame
    void Update()
    {
        FSM();

    }

    private void FSM()
    {
        switch (currentState)
        {
            case GuardState.Patrol:
                HandlePatrol();
                break;
            case GuardState.Chase:
                HandleChase();
                break;
            case GuardState.Attack:
                HandleAttack();
                break;
            case GuardState.RunAway:
                HandleRunAway();
                break;
            default:
                break;

        }
    }

    private void HandleRunAway()
    {
        //DEAFULT actions during RunAway state
        print("Running Away...");
        RunAway();
        //CHECK TRANSITION CONDITIONS
        if (Safe())
        {
            ChangeState(GuardState.Patrol);
        }

    }

    private void RunAway()
    {
        //E.heading = (E - G).

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;

        enemyHeading.Normalize();
        //rb.velocity=enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s *s/frame=  m/frame 
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position -= movement;
    }

    private bool Safe()
    {
        
        return !Threatened();
    }

    private void HandleAttack()
    {
        //DEAFULT actions 
        print("Attack...");

        //CHECK TRANSITION CONDITIONS
        //T3 - ThreatenedAndWeakerThanEnemy
        if (ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway);
        }

    }

    private void HandleChase()
    {

        //DEAFULT actions 
        print("Chase...");
        Chase();

        //CHECK TRANSITION CONDITIONS
        //T2 - Within Range and Stronger
        if (WithinRangeAndStrongerThanEnemy())
        {
            ChangeState(GuardState.Attack);
        }
    }

    private void Chase()
    {
        //E.heading = (E - G).
        
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance=enemyHeading.magnitude;

        enemyHeading.Normalize();
       //rb.velocity=enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed*Time.deltaTime; //m/s *s/frame=  m/frame 
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;

    }

    private bool WithinRangeAndStrongerThanEnemy()
    {
        //throw new NotImplementedException();
        if(WithinRange() && !WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WeakerThanEnemy()
    {
        PlayerController enemyController = enemy.GetComponent<PlayerController>();
        if (strength < enemyController.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool WithinRange()
    {
        return EnemyCloseEnough(closeEnoughAttackCutoff);
    }

    private void HandlePatrol()
    {
        //DEAFULT actions during Patrol state
        //print("Patrolling...");
        Patrol();

        //CHECK TRANSITION CONDITIONS
        //T1 - Sense Enemy
        if (SenseEnemy())
        {
            ChangeState(GuardState.Chase);
        }
        //T3 - ThreatenedAndWeakerThanEnemy
        if (ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway); 
        }

    }

    private void Patrol()
    {
        if(Vector3.Distance(this.transform.position, waypoints[nextWaypointIndex].position) < float.Epsilon)
        {
            nextWaypointIndex=(nextWaypointIndex+1) % waypoints.Length;
        }
        Vector3 target = waypoints[nextWaypointIndex].position;
        Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        //movement.y = 0.5f;
        this.transform.position = movement;


    }

    private bool ThreatenedAndWeakerThanEnemy()
    {
        if (Threatened() && WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Threatened()
    {
        return EnemyCloseEnough(closeEnoughAttackCutoff);
    }

    private void ChangeState(GuardState newGuardState)
    {
        currentState = newGuardState;
    }

    private bool SenseEnemy()
    {
        
        //Case1: Enemy in front and close enough
        if(EnemyInFront() && EnemyCloseEnough(closeEnoughSenseCutoff)){
            return true;
        } else { 
            return false;
        }

        
        //Case2: Guard hears the enemy footsteps

        //...

        //CaseN: Smells the enemy
    }

    private bool EnemyCloseEnough(float distance)
    {
        
        if(Vector3.Distance(this.transform.position, enemy.transform.position) <= distance)
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
        //Angle(Guard.Fwd, E.heading) < GuardFOV/2 => true, else false
        // <=> cos(angle)>cos(GuardFOV/2)
        //throw new NotImplementedException();
        //E.heading = (E - G).
        Vector3 enemyHeading=(enemy.transform.position- this.transform.position).normalized;
        //if(Vector3.Angle(enemyHeading, this.transform.forward))
        //{
        //    return true;
        //}
        float cosAngle=Vector3.Dot(enemyHeading, this.transform.forward);
        if( cosAngle> cosGuardFOVover2InRAD)
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
        for(int i= 0;i<waypoints.Length;i++)
        {
            int i1=(i+1) % waypoints.Length;
            Gizmos.DrawLine(waypoints[i].transform.position, waypoints[i1].transform.position);
        }

        Gizmos.color= Color.cyan;
        //Want to see a cone from fwd-FOV/2 to fwd+FOV/2
        //Gizmos.DrawFrustum(this.transform.forward, GuardFOV/10f, closeEnoughSenseCutoff, 0.5f, 10f);
        Vector3[] pointsArray=new Vector3[20];
        float dAlpha=GuardFOV/pointsArray.Length;
        Vector3 fwdInWorldSpace = this.transform.TransformDirection(this.transform.forward);
        
        for(int i = 0; i < pointsArray.Length/4; i++)
        {
            float dAlphaPlus = dAlpha * i*Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaPlus), 0, Mathf.Sin(dAlphaPlus));
            Vector3 v = Vector3.RotateTowards(fwdInWorldSpace, target, dAlphaPlus, 10);
            
            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i+1] = this.transform.position+v*10;
        }
        for (int i = pointsArray.Length / 4; i < pointsArray.Length/2 ; i++)
        {
            float dAlphaPlus = dAlpha * (i- pointsArray.Length / 4) * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(Mathf.Cos(dAlphaMinus), 0, Mathf.Sin(dAlphaMinus));
            Vector3 v = Vector3.RotateTowards(fwdInWorldSpace, target, dAlphaMinus, 10);

            pointsArray[2 * i] += this.transform.position; //P0
            pointsArray[2 * i + 1] = this.transform.position + v * 10;
        }
        pointsArray[0] = this.transform.position;
        pointsArray[1] = this.transform.position +fwdInWorldSpace* 10;
        ReadOnlySpan<Vector3> points = new ReadOnlySpan<Vector3>(pointsArray);
        
        Gizmos.DrawLineList(points);
    }
}
