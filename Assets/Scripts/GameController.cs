using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public GameObject enemyShip;
    public GameObject player;
    public Vector3 spawnValues;
    public int hazardCount;

    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int lives;

    public float startingEnemySpeed;
    public float levelSpeedIncrease;
    public int wavesPerLevel;

    public GameGuiText gameGuiText;

    private bool gameOver;
    private bool restart;

    private int score;
    private int level;
    private int wave;
    private float currentEnemySpeed;

    private bool playerInvincible;
    private float playerInvincibleEnds;
    
    void Start()
    {
        score = 0;
        level = 1;
        currentEnemySpeed = startingEnemySpeed;
        gameOver = false;
        restart = false;
        gameGuiText.restartText.text = "";
        gameGuiText.gameOverText.text = "";

        UpdateLives();
        UpdateScore();
        UpdateLevel();

        CreatePlayer();

        // Start spawning hazards and enemies
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        // Allows the game to be restarted
        if (restart && Input.GetKeyDown(KeyCode.R))
        {
            // Loads the scene named scene again, which restarts the game
            SceneManager.LoadScene("Main");
        }

        // Check for player invincibility
        if (playerInvincible && Time.time > playerInvincibleEnds)
        {
            playerInvincible = false;
        }
    }

    // Public method exposed to other parts of the application
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void AdjustLives(int newLivesAdjustment)
    {
        lives += newLivesAdjustment;
        UpdateLives();
    }

    public void PlayerHit()
    {
        AdjustLives(-1);

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            CreatePlayer();
        }
    }

    public bool IsPlayerInvincible()
    {
        return playerInvincible;
    }

    IEnumerator SpawnWaves()
    {
        // Wait a bit before starting the waves for player to get ready
        yield return new WaitForSeconds(startWait);

        while (1 == 1)
        {
            for (int wv = 1; wv <= wavesPerLevel; wv++)
            {
                wave = wv;
                UpdateLevel();

                for (int i = 0; i < hazardCount; i++)
                {
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;

                    GameObject hazard = ChooseRandomHazard();
                    GameObject hazardInstance = Instantiate(hazard, spawnPosition, spawnRotation);
                    hazardInstance.GetComponent<Mover>().speed = currentEnemySpeed * -1;

                    // Wait the spawnWait time before sending in another wave
                    yield return new WaitForSeconds(spawnWait);
                }

                // TODO: Put the enemy in randomly with the hazards, not just at end
                // Create one enemy at end of wave
                Vector3 enemyShipSpawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion enemyShipSpawnRotation = Quaternion.Euler(180f, 0.0f, 180f);
                GameObject enemyShipInstance = Instantiate(enemyShip, enemyShipSpawnPosition, enemyShipSpawnRotation);
                enemyShipInstance.GetComponent<EnemyShipController>().speed = currentEnemySpeed;
                enemyShipInstance.GetComponent<EnemyShipController>().boltSpeed = currentEnemySpeed * 3;

                yield return new WaitForSeconds(waveWait);

                if (gameOver)
                {
                    gameGuiText.restartText.text = "Press 'R' for Restart";
                    restart = true;
                    break;
                }
            }

            // Level is over
            level++;
            UpdateLevel();
            currentEnemySpeed += levelSpeedIncrease;
        }
    }

    void UpdateScore()
    {
        gameGuiText.scoreText.text = "Score: " + score;
    }

    void UpdateLives()
    {
        gameGuiText.livesText.text = "Lives: " + lives;
    }

    void UpdateLevel()
    {
        gameGuiText.levelText.text = "Level: " + level + "." + wave;
    }

    private GameObject ChooseRandomHazard()
    {
        int numPossibleHazards = hazards.Length;
        int random = Random.Range(0, numPossibleHazards);
        return hazards[random];
    }

    private void GameOver()
    {
        gameGuiText.gameOverText.text = "Game Over!";
        gameOver = true;
    }

    private void CreatePlayer()
    {
        Debug.Log("Creating player");
        Vector3 staringPosition = new Vector3(0, 0, 0);
        Quaternion startingRotation = Quaternion.Euler(0f, 0f, 0f);
        GameObject localPlayerObject = Instantiate(player, staringPosition, startingRotation);

        // Set temporary invincibility
        playerInvincible = true;
        playerInvincibleEnds = Time.time + 3;
        StartCoroutine(Blink(localPlayerObject.GetComponent<Renderer>(), 3.0f));
    }

    private IEnumerator Blink(Renderer gameObjectRenderer, float duration)
    {
        var endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            gameObjectRenderer.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.1f);

            gameObjectRenderer.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }
}

[System.Serializable]
public class GameGuiText
{
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
    public GUIText livesText;
    public GUIText levelText;
}
