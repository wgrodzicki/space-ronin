using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullObject : MonoBehaviour
{
    [Tooltip("Pulling force of the black hole")]
    public int pullingForce = 0;

    [Tooltip("Event horizon of the black hole")]
    public float eventHorizon = 0;
    
    [Tooltip("Damage dealt behind the event horizon of the black hole")]
    public int damage = 0;

    // Black hole's position
    private Vector2 blackHolePosition = Vector2.zero;

    // Distance between the black hole and the object in the pulling range
    private float distance = 0;

    // Pulling rate of the black hole
    private float pullingRate = 0;

    // Call upon loading the scene
    void Awake() {
        // Determine the position (transform) of this black hole in the game world
        blackHolePosition = this.gameObject.transform.position;
    }

    // Listen for colliders staying in range
    void OnTriggerStay2D(Collider2D collider) {

        // Check if the collider is of appropriate type/tag
        if (collider.tag == "Player" || collider.tag == "Enemy" || collider.tag == "Kamikaze" || collider.tag == "Freighter" || collider.tag == "Boss") {
            // Calculate the pulling rate
            pullingRate = pullingForce * Time.fixedDeltaTime;

            // Pull the collider towards the center of the black hole
            collider.transform.position = Vector2.MoveTowards(collider.transform.position, blackHolePosition, pullingRate);

            // Check the distance between the collider and the center of the black hole
            distance = Vector2.Distance(blackHolePosition, collider.transform.position);

            // Check if the collider has passed the event horizon
            if (distance <= eventHorizon) {
                // Make sure the collider has a health component
                if (collider.GetComponent<Health>() != null) {
                    // Deal damage if so
                    collider.GetComponent<Health>().TakeDamage(damage);
                }
            }
        }
    }
}