using Pada1.BBCore;
using Pada1.BBCore.Tasks;
using BBUnity.Actions;
using UnityEngine;

[Action("Chapter08/ShootOnce")]
[Help("Clone a 'bullet' and shoots it through the Forward axis with the " +
        "specified velocity.")]

public class ShootOnce : GOAction
{
    [InParam("shootPoint")]
    public Transform shootPoint;

    [InParam("bullet")]
    public GameObject bullet;

    [InParam("velocity", DefaultValue = 30f)]
    public float velocity;


    public override void OnStart()
    {
        if (shootPoint == null)
        {
            shootPoint = gameObject.transform.Find("shootPoint");
            if (shootPoint == null)
            {
                Debug.LogWarning("Shoot point not specified. ShootOnce will not work " + "for " + gameObject.name);
            }
        }
        base.OnStart();
    }

    public override TaskStatus OnUpdate()
    {
        if (shootPoint == null)
        {
            return TaskStatus.FAILED;
        }
        GameObject newBullet = GameObject.Instantiate(
                                    bullet, shootPoint.position,
                                    shootPoint.rotation * bullet.transform.rotation
                                ) as GameObject;
        // Give it a velocity
        if (newBullet.GetComponent<Rigidbody>() == null)
        {
            newBullet.AddComponent<Rigidbody>();
        }
        newBullet.GetComponent<Rigidbody>().velocity = velocity * shootPoint.forward;
        return TaskStatus.COMPLETED;
    }
}

