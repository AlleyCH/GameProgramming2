using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    StateMachine stateMachine;

    public StateMachine.State patrol, chase, attack, runAway, dead;
    public float GuardFOV = 89; //degrees

    public GameObject enemy;
    public float guardFOV = 89; //degrees
    private float cosGuardFOVover2InRAD;

    public float closeEnoughAttackCutoff = 3; //if distance of guard to enemy is less than or equal to this value, closeenough is true
    public float senseCutOff = 15; //start chasing at this distance
    public float strength = 90; //[0,100]
    public float speed = 2; //2 m/s

    public Transform[] waypoints;
    public int nextWaypointIndex = 0;

    void Start()
    {
        cosGuardFOVover2InRAD = Mathf.Cos(guardFOV / 2f * Mathf.Deg2Rad); //changes degrees to radians

        stateMachine = new StateMachine();

     
        dead = stateMachine.CreateState("Dead");
        dead.onEnter = delegate { Debug.Log("dead.onEnter"); };
        dead.onExit = delegate { Debug.Log("dead.onExit"); };
        dead.onFrame = DeadOnFrame;

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

    public static bool SensePlayer(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float senseCutOff, float distance)
    {
        //Player in front and close enough
        if (EnemyInFront(start, enemyPos, thisForward, senseCutOff) && EnemyCloseEnough(start, enemyPos, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    
    }

    void PatrolOnFrame()
    {
        Debug.Log("Patrol.onFrame");
        //default actions
        Patrol();
      
        if (SensePlayer(this.transform.position, enemy.transform.position,
            this.transform.forward, cosGuardFOVover2InRAD, senseCutOff))
        {
            stateMachine.ChangeState(chase);
        }
        //weaker, run
        if (ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }

    }

    private void Patrol()
    {
        
        if (Vector3.Distance(this.transform.position, waypoints[nextWaypointIndex].position) < 0.1f)
        {
            nextWaypointIndex = (nextWaypointIndex + 1) % waypoints.Length;
        }

        Vector3 target = waypoints[nextWaypointIndex].position;
        Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);

        this.transform.position = movement;
    }
    void AttackOnFrame()
    {
        Debug.Log("Attack.onFrame");

        //check transition condition
        if (ThreatenedAndWeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }
        if (!SensePlayer(this.transform.position, enemy.transform.position,
            this.transform.forward, cosGuardFOVover2InRAD, closeEnoughAttackCutoff))
        {
            stateMachine.ChangeState(patrol);
        }
    }


    private bool ThreatenedAndWeakerThanEnemy()
    {
        if (EnemyCloseEnough(closeEnoughAttackCutoff) && WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    void ChaseOnFrame()
    {
        Debug.Log("Chase.onFrame");
        Chase();
        //check transition condition
        if (WithinRangeAndStrongerThanEnemy())
        {
            stateMachine.ChangeState(attack);
        }
        if (WeakerThanEnemy())
        {
            stateMachine.ChangeState(runAway);
        }
    }

    private void Chase()
    {
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;

        enemyHeading.Normalize();
        //rb.velocity = enemyHeading * speed;
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s * 2/frame = m/frame
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position += movement;
    }

    void RunAwayOnFrame()
    {
        Debug.Log("RunAway.onFrame");
        RunAway();
        //check transition condition        
        if (Safe())
        {
            stateMachine.ChangeState(patrol);
        }
    }

    private void RunAway()
    {

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;

        enemyHeading.Normalize();
        //rb.velocity = enemyHeading * speed;
        Vector3 movement = enemyHeading * speed * Time.deltaTime; //m/s * 2/frame = m/frame
        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position -= movement;
    }

    private bool Safe()
    {
        return !EnemyCloseEnough(closeEnoughAttackCutoff);
    }

    void DeadOnFrame()
    {   
        Debug.Log("Dead.onFrame");
        if (this.strength <=0)
        {
            Dead();
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

    private bool EnemyCloseEnough(float distance)
    {
        if (Vector3.Distance(this.transform.position, enemy.transform.position) <= distance)
        {
            return true;
        }
        else
        { return false; }
    }

    private bool WithinRangeAndStrongerThanEnemy()
    {

        if (EnemyCloseEnough(closeEnoughAttackCutoff) && !WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static bool EnemyInFront(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float senseCutOff)
    {
       
        Vector3 enemyHeading = (enemyPos - start).normalized;
     
        float cosAngle = Vector3.Dot(enemyHeading, thisForward);
        if (cosAngle > senseCutOff)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public static bool EnemyCloseEnough(Vector3 start, Vector3 enemyPos, float distance)
    {
        if (Vector3.Distance(start, enemyPos) <= distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (strength > 0)
        {
            strength -= damage;
            print($"Guard has taken {damage} damage");
        }
        if (strength <= 0)
        {
            strength = 0;
            stateMachine.ChangeState(dead);
        }
    }

    private void Dead()
    { 
        print("Guard is dead...");
        Destroy(gameObject);
    }



    void Update()
    {
        stateMachine.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
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
        // See cone from fwd - FOV/2 to fwd+FOV/2
        //Gizmos.DrawFrustum(this.transform.forward, GuardFOV/10f,closeEnoughSenseCutoff, 10f, 1f);
        Vector3[] pointsArray = new Vector3[10];
        float dAlpha = GuardFOV / pointsArray.Length;
        Vector3 fwdInWorldSpace = this.transform.TransformDirection(this.transform.forward);

        for (int i = 0; i < pointsArray.Length / 4; i++)
        {
            float dAlphaPlus = dAlpha * i * Mathf.Deg2Rad;
            float dAlphaMinus = -dAlphaPlus;
            Vector3 target = new Vector3(MathF.Cos(dAlphaPlus), 0, MathF.Sin(dAlphaPlus));
            Vector3 v = Vector3.RotateTowards(fwdInWorldSpace, target, dAlphaPlus, 10);


            pointsArray[2 * i] += this.transform.position;
            pointsArray[2 * i + 1] = this.transform.position + v * 10f;
            //pointsArray[2 * i + 1] += this.transform.position;
            ///pointsArray[2 * i + 1] = (new Vector3(Mathf.Cos(dAlphaMinus), 0,  Mathf.Cos(dAlphaMinus)))* closeEnoughSenseCutoff;

        }
      
        pointsArray[0] = this.transform.position;
        pointsArray[1] = this.transform.position +fwdInWorldSpace*10;
        ReadOnlySpan<Vector3> points = new ReadOnlySpan<Vector3>(pointsArray);
        Gizmos.DrawLineList(points);
  
   }   
}
