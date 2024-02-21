using UnityEngine;

public class PullObject : MonoBehaviour
{
    [Tooltip("Pulling force of the black hole")]
    [SerializeField] private int pullingForce = 0;
    [Tooltip("Event horizon of the black hole")]
    [SerializeField] private float eventHorizon = 0;
    [Tooltip("Damage dealt behind the event horizon of the black hole")]
    [SerializeField] private int damage = 0;

    // Distance between the black hole and the object in the pulling range
    private float distance = 0;
    private float pullingRate = 0;
    private Vector2 blackHolePosition = Vector2.zero;

    private void Awake()
    {
        blackHolePosition = this.gameObject.transform.position;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player" || collider.tag == "Enemy" || collider.tag == "Kamikaze" || collider.tag == "Freighter" || collider.tag == "Boss")
        {
            pullingRate = pullingForce * Time.fixedDeltaTime;

            // Pull the collider towards the center of the black hole
            collider.transform.position = Vector2.MoveTowards(collider.transform.position, blackHolePosition, pullingRate);

            distance = Vector2.Distance(blackHolePosition, collider.transform.position);

            // Check if the collider has passed the event horizon
            if (distance <= eventHorizon)
            {
                if (collider.GetComponent<Health>() != null)
                {
                    collider.GetComponent<Health>().TakeDamage(damage);
                }
            }
        }
    }
}