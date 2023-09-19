using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVector3 : MonoBehaviour
{
    public float x, y, z;

    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public float magnitude()
    {
        return Mathf.Sqrt(x * x + y * y + z * z);
    }

    public MyVector3 normalized()
    {
        float mySize = this.magnitude();
        return new MyVector3(this.x / mySize, this.y / mySize, this.z / mySize);
    }

    public float Dot(MyVector3 v1, MyVector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    public MyVector3 Cross(MyVector3 v1, MyVector3 v2)
    {
        float x = v1.y * v2.z - v2.y * v1.z;
        float y = v1.z * v2.x - v2.z * v1.x;
        float z = v1.x * v2.y - v2.x * v1.y;

        return new MyVector3(x, y, z);
    }

    public static MyVector3 operator *(MyVector3 v1, float k)
    {
        return new MyVector3(v1.x * k, v1.y * k, v1.z * k);
    }

    public static MyVector3 operator *(float k, MyVector3 v1)
    {
        return new MyVector3(v1.x * k, v1.y * k, v1.z * k);
    }

    public static MyVector3 operator /(MyVector3 v1, float k)
    {
        if (k == 0)
        {
            throw new System.Exception("Dividing by 0");
        }
        else
        {
            return v1 * (1 / k);
        }
    }

    public static MyVector3 operator +(MyVector3 v1, MyVector3 v2)
    {
        return new MyVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static MyVector3 operator -(MyVector3 v1, MyVector3 v2)
    {
        return v1 + (-v2);
    }

    public static MyVector3 operator -(MyVector3 v)
    {
        return new MyVector3(-v.x, -v.y, -v.z);
    }

    void TestVector3()
    {
        Vector3 v1 = new Vector3(1.5f, 2.5f, 3.5f);
        Vector3 v2 = new Vector3(1.0f, -3.5f, 1.5f);

        print("v1=" + v1);
        print("v2=" + v2);
        print("Add Them" + (v1 + v2));
        print("Subtract them" + (v1 - v2));

        float v1dotv2 = Vector3.Dot(v1, v2);
        Vector3 v1xv2 = Vector3.Cross(v1, v2);
        float v1size = v1.magnitude;
        float v2size = v2.magnitude;

        print("v1dotv2 = " + v1dotv2);
        print("v1xv2" + v1xv2);
        print("v1size = " + v1size);
        print("v2size = " + v2size);

        Vector3 v1u = v1.normalized;
        Vector3 v1u_2 = v1 / v1size;
        print("v1u-v1u_2" + (v1u - v1u_2));
    }

    void TestMyVector3()
    {
        MyVector3 v1 = new MyVector3(1.5f, 2.5f, 3.5f);
        MyVector3 v2 = new MyVector3(1.0f, -3.5f, 1.5f);

        print("v1=" + v1);
        print("v2=" + v2);
        print("v1+v2=" + (v1 + v2));
        print("v1-v2=" + (v1 - v2));
        float V1DotV2 = v1.Dot(v1, v2);
        float v1size = v1.magnitude();
        float v2size = v2.magnitude();
        print("v1dotv2=" + (V1DotV2));
        print("v1size=" + v1size);
        print("v2size=" + v2size);

        MyVector3 v1u = v1.normalized();
        MyVector3 v1u_2 = v1 / v1size;
        print("v1u-v1u_2" + (v1u - v1u_2));
    }

    void Start()
    {
        TestVector3();
        TestMyVector3();
    }

    // Update is called once per frame
    void Update()
    {

    }
}