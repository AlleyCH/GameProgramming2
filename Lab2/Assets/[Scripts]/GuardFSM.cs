using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class GuardFSM : MonoBehaviour
{
    public Transform[] waypoints;
    public GameObject enemy;
    public float closeEnoughCutoff = 2;
    public float closeEnoughSenseCutoff = 50;
    public float GuardFOV = 89; //degrees
    private float cosGuardFOVover2InRad;
    public float strength = 90;
    public float speed = 2;
    public int nextWaypointsIndex = 0;
    public float closeEnoughAttackCutoff = 2;


    public enum GuardState
    {
        Patrol, Chase, Attack, RunAway
    }

    public GuardState currentState;

    // Start is called before the first frame update
    void Start()
    {

        cosGuardFOVover2InRad = Mathf.Cos(GuardFOV / 2f * Mathf.Deg2Rad);

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
        print("Run Away...");
        RunAway();
        // sense player
        if (Safe())
        {
            ChangeState(GuardState.Patrol);
        }


    }

    private void RunAway()
    {
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        float enemyDistance = enemyHeading.magnitude;
        enemyHeading.Normalize();

        Vector3 movement = enemyHeading * speed * Time.deltaTime;

        Vector3.ClampMagnitude(movement, enemyDistance);
        this.transform.position -= movement;


        // throw new NotImplementedException();
    }

    private bool Safe()
    {
        return !Threatened();
        //throw new NotImplementedException();
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
        //throw new NotImplementedException();
    }

    private bool Threatened()
    {
        return EnemyCloseEnough(closeEnoughSenseCutoff);
        //throw new NotImplementedException();
    }

    private void HandleAttack()
    {
        print("Attacking...");

        if (ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway);
        }
        // throw new NotImplementedException();
    }

    private void HandleChase()
    {
        print("Chasing...");

        Chase();

        if (WithinRangeAndStrongerThanEnemy())
        {
            ChangeState(GuardState.Attack);
        }


        //throw new NotImplementedException();
    }

    private void Chase()
    {

        Vector3 enemyHeading = (enemy.transform.position - this.transform.position);
        enemyHeading.Normalize();

        // Calculate the movement vector with a fixed speed
        Vector3 movement = enemyHeading * speed * Time.deltaTime;

        // Update the guard's position based on the movement vector
        transform.position += movement;
    }

    private bool WithinRangeAndStrongerThanEnemy()
    {
        if (WithinRange() && !WeakerThanEnemy())
        {
            return true;
        }
        else
        {
            return false;
        }
        //throw new NotImplementedException();
    }

    private bool WeakerThanEnemy()
    {
        PlayerController enemyController = enemy.GetComponent<PlayerController>();
        if (this.strength < enemyController.strength)
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
        // throw new NotImplementedException();
    }

    private void HandlePatrol()
    {
        print("Patrolling...");
        Patrol();

        if (SenseEnemy())
        {
            ChangeState(GuardState.Chase);
        }

        // threatenedandweakerthanenemy
        if (ThreatenedAndWeakerThanEnemy())
        {
            ChangeState(GuardState.RunAway);
        }
        // throw new NotImplementedException();
    }

    private void Patrol()
    {

        if (Vector3.Distance(this.transform.position, waypoints[nextWaypointsIndex].position) < float.Epsilon)
        {
            nextWaypointsIndex = (nextWaypointsIndex + 1) % waypoints.Length;
        }
        Vector3 target = waypoints[nextWaypointsIndex].position;
        Vector3 movement = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
        this.transform.position = movement;
    }

    private bool SenseEnemy()
    {
        if (EnemyInFront() && EnemyCloseEnough(closeEnoughSenseCutoff))
        {
            return true;
        }
        else
        {
            return false;
        }
        //throw new NotImplementedException();
    }

    private bool EnemyCloseEnough(float distance)
    {
        if (Vector3.Distance(this.transform.position, enemy.transform.position) <= distance) {
            return true;
        }
        else
        {
            return false;
        }

        //throw new NotImplementedException();
    }

    private bool EnemyInFront()
    {
        //Angle(GameObject.fwd, EasingFunction.heading) < GuardFOV/2 => true
        //throw new NotImplementedException();
        Vector3 enemyHeading = (enemy.transform.position - this.transform.position).normalized;
        float cosAngle = Vector3.Dot(enemyHeading, this.transform.forward);
        if (cosAngle > cosGuardFOVover2InRad)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChangeState(GuardState newGuardState)
    {
        currentState = newGuardState;
        //throw new NotImplementedException();
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




/*   . What changes you needed to make to accommodate the addition of one extra state?
           
        - I didn't need to change anything because the state I added was to the player instead of the guard. 
The statewas player movement. 

     . How are the changes dependent on the number of transitions from/to this newly added state?
        
        -  It impacts the complexity of the code. Chase is now dependent on the player moving close to the guard

     . What changes you predict you need to make to accommodate the removal of one state?

        - If I had to remove patrol then I wouldn't need the waypoints and would need to change the default state. To something else like chasing instead.

     . How are these changes you predict dependent on the states you start with and number of transitions from/to the state to be removed?

        -  The guards chase state is now dependant on the players movement and vicinity. 

     . Is this method sustainable if you have a prior knowledge that the lead game designer is fond of changing (adding/removing) states a lot? 

        - It would end up getting very complicated and error-prone
*/