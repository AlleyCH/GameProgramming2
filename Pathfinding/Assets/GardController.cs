using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardController : MonoBehaviour
{
    
    public StateMachine stateMachine;

    public StateMachine.State patrol, chase, attack, runAway;
    // Start is called before the first frame update

    public GameObject enemy;
    public float GuardFOV = 89; //degrees
    private float cosGuardFOVover2InRAD;

    public float closeEnoughAttackCutoff = 2; //if (d(G,E) <=2 m) => close enough to attack
    public float closeEnoughSenseCutoff = 15; //if (d(G,E) <=15 m) => close enough to start chasing
    // Start is called before the first frame update

    public float strength = 90; //[0,100] 

    public float speed = 2; //2 m/s 

    public Transform[] waypoints;
    public int nextWaypointIndex = 0;

    void Start()
    {
        cosGuardFOVover2InRAD = Mathf.Cos(GuardFOV / 2f * Mathf.Deg2Rad); //in Rad

        stateMachine = new StateMachine();

        //Use the Factory Pattern 
        //StateMachine.State patrol = new StateMachine.State();
        patrol = stateMachine.CreateState("Patrol");
        patrol.onEnter = delegate { Debug.Log("Patrol.onEnter"); };
        patrol.onExit = delegate { Debug.Log("Patrol.onExit"); };
        patrol.onFrame = PatrolOnFrame; 

        chase = stateMachine.CreateState("Chase");
        chase.onEnter = delegate { Debug.Log("Chase.onEnter"); };
        chase.onExit = delegate { Debug.Log("Chase.onExit"); };
        chase.onFrame = ChaseOnFrame;

        attack = stateMachine.CreateState("Attack");
        attack.onEnter = delegate { Debug.Log("Attack.onEnter"); };
        attack.onExit = delegate { Debug.Log("Attack.onExit"); };
        attack.onFrame = AttackOnFrame;


        runAway = stateMachine.CreateState("RunAway");
        runAway.onEnter = delegate { Debug.Log("RunAway.onEnter"); };
        runAway.onExit = delegate { Debug.Log("RunAway.onExit"); };
        runAway.onFrame = RunAwayOnFrame;


    }
    void PatrolOnFrame()
    {
        Debug.Log("Patrol.onFrame");
        //DEAFULT actions during Patrol state
        //print("Patrolling...");
        Patrol();

        //CHECK TRANSITION CONDITIONS
        //T1 - Sense Enemy
        if (Utilities.SenseEnemy(this.transform.position
            , enemy.transform.position
            , this.transform.forward
            , cosGuardFOVover2InRAD
            , closeEnoughSenseCutoff))
        {
            stateMachine.ChangeState(chase);
        }
        //T3 - ThreatenedAndWeakerThanEnemy
        if (ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }
    }
    void ChaseOnFrame()
    {
        Debug.Log("Chase.onFrame");

        //DEAFULT actions 
        print("Chase...");
        Chase();

        //CHECK TRANSITION CONDITIONS
        //T2 - Within Range and Stronger
        if (WithinRangeAndStrongerThanEnemy())
        {
            stateMachine.ChangeState(attack);
        }

        if (WeakerThanEnemy())
        {
            stateMachine.ChangeState(patrol);
        }
    }

    private void Chase()
    {
        //E.heading = (E - G).

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;

        enemyHeading.Normalize();
        //rb.velocity=enemyHeading*speed;
        //
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s *s/frame=  m/frame 
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;

    }

    private bool WithinRangeAndStrongerThanEnemy()
    {
        //throw new NotImplementedException();
        if (WithinRange() && !WeakerThanEnemy())
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


    void AttackOnFrame()
    {
        Debug.Log("Attack.onFrame");
        //DEAFULT actions 
        print("Attack...");

        //CHECK TRANSITION CONDITIONS
        //T3 - ThreatenedAndWeakerThanEnemy
        if (ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }
        if (!Utilities.SenseEnemy(this.transform.position
            , enemy.transform.position
            , this.transform.forward
            , cosGuardFOVover2InRAD
            , closeEnoughSenseCutoff))
            {
            stateMachine.ChangeState(patrol);
        }
        if (WeakerThanEnemy())
        {
            stateMachine.ChangeState(patrol);
        }
    }

    void RunAwayOnFrame()
    {
        Debug.Log("RunAway.onFrame");
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        
    }

    private void Patrol()
    {
        if (Vector3.Distance(this.transform.position, waypoints[nextWaypointIndex].position) < float.Epsilon)
        {
            nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Length;
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


    private bool EnemyCloseEnough(float distance)
    {

        if (Vector3.Distance(this.transform.position, enemy.transform.position) <= distance)
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
        float dAlpha = GuardFOV / pointsArray.Length;
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
