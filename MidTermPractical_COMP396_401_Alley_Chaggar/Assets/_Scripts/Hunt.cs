using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunt : MonoBehaviour
{
    public Transform player;
    public float huntingSpeed = 5f;
    public float fov = 60f; // Field of view in degrees
    public float maxDistance = 6f;

    private Renderer enemyRenderer;
    private bool isHunting = false;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Hunting();
    }
    public void Hunting()
    {
        // Calculate the direction to the player
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if the player is within the FOV and distance limits
        if (angleToPlayer < fov / 2 && directionToPlayer.magnitude < maxDistance)
        {
            // Player is visible and within range
            isHunting = true;
            enemyRenderer.material.color = Color.red;

            // Move towards the player
            Vector3 moveDirection = directionToPlayer.normalized;
            transform.Translate(moveDirection * huntingSpeed * Time.deltaTime);
        }
        else
        {
            // Player is not visible or out of range
            isHunting = false;
            enemyRenderer.material.color = Color.blue;
        }
    }
}
