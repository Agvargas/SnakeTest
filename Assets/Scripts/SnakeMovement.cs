using UnityEngine;
using System.Collections.Generic;
using System;

public class SnakeMovement : MonoBehaviour
{
    public GameObject bodySegmentPrefab;
    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public List<Transform> snakeBody; // List to store the snake's body segments
    public event Action FoodEaten; // Event triggered when food is eaten

    private Vector3 lastPos;
    private bool growIsPending;

    public void Init()
    {
        isDead = false;
        growIsPending = false;
        snakeBody = new List<Transform>();
        snakeBody.Add(transform); // Add the initial segment (the head) to the list
    }

    public void Move(Vector3 direction, Action callback)
    {
        lastPos = transform.position;
        Vector3 nextPosition = transform.position + direction;
        transform.position = nextPosition;
        MoveTail();

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void MoveTail()
    {
        for (int i = 1; i < snakeBody.Count; i++)
        {
            Vector3 tempPos = snakeBody[i].position;
            snakeBody[i].position = lastPos;
            lastPos = tempPos;
        }
        // Only grow up when move is done to avoid collision issues 
        if (growIsPending)
        {
            GrowSnake();
        }
    }

    public bool IsCollidingWithBody(Vector3 position)
    {
        // Check if the position collides with any segment of the snake's body to avoid it
        for (int i = 1; i < snakeBody.Count; i++)
        {
            if (position == snakeBody[i].position)
            {
                return true;
            }
        }

        return false;
    }

    private void Die()
    {
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            // Increase the length of the snake and destroy the food
            growIsPending = true;
            Destroy(collision.gameObject);
            // Generate new food
            FoodEaten?.Invoke();
        }
        else if (collision.CompareTag("Boundary") || collision.CompareTag("SnakeBody"))
        {
            // The snake has collided with the boundaries of the game field or itself
            Die();
        }
    }

    private void GrowSnake()
    {
        // Instantiate a new body segment at the position of the last segment
        GameObject newSegment = Instantiate(bodySegmentPrefab, lastPos, Quaternion.identity);
        snakeBody.Add(newSegment.transform);
        growIsPending = false;
    }
}
