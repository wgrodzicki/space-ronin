using UnityEngine;

public class ProjectileBuffEffect : MonoBehaviour
{
    [Tooltip("The effect to create when the buff is used")]
    [SerializeField] private GameObject projectileBuffEffect = null;
    [Tooltip("How long should the buff last")]
    [SerializeField] private float projectileBuffDuration = 30.0f;

    [HideInInspector] public bool projectileBuffActive = false;
    [HideInInspector] public float timeToEndProjectileBuff = 0;

    private GameObject player = null;
    private ShootingController playerShooting = null;
    private float playerFireRate = 0;
    private GameObject buffDisplay = null;

    private void Update()
    {
        if (projectileBuffActive)
        {
            // Update the remaining buff time in the UI
            buffDisplay.GetComponent<ProjectileBuffDisplay>().remainingTime = timeToEndProjectileBuff - Time.time;
            GameManager.UpdateUIElements();

            if (Time.time >= timeToEndProjectileBuff)
            {
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
    /// Handles the projectile buff effect.
    /// </summary>
    void HandleProjectileBuff()
    {
        player = GameObject.FindWithTag("Player");
        playerShooting = player.GetComponent<ShootingController>();

        if (playerShooting.hasBuff != true)
        {
            playerFireRate = playerShooting.fireRate;
            playerShooting.fireRate = 0.05f;

            // Set the buff to active and start the timer
            projectileBuffActive = true;
            playerShooting.hasBuff = true;
            timeToEndProjectileBuff = Time.time + projectileBuffDuration;

            // Get the UI display of the remaining buff time and activate it
            buffDisplay = GameObject.Find("ProjectileBuff Text");
            buffDisplay.GetComponent<ProjectileBuffDisplay>().buffIsActive = true;
            Instantiate(projectileBuffEffect, transform.position, transform.rotation, null);

            // Make the buff disappear
            this.gameObject.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            HandleProjectileBuff();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Player")
        {
            HandleProjectileBuff();
        }
    }
}