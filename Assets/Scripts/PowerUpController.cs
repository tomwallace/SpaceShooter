using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public PowerUpType type;

    private GameController gameController;

    // Use this for initialization
    private void Start()
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

        Behaviour halo = (Behaviour)gameObject.GetComponent("Halo");
        StartCoroutine(BlinkHalo(halo));
    }

    public void Activate()
    {
        Debug.Log("Player picked up power up of type: " + type.ToString());
        switch (type)
        {
            case PowerUpType.LIFE:
                gameController.AdjustLives(1);
                break;

            case PowerUpType.INVULNERABLE:
                gameController.MakePlayerInvincible(10.0f);
                break;

            case PowerUpType.SIDECAR:
                gameController.SetPlayerSidecarEnabled(15.0f);
                break;
        }
    }

    private IEnumerator BlinkHalo(Behaviour halo)
    {
        while (true)
        {
            halo.enabled = false;
            yield return new WaitForSeconds(0.5f);

            halo.enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
}

[System.Serializable]
public enum PowerUpType
{
    LIFE,
    INVULNERABLE,
    SIDECAR
}