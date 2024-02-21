using UnityEngine;
using UnityEngine.UI;

public class ProjectileBuffDisplay : UIelement
{
    [Tooltip("The text UI to use for display")]
    [SerializeField] private Text displayText = null;
    
    [HideInInspector] public bool buffIsActive = false;
    [HideInInspector] public float remainingTime = 0;

    /// <summary>
    /// Changes the buff time display.
    /// </summary>
    public void DisplayProjectileBuff()
    {
        if (buffIsActive)
        {   
            if (GameManager.instance.gameIsOver != true)
            {
                if (GameManager.instance.gameIsWon != true)
                {
                    if (displayText != null)
                    {
                        displayText.text = "Quickshot: " + (int)remainingTime;
                    }
                }    
            }
        }
        else
        {
            displayText.text = "";
        }
    }

    /// <summary>
    /// Overrides the virtual function UpdateUI() of the UIelement class and uses the DisplayProjectileBuff function to update.
    /// </summary>
    public override void UpdateUI()
    {
        base.UpdateUI();
        DisplayProjectileBuff();
    }
}