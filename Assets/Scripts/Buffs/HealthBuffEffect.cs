using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the health buff effect to heal the player.
/// </summary>

public class HealthBuffEffect : MonoBehaviour
{
    [Header("Effect")]
    [Tooltip("The effect to create when the buff is used")]
    public GameObject healthBuffEffect = null;

    private GameObject player = null;
    private Health playerHealth = null;
    private int playerMaxHealth = 0;
    private int playerCurrentHealth = 0;

    /// <summary>
    /// Description:
    /// Handles the health buff effect
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void handleHealthBuff() {

        // Get player's max and current health
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        playerMaxHealth = playerHealth.maximumHealth;
        playerCurrentHealth = playerHealth.currentHealth;

        // If player is not at max health
        if (playerCurrentHealth < playerMaxHealth) {

            // Heal the player
            playerHealth.currentHealth++;

            // Update UI to display current health
            GameManager.UpdateUIElements();

            // Play effect
            Instantiate(healthBuffEffect, transform.position, transform.rotation, null);
            
            // Destroy the buff
            Destroy(this.gameObject);
        }
    }

    // Listen for collisions
    void OnTriggerEnter2D(Collider2D collider) {

        // If the collider is the player
        if (collider.gameObject.name == "Player") {
            handleHealthBuff();
        }
    }

    // Listen for objects staying in
    void OnTriggerStay2D(Collider2D collider) {

        // If the collider is the player
        if (collider.gameObject.name == "Player") {
            handleHealthBuff();
        }
    }
}
