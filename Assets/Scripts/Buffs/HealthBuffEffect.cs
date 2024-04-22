using UnityEngine;

public class HealthBuffEffect : MonoBehaviour
{
    [Tooltip("The effect to create when the buff is used")]
    [SerializeField] private GameObject healthBuffEffect = null;

    private GameObject player = null;
    private Health playerHealth = null;
    private int playerMaxHealth = 0;
    private int playerCurrentHealth = 0;

    /// <summary>
    /// Handles the health buff effect.
    /// </summary>
    private void HandleHealthBuff()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        playerMaxHealth = playerHealth.maximumHealth;
        playerCurrentHealth = playerHealth.currentHealth;

        // Player is not at max health
        if (playerCurrentHealth < playerMaxHealth)
        {
            playerHealth.currentHealth++;
            GameManager.UpdateUIElements();
            Instantiate(healthBuffEffect, transform.position, transform.rotation, null);

            // Destroy the buff
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            HandleHealthBuff();
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            HandleHealthBuff();
        }
    }
}
