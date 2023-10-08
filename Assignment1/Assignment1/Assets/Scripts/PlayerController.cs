using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float strength = 100; //[0,100]
    public float speed = 3;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    public void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 dir = input.normalized;
        Vector3 vel = dir * speed * Time.deltaTime;

        transform.Translate(vel);
    }

    public void TakeDamage(float damage)
    {
        if (strength > 0)
        {
            strength -= damage;
        }
        if (strength <= 0)
        {

            strength = 0;
            print("You died!");
        }


    }

    public void TakeHealingPotion(float healing_strength)
    {
        if (strength < 100)
        {
            strength += healing_strength;
        }
        if (strength >= 100)
        {

            strength = 100;
            print("You have max strength!");
        }
    }
}

