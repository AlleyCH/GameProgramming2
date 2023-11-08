using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TestBigOh : MonoBehaviour
{
    public int NumberOfElements = 1000;
    public float[] xs;
    public float[] xs_sorted;
    // Start is called before the first frame update
    void Start()
    {
        xs= new float[NumberOfElements];
        xs_sorted = new float[NumberOfElements];
        ArrayList xs_al = new ArrayList();
        //TimeSpan timeSpan = new TimeSpan(0);
        //Populate xs
        //Populate xs_al
        //System.Timers.Timer timer = new System.Timers.Timer(1000);
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        print($"Start popultating: NumberOfElements={NumberOfElements}:"+Time.time);
        for (int i=0; i< xs.Length; i++)
        {
            xs[i]=UnityEngine.Random.value;
            xs_al.Add(xs[i]);
        }
        long populatingMS=stopwatch.ElapsedMilliseconds;
        print($"populatingMS={populatingMS}");

        //Sort with ArraList.Sort
        print($"Start ArraList.Sort");
        xs_al.Sort();
        //Measure how long it took for sorting
        long al_sortingMS = stopwatch.ElapsedMilliseconds-populatingMS;
        print($"al_sortingMS={al_sortingMS}");

        //Sort with MySortAscending
        print($"Start Utilities.MySortAscending");
        xs_sorted = Utilities.MySortAscending(xs);
        //Measure how long it took for sorting
        print("End of Sorting");
        long MySortAscendingMS = stopwatch.ElapsedMilliseconds - al_sortingMS;
        print($"MySortAscendingMS={MySortAscendingMS}");


    }
}
