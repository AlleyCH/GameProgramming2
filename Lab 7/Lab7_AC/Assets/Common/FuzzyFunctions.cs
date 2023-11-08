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
        if( !(0<= a && a< b && b <= 1))
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

    //Triangular

    //Trapesoidal

    //Optional
    //Crisp

    //S_curve


}
