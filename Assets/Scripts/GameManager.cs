using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public int pointsForEat = 10;
    public float moveInterval = 0.5f;
    public float restartDelay = 5f;
    public Vector2 gameFieldRange = new Vector2(-7f, 7f);

    [Header("References")]
    public SnakeMovement snakeMovement;
    public FoodSpawner foodSpawner;
    public BoundarySpawner boundarySpawner;
    public GameObject gameOverUI;
    public TextMeshProUGUI scoreText;

    private int score;
    private float timer = 0f;
    private bool canChangeDirection = true;
    private Vector3 direction;

    private void Start()
    {
        // Initialization
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        direction = Vector3.right;
        gameOverUI.SetActive(false);
        boundarySpawner.Init(gameFieldRange);
        foodSpawner.Init(gameFieldRange, IsPositionOccupied);
        snakeMovement.Init();

        snakeMovement.FoodEaten += FoodEaten;// Subscribe to the FoodEaten event

        timer = moveInterval;
    }

    private void Update()
    {
        if (snakeMovement.isDead)
        {
            // Game Over
            GameOver();
            Invoke("RestartGame", restartDelay);
            return;
        }

        // Update timer
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Move snake
            snakeMovement.Move(direction, ChangeDirection);
            timer = moveInterval;
        }

        // Read player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Change snake direction if a valid direction key is pressed
        if (canChangeDirection)
        {
            if (horizontalInput > 0f && direction != Vector3.left)
            {
                direction = Vector3.right;
                canChangeDirection = false;
            }
            else if (horizontalInput < 0f && direction != Vector3.right)
            {
                direction = Vector3.left;
                canChangeDirection = false;
            }
            else if (verticalInput > 0f && direction != Vector3.down)
            {
                direction = Vector3.up;
                canChangeDirection = false;
            }
            else if (verticalInput < 0f && direction != Vector3.up)
            {
                direction = Vector3.down;
                canChangeDirection = false;
            }
        }
    }

    private void ChangeDirection()
    {
        canChangeDirection = true;
    }

    private void FoodEaten()
    {
        SpawnFood();
        UpdateScore();
    }

    private void UpdateScore()
    {
        // Update score and score text
        score += pointsForEat;
        scoreText.text = "Score: " + score.ToString();
    }

    private void SpawnFood()
    {
        // Spawn food using the food spawner
        foodSpawner.SpawnFood();
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        // Check if position is occupied by the snake's body
        bool isOccupied = snakeMovement.IsCollidingWithBody(position);
        return isOccupied;
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

