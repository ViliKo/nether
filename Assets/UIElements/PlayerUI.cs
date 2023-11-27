using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerUI : MonoBehaviour
{
    private Image[] cooldownImages;
    private Image cooldownImage;

    public float cooldownDuration = 10f;  // Set the cooldown duration in seconds
    private float cooldownTimer;

    void Start()
    {

       cooldownImages = GetComponentsInChildren<Image>();

        foreach (Image cooldownImageSearched in cooldownImages)
        {
            if (cooldownImageSearched.gameObject.name == "dash-cooldown")
            {
                // Do something with the component
                Debug.Log("Found Image component with name 'YourTargetName': " + cooldownImageSearched);
                cooldownImage = cooldownImageSearched;
                
                break;  // Stop searching after finding the first match
            }
        }

        cooldownImage.fillAmount = 0;

        cooldownTimer = cooldownDuration;
        UpdateCooldownUI();
    }

    void Update()
    {
        // Update the cooldown timer
        cooldownTimer -= Time.deltaTime;

        // Update the UI
        UpdateCooldownUI();
    }

    void UpdateCooldownUI()
    {
        // Calculate the fill amount based on the remaining cooldown time
        float fillAmount = Mathf.Clamp01(1 - (cooldownTimer / cooldownDuration));

        // Set the fill amount of the cooldown image
        cooldownImage.fillAmount = fillAmount;

        // Check if the cooldown is complete
        if (cooldownTimer <= 0)
        {
            cooldownTimer = cooldownDuration;  // Reset the cooldown timer
        }
    }
}