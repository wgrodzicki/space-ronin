using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// This class handles the health state of a game object.
/// 
/// Implementation Notes: 2D Rigidbodies must be set to never sleep for this to interact with trigger stay damage
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Team Settings")]
    [Tooltip("The team associated with this damage")]
    public int teamId = 0;

    [Header("Health Settings")]
    [Tooltip("The default health value")]
    public int defaultHealth = 1;
    [Tooltip("The maximum health value")]
    public int maximumHealth = 1;
    [Tooltip("The current in game health value")]
    public int currentHealth = 1;
    [Tooltip("Invulnerability duration, in seconds, after taking damage")]
    public float invincibilityTime = 3f;
    [Tooltip("Whether or not this health is always invincible")]
    public bool isAlwaysInvincible = false;

    [Header("Lives settings")]
    [Tooltip("Whether or not to use lives")]
    public bool useLives = false;
    [Tooltip("Current number of lives this health has")]
    public int currentLives = 3;
    [Tooltip("The maximum number of lives this health can have")]
    public int maximumLives = 5;

    // Variables to access and control player movement
    private Controller playerController = null;
    private float playerSpeed = 0;
    private ShootingController playerShootingController = null;

    /// <summary>
    /// Description:
    /// Standard unity funciton called before the first frame update
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void Start()
    {
        SetRespawnPoint(transform.position);
        // Save player's movement speed and get its shooting controller for respawning purposes
        if (gameObject.tag == "Player")
        {
            playerController = gameObject.GetComponent<Controller>();
            playerSpeed = playerController.moveSpeed;
            playerShootingController = gameObject.GetComponent<ShootingController>();
        }
    }

    // Variables to control the respawn moment
    private float respawnDelay = 3.5f;
    private float timeToRespawn = 0;
    private bool shouldRespawn = false;
    private bool respawnTimer = false;

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
        InvincibilityCheck();
        HandleRespawn();
    }

    /// <summary>
    /// Description:
    /// Allows to wait a specific amount of time before respawn (lets the death animation finish)
    /// Immobilizes the player before respawn.
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>    
    private void HandleRespawn()
    {
        // Check if the health should respawn
        if (shouldRespawn == true)
        {
            // If health is player, stop it from moving and shooting
            if (gameObject.tag == "Player")
            {
                playerController.moveSpeed = 0;
                playerShootingController.enabled = false;
            }
            // Start the timer if so
            if (respawnTimer == false)
            {
                timeToRespawn = Time.time + respawnDelay;
                respawnTimer = true;
            }
            // Respawn the health if enough time has passed
            if (Time.time >= timeToRespawn)
            {
                // If health is the player, restore its original movement speed and enable shooting
                if (gameObject.tag == "Player")
                {
                    playerController.moveSpeed = playerSpeed;
                    playerShootingController.enabled = true;
                }
                Respawn();
            }
        }
    }

    // The specific game time when the health can be damged again
    private float timeToBecomeDamagableAgain = 0;
    // Whether or not the health is invincible
    private bool isInvincableFromDamage = false;

    /// <summary>
    /// Description:
    /// Checks against the current time and the time when the health can be damaged again.
    /// Removes invicibility if the time frame has passed
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    private void InvincibilityCheck()
    {
        if (timeToBecomeDamagableAgain <= Time.time)
        {
            isInvincableFromDamage = false;
        }
    }

    // The position that the health's gameobject will respawn at if lives are being used
    private Vector3 respawnPosition;
    /// <summary>
    /// Description:
    /// Changes the respawn position to a new position
    /// Inputs:
    /// Vector3 newRespawnPosition
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="newRespawnPosition">The new position to respawn at</param>
    public void SetRespawnPoint(Vector3 newRespawnPosition)
    {
        respawnPosition = newRespawnPosition;
    }

    /// <summary>
    /// Description:
    /// Repositions the health's game object to the respawn position and resets the health to the default value
    /// Inputs:
    /// None
    /// Returns:
    /// void (no return)
    /// </summary>
    void Respawn()
    {
        // Respawn the health in the position
        transform.position = respawnPosition;
        // Reset health
        currentHealth = defaultHealth;
        // Make it appear
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        // Reset respawn timer
        timeToRespawn = 0;
        shouldRespawn = false;
        respawnTimer = false;

        // If health is player, update UI to display current health
        if (gameObject.tag == "Player")
        {
            GameManager.UpdateUIElements();
        }
    }

    /// <summary>
    /// Description:
    /// Applies damage to the health unless the health is invincible.
    /// Inputs:
    /// int damageAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="damageAmount">The amount of damage to take</param>
    public void TakeDamage(int damageAmount)
    {
        if (isInvincableFromDamage || isAlwaysInvincible)
        {
            return;
        }
        else
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, transform.rotation, null);
            }
            timeToBecomeDamagableAgain = Time.time + invincibilityTime;
            isInvincableFromDamage = true;
            currentHealth -= damageAmount;

            // If health is player, freighter or boss, update UI to display current health
            if (gameObject.tag == "Player" || gameObject.tag == "Freighter" || gameObject.tag == "Boss")
            {
                GameManager.UpdateUIElements();
            }

            CheckDeath();
        }
    }

    /// <summary>
    /// Description:
    /// Applies healing to the health, capped out at the maximum health.
    /// Inputs:
    /// int healingAmount
    /// Returns:
    /// void (no return)
    /// </summary>
    /// <param name="healingAmount">How much healing to apply</param>
    public void ReceiveHealing(int healingAmount)
    {
        currentHealth += healingAmount;
        if (currentHealth > maximumHealth)
        {
            currentHealth = maximumHealth;
        }
        CheckDeath();
    }

    [Header("Effects & Polish")]
    [Tooltip("The effect to create when this health dies")]
    public GameObject deathEffect;
    [Tooltip("The effect to create when this health is damaged")]
    public GameObject hitEffect;

    /// <summary>
    /// Description:
    /// Checks if the health is dead or not. If it is, true is returned, false otherwise.
    /// Calls Die() if the health is dead.
    /// Inputs:
    /// none
    /// Returns:
    /// bool
    /// </summary>
    /// <returns>Bool: true or false value representing if the health has died or not (true for dead)</returns>
    bool CheckDeath()
    {
        if (currentHealth <= 0)
        {
            Die();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Description:
    /// Handles the death of the health. If a death effect is set, it is created. If lives are being used, the health is respawned.
    /// If lives are not being used or the lives are 0 then the health's game object is destroyed.
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, transform.rotation, null);
        }

        if (useLives)
        {
            HandleDeathWithLives();
        }
        else
        {
            HandleDeathWithoutLives();
        }
    }

    /// <summary>
    /// Description:
    /// Handles the death of the health when lives are being used
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void HandleDeathWithLives()
    {
        currentLives -= 1;
        if (currentLives > 0)
        {
            // Make the health disappear
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
            // Update the respawn controlling variable
            shouldRespawn = true;
        }
        else
        {
            if (gameObject.tag == "Player" && GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
            if (gameObject.GetComponent<Enemy>() != null)
            {
                gameObject.GetComponent<Enemy>().DoBeforeDestroy();
            }
            // Game over if health is the object to be protected
            if (gameObject.tag == "Freighter")
            {
                GameManager.instance.GameOver();
                // Disable player's controls if game over
                GameManager.instance.player.SetActive(false);
            }
            // Game won if health is a boss
            if (gameObject.tag == "Boss")
            {
                GameManager.instance.GameWon();
                // Disable player's controls if game over
                GameManager.instance.player.SetActive(false);
            }
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Description:
    /// Handles death when lives are not being used
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    void HandleDeathWithoutLives()
    {
        if (gameObject.tag == "Player" && GameManager.instance != null)
        {
            GameManager.instance.GameOver();
        }
        if (gameObject.GetComponent<Enemy>() != null)
        {
            gameObject.GetComponent<Enemy>().DoBeforeDestroy();
        }
        // Game over if health is the object to be protected
        if (gameObject.tag == "Freighter")
        {
            GameManager.instance.GameOver();
            // Disable player's controls if game over
            GameManager.instance.player.SetActive(false);
        }
        // Game won if health is a boss
        if (gameObject.tag == "Boss")
        {
            GameManager.instance.GameWon();
            // Disable player's controls if game over
            GameManager.instance.player.SetActive(false);
        }
        Destroy(this.gameObject);
    }
}
