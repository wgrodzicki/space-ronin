using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits from the UIelement class and handles the display of the player's current health.
/// </summary>
public class HealthDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    /// <summary>
    /// Description:
    /// Changes the health display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void DisplayHealth()
    {
        if (displayText != null)
        {
            if (GameManager.instance.player.GetComponent<Health>().currentHealth <= 0) {
                displayText.text = "Health: 0";
            }
            else {
                displayText.text = "Health: " + GameManager.instance.player.GetComponent<Health>().currentHealth.ToString();
            }
            
        }
    }

    /// <summary>
    /// Description:
    /// Overrides the virtual function UpdateUI() of the UIelement class and uses the DisplayHealth function to update
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
        DisplayHealth();
    }
}