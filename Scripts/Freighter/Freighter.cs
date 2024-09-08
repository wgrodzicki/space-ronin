using UnityEngine;

public class Freighter : MonoBehaviour
{
    [Tooltip("The target to be followed/reached")]
    [SerializeField] private GameObject targetObject = null;
    [Tooltip("How close the object has to come to the target")]
    [SerializeField] private float requiredProximity = 0;

    private float distance = 0;
    private bool timeToWin = false;

    private void Update()
    {
        distance = Vector3.Distance(this.gameObject.transform.position, targetObject.transform.position);

        if (timeToWin == false)
        {
            if (distance <= requiredProximity)
            {
                // Stop moving
                this.gameObject.GetComponent<Enemy>().enabled = false;

                // Win the game if distance is short enough
                timeToWin = true;
                GameManager.instance.GameWon();
            }
        }
    }
}
