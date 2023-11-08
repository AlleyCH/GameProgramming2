using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool hasPowerUp = false;
    public float movementSpeed = 5f;

    private void Update()
    {
        // Input handling for state transitions
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hasPowerUp)
            {
                DeactivatePowerUp();
            }
            else
            {
                ActivatePowerUp();
            }
        }

        // Input handling for player movement
        if (hasPowerUp == true || hasPowerUp == false)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void ActivatePowerUp()
    {
        hasPowerUp = true;
    }

    private void DeactivatePowerUp()
    {
        hasPowerUp = false;
    }
}
