using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the projectile buff effect to increase shooting frequency of the player.
/// </summary>

public class ProjectileBuffEffect : MonoBehaviour
{
    [Header("Effect")]
    [Tooltip("The effect to create when the buff is used")]
    public GameObject projectileBuffEffect = null;

    private GameObject player = null;
    private ShootingController playerShooting = null;
    
    [HideInInspector]
    // Variable to control whether the buff is currently active
    public bool projectileBuffActive = false;
    private float playerFireRate = 0;
    
    [Tooltip("How long should the buff last")]
    // Variables to control how long the buff should last
    public float projectileBuffDuration = 30.0f;
    
    [HideInInspector]
    public float timeToEndProjectileBuff = 0;
    // Variable to access the UI display of the remaining buff time
    private GameObject buffDisplay = null;

    /// <summary>
    /// Description:
    /// Standard Unity function called once per frame
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void Update()
    {
        // If buff is active, keep track of the timer
        if (projectileBuffActive) {

            // Update the remaining buff time in the UI
            buffDisplay.GetComponent<ProjectileBuffDisplay>().remainingTime = timeToEndProjectileBuff - Time.time;
            GameManager.UpdateUIElements();

            if (Time.time >= timeToEndProjectileBuff) {

                // Deactivate buff if enough time has passed
                projectileBuffActive = false;
                playerShooting.hasBuff = false;

                // Reset player's fire rate
                playerShooting.fireRate = playerFireRate;

                // Deactivate the UI display of the remaining buff time
                buffDisplay.GetComponent<ProjectileBuffDisplay>().buffIsActive = false;
                GameManager.UpdateUIElements();

                // Destroy the buff
                Destroy(this.gameObject);
            }
        }
    }

    /// <summary>
    /// Description:
    /// Handles the projectile buff effect
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void handleProjectileBuff() {

        player = GameObject.FindWithTag("Player");
        playerShooting = player.GetComponent<ShootingController>();
        
        // Check if the player has the buff already active
        if (playerShooting.hasBuff != true) {

            // Get and increase player's fire rate
            playerFireRate = playerShooting.fireRate;
            playerShooting.fireRate = 0.05f;

            // Set the buff to active and start the timer
            projectileBuffActive = true;
            playerShooting.hasBuff = true;
            timeToEndProjectileBuff = Time.time + projectileBuffDuration;

            // Get the UI display of the remaining buff time and acticate it
            buffDisplay = GameObject.Find("ProjectileBuff Text");
            buffDisplay.GetComponent<ProjectileBuffDisplay>().buffIsActive = true;

            // Play effect
            Instantiate(projectileBuffEffect, transform.position, transform.rotation, null);

            // Make the buff disappear
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    // Listen for collisions
    void OnTriggerEnter2D(Collider2D collider) {

        // If the collider is the player
        if (collider.gameObject.name == "Player") {
            handleProjectileBuff();
        }
    }

    // Listen for objects staying in
    void OnTriggerStay2D(Collider2D collider) {

        // If the collider is the player
        if (collider.gameObject.name == "Player") {
            handleProjectileBuff();
        }
    }
}