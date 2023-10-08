using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
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

    public static bool EnemyInFront(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float cutOff)
    {
        //Angle(Guard.Fwd, E.heading) < GuardFOV/2 => true, else false
        // <=> cos(angle)>cos(GuardFOV/2)
        //throw new NotImplementedException();
        //E.heading = (E - G).
        Vector3 enemyHeading = (enemyPos - start).normalized;
        //if(Vector3.Angle(enemyHeading, this.transform.forward))
        //{
        //    return true;
        //}
        float cosAngle = Vector3.Dot(enemyHeading, thisForward);
        if (cosAngle > cutOff)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public static bool SenseEnemy(Vector3 start, Vector3 enemyPos, Vector3 thisForward, float cutOff, float dist)
    {

        //Case1: Enemy in front and close enough
        if (EnemyInFront(start, enemyPos, thisForward, cutOff)
            && EnemyCloseEnough(start, enemyPos, dist))
        {
            return true;
        }
        else
        {
            return false;
        }


        //Case2: Guard hears the enemy footsteps

        //...

        //CaseN: Smells the enemy
    }

    public static float[] MySortAscending(float[] xs)
    {
        //in ascending order
        float[] res = new float[xs.Length];
        //Naive sorting
        //x0 x1 ... xn-1
        //start with x0,
        // compare each other value x1,...xn-1 with x0; if it is less, swap it
        // goto x1
        // 
        for (int i = 0; i < xs.Length - 1; i++)
        {
            for (int j = i + 1; j < xs.Length; j++)
            {
                if (xs[i] > xs[j])
                {
                    //swap xi with xj
                    float temp = xs[i];
                    xs[i] = xs[j];
                    xs[j] = temp;
                }
            }
        }

        //Analysis:
        //Let n=xs.Length
        //Then it can be provved that is an O(n^2) algo, in the worse case
        //Proof:
        //Outer cycle run n-1 times
        //n-1, n-2 n-3, ..., n-(n-1)=1
        // (n-1)+(n-2)+...+1  = (n-1)*(n)/2 = n^2/2-n/2
        //  f(n)=n^2/2-n/2
        // What is O(f(n)) = O(n^2)

        //n/2, n/2 elements are not sorted  O(n^2/4)=O(n^2) 

        return res;

    }

    public static float MyMax(float[] xs)
    {
        if (xs.Length == 0)
        {
            throw new System.Exception("array has no elements!");
        }
        else if (xs.Length == 1)
        {
            return xs[0];
        }
        //xs.Length>1
        float res = xs[0];
        for (int i = 1; i < xs.Length; i++)
        {
            if (xs[i] > res)
            {
                res = xs[i];
            }
        }
        return res;

    }
}

