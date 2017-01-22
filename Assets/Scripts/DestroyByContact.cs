using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    private GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Destroy anything that touches the boundary
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Boundary"))
        {
            // If we hit the Player, use the player explosion too
            if (other.CompareTag("Player"))
            {
                // If player is invincible, do nothing
                if (gameController.IsPlayerInvincible())
                    return;

                Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                gameController.PlayerHit();
            }

            // Only credit the explosion if it is from a player bolt
            if (other.CompareTag("PlayerWeapon"))
                gameController.AddScore(scoreValue);

            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
