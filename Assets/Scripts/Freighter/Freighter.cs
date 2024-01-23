using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles the winning/losing of the game when the goal is to allow an object reach another object before it is destroyed.
/// </summary>

public class Freighter : MonoBehaviour
{
    [Tooltip("The target to be followed/reached")]
    public GameObject targetObject = null;

    [Tooltip("How close the object has to come to the target")]
    public float requiredProximity = 0;

    // Distance between the object and the target
    private float distance = 0;

    // Variables to control whether the winning/losing state has been reached
    private bool timeToWin = false;

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(this.gameObject.transform.position, targetObject.transform.position);

        // If not won yet
        if (timeToWin == false) {

            // Check the distance between the object and the target
            if (distance <= requiredProximity) {
                
                // Stop moving
                this.gameObject.GetComponent<Enemy>().enabled = false;

                // Win the game if distance is short enough
                timeToWin = true;
                GameManager.instance.GameWon();
            }
        }
    }
}
