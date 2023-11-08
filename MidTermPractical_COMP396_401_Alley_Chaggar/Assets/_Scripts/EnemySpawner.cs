using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesPerWave = 3;
    public float waveInterval = 10f;
    private bool playerHasPowerUp = false;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        for (int wave = 1; wave <= 3; wave++)
        {
            yield return new WaitForSeconds(waveInterval);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Adjust enemy behavior based on player's power-up status
                Hunt enemyHunt = newEnemy.GetComponent<Hunt>();
                Retreating retreating = newEnemy.GetComponent<Retreating>();

                if (enemyHunt != null && retreating != null)
                {
                    if (playerHasPowerUp)
                    {
                        enemyHunt.Hunting();
                    }
                    else
                    {
                        retreating.Retreat();
                    }
                }
                else
                {
                    Debug.LogWarning("Enemy is missing either the Hunt or Retreating script.");
                }
            }
        }
    }

    public void PlayerHasPowerUp(bool hasPowerUp)
    {
        playerHasPowerUp = hasPowerUp;
    }
}
