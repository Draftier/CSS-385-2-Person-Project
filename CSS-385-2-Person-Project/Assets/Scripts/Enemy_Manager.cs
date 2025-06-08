
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class Enemy_Manager : MonoBehaviour
{
    public List<GameObject> enemyTypes;
    public int baseNumber = 10;
    public int increasePerWave = 5;

    public int curr_wave = 1;
    public TextMeshProUGUI wave_counter;
    public static Enemy_Manager Instance;
    private bool isSpawning;

    private void Start()
    {
        wave_counter.text = "Wave: 0";
    }

    private void Update()
    {
        if (Enemy.enemyCount == 0)
        {
            SpawnNextWave();
        }
    }


    IEnumerator SpawnWave()
    {
        int total = baseNumber + increasePerWave * (curr_wave - 1);

        for (int i = 0; i < total; i++)
        {
            // Choose random enemy
            GameObject prefab = enemyTypes[Random.Range(0, enemyTypes.Count)];

            // Spawn at random off-screen position
            Vector2 spawnPos = GetOffScreenSpawnPosition();
            Instantiate(prefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(2f);
        }
        curr_wave++;
        isSpawning = false;
    }

    Vector2 GetOffScreenSpawnPosition()
    {
        Camera cam = Camera.main;
        float buffer = 1f; // how far outside the screen enemies spawn
        Vector2 screenSize = new Vector2(cam.orthographicSize * cam.aspect, cam.orthographicSize);

        // Choose a random side: 0 = left, 1 = right, 2 = top, 3 = bottom
        int side = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector2(-screenSize.x - buffer, Random.Range(-screenSize.y, screenSize.y));
                break;
            case 1: // Right
                spawnPos = new Vector2(screenSize.x + buffer, Random.Range(-screenSize.y, screenSize.y));
                break;
            case 2: // Top
                spawnPos = new Vector2(Random.Range(-screenSize.x, screenSize.x), screenSize.y + buffer);
                break;
            case 3: // Bottom
                spawnPos = new Vector2(Random.Range(-screenSize.x, screenSize.x), -screenSize.y - buffer);
                break;
        }

        // Offset from camera position
        return spawnPos + (Vector2)cam.transform.position;
    }

    public void SpawnNextWave()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnWave());
            wave_counter.text = "Wave: " + curr_wave;
            isSpawning = true;
        }
    }
}
