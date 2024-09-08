using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class inherits from the UIelement class and handles the display of the object's current health.
/// </summary>

public class ObjectHealthDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    [Tooltip("The object whose health is to be displayed")]
    public GameObject targetObject = null;

    /// <summary>
    /// Description:
    /// Changes the health display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public void DisplayObjectHealth()
    {
        if (displayText != null)
        {
            if (targetObject.GetComponent<Health>().currentHealth <= 0)
            {
                displayText.text = targetObject.name + " health: 0";
            }
            else
            {
                displayText.text = targetObject.name + " health: " + targetObject.GetComponent<Health>().currentHealth.ToString();
            }

        }
    }

    /// <summary>
    /// Description:
    /// Overrides the virtual function UpdateUI() of the UIelement class and uses the DisplayObjectHealth function to update
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
        DisplayObjectHealth();
    }
}