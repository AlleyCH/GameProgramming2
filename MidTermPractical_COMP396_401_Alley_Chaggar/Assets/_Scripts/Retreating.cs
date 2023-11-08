using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retreating : MonoBehaviour
{
    public Transform player;
    public float retreatSpeed = 3f;
    public float fov = 60f; // Field of view in degrees
    public float maxDistance = 6f;
  
    public PlayerController playerController;
    private Renderer enemyRenderer;
    private bool isRetreating = false;

    void Start()
    {

        enemyRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Retreat();
    }
    public void Retreat()
    {
        bool playerHasPowerUp = playerController.hasPowerUp;
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        // Check if the player is within the FOV and distance limits
        if (angleToPlayer < fov / 2 && distanceToPlayer < maxDistance)
        {
            // Player is visible and within range
            if (playerHasPowerUp && distanceToPlayer < 3f)
            {
                // Player has a power-up and is close, retreat
                isRetreating = true;
                enemyRenderer.material.color = Color.yellow;

                // Move away from the player
                Vector3 moveDirection = -directionToPlayer.normalized;
                transform.Translate(moveDirection * retreatSpeed * Time.deltaTime);
            }
            else
            {
                // Player is not retreating
                isRetreating = false;
                enemyRenderer.material.color = Color.red;
            }
        }
        else
        {
            // Player is not visible or out of range
            isRetreating = false;
            enemyRenderer.material.color = Color.blue;
        }
    }
}
