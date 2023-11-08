using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFuzzyFunctions : MonoBehaviour
{
    //Parameters
    [Header("Parameters")]

    [Tooltip("a and b parameters for LeftShoulder")]
    public float a_lsh=.1f, b_lsh=.4f;

    [Tooltip("a and b parameters for RightShoulder")]
    public float a_rsh=.3f, b_rsh=.7f;

    [Tooltip("a,b and c parameters for Triangular")]
    public float a_tri=.15f, b_tri=.45f, c_tri=.85f;

    public float a_tra=.2f, b_tra=.4f, c_tra=.55f, d_tra=.8f;

    public float a_crisp=.35f;

    [Tooltip("dx is the interval between x-s of points")]
    public float dx = .05f;

    [Header("Calculated Quantities")]
    [Tooltip("Number Of Points")]
    public int NumberOfPoints = System.Convert.ToInt32(1f / .05f) + 1;


    //
    public float[] xs;
    public float[] ys_lsh;
    public float[] ys_rsh;
    public float[] ys_tri;
    public float[] ys_tra;
    public float[] ys_crisp;
    public float[] ys_s_curve;


    // Start is called before the first frame update
    void Start()
    {
        xs = new float[NumberOfPoints];
        ys_lsh = new float[NumberOfPoints];
        ys_rsh = new float[NumberOfPoints];
        ys_tri = new float[NumberOfPoints];
        ys_tra=new float[NumberOfPoints];
        ys_crisp = new float[NumberOfPoints];
        ys_s_curve = new float[NumberOfPoints];

        LineRenderer lr_lsh = GameObject.Find("LSh_Visual").GetComponent<LineRenderer>();
        lr_lsh.positionCount = NumberOfPoints;
        for (int i = 0;i< NumberOfPoints; i++)
        {
            xs[i] = i*dx;
            ys_lsh[i] = FuzzyFunctions.LeftShoulder(xs[i], a_lsh, b_lsh);
            print($"x={xs[i]},y={ys_lsh[i]}");
            //For visualizing later you can use LineTrailer comp.
            lr_lsh.SetPosition(i,new Vector3(xs[i],ys_lsh[i],0));

        }



    }


}
