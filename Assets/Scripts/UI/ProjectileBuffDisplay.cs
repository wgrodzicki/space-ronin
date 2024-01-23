using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits from the UIelement class and handles the display of the projectile buff remaining time.
/// </summary>

public class ProjectileBuffDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;
    
    // Variables to control the state of the currently active buff
    [HideInInspector]
    public bool buffIsActive = false;
    [HideInInspector]
    public float remainingTime = 0;

    /// <summary>
    /// Description:
    /// Changes the buff time display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void DisplayProjectileBuff()
    {
        // Check if any of the buffs is active
        if (buffIsActive)
        {   
            // Make sure the game isn't over or won yet
            if (GameManager.instance.gameIsOver != true)
            {
                if (GameManager.instance.gameIsWon != true)
                {
                    if (displayText != null)
                    {
                        // Display the remaining buff time if so
                        displayText.text = "Quickshot: " + (int)remainingTime;
                    }
                }    
            }
        }  
        // Otherwise, display nothing
        else
        {
            displayText.text = "";
        }
    }

    /// <summary>
    /// Description:
    /// Overrides the virtual function UpdateUI() of the UIelement class and uses the DisplayProjectileBuff function to update
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayProjectileBuff();
    }
}