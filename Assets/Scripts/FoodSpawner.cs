using System;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    private Func<Vector3, bool> checkOccupiedCallback;
    private Vector2 spawnRange;

    public void Init(Vector2 gameFieldRange, Func<Vector3, bool> checkOccupiedCallback)
    {
        spawnRange = gameFieldRange;
        this.checkOccupiedCallback = checkOccupiedCallback;
        SpawnFood();
    }

    public void SpawnFood()
    {
        // Generate a random position within the game field limits
        Vector3 randomPosition = GetRandomPosition();

        // Check if the generated position is occupied by the snake
        while (IsOccupied(randomPosition))
        {
            randomPosition = GetRandomPosition();
        }

        // Instantiate the food at the generated position
        GameObject newFood = Instantiate(foodPrefab, randomPosition, Quaternion.identity);
        newFood.transform.SetParent(transform);
    }

    private Vector3 GetRandomPosition()
    {
        // Generate a random position within the defined range, taking into account the offset of 1 to avoid spawning within the boundaries
        int x = UnityEngine.Random.Range((int)spawnRange.x + 1, (int)spawnRange.y - 1);
        int y = UnityEngine.Random.Range((int)spawnRange.x + 1, (int)spawnRange.y - 1);

        return new Vector3(x, y, 0f);
    }

    private bool IsOccupied(Vector3 position)
    {
        if (checkOccupiedCallback != null)
        {
            return checkOccupiedCallback(position);
        }

        return false;
    }
}
