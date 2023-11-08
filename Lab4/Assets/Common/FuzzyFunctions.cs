using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FuzzyFunctions
{
    public static float LeftShoulder(float x, float a, float b)
    {
        float result = 0;
        //Guards
        //   0<=x<=1
        if(x<0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }
        // 0<=a < b <=1
        if( !(0<= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a},b={b}: They should obey 0<=a<b<=1");
        }

        if (x <= a)
        {
            result = 1;
        }else if (x <= b)
        {
            result = (b - x) / (b - a);
        }

        return result;
    }

    //Right Shoulder

    public static float RightShoulder(float x, float a, float b)
    {
        float result = 0;

        // Guards
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }

        if ( !(0 <= a && a < b && b <= 1))
        {
            throw new System.Exception($"a={a}, b={b}: They should obey 0<=a<b<=1");
        }

        if (x >= b)
        {
            result = 1;
        }
        else if (x >= a)
        {
            result = (x - a) / (b - a);
        }

        return result;
    }

    //Triangular

    public static float Triangular(float x, float a, float b, float c)
    {
        float result = 0;

        // guards
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }

        if ( !(0 <= a && a < b && b <= c && c <= 1))
        {
            throw new System.Exception($"a={a}, b={b}, c={c}: They should obey 0<=a<b<=c<=1");
        }

        if (x >= a && x <= b)
        {
            result = (x - a) / (b - a);
        }
        else if (x >= b && x <= c)
        {
            result = (c - x) / (c - b);
        }

        return result;
    }

    //Trapesoidal

    public static float Trapezoidal(float x, float a, float b, float c, float d)
    {
        float result = 0;

        // Guards
        if (x < 0 || x > 1)
        {
            throw new System.Exception($"x={x}: It should be in [0,1]");
        }

        if ( !(0 <= a && a < b && b <= c && c < d && d <= 1))
        {
            throw new System.Exception($"a={a}, b={b}, c={c}, d={d}: They should obey 0<=a<b<=c<d<=1");
        }

        if (x >= a && x <= b)
        {
            result = (x - a) / (b - a);
        }
        else if (x >= b && x <= c)
        {
            result = 1;
        }
        else if (x >= c && x <= d)
        {
            result = (d - x) / (d - c);
        }

        return result;
    }

    //Optional
    //Crisp
    public static float Crisp(float x, float a)
    {
        float result;

        if (x == a)
        {
            return result = 1;
        }
        else
        {
            return result = 0;
        }
    }

    //S_curve

    public static float SCurve(float x)
    {
        float result = 1 / (1 + Mathf.Exp(-x));
        return result;
    }
}
