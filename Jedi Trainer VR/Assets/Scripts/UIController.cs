using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider healthSlider;
    public Slider forceSlider;
    private Image healthFillImage;
    private PlayerController playerController;
    void Start()
    {
        playerController = GameObject.Find("XR Origin (XR Rig)").GetComponent<PlayerController>();
        healthFillImage = healthSlider.fillRect.GetComponentInChildren<Image>();
    }

    void Update()
    {
        if (playerController != null)
        {
            healthSlider.value = playerController.playerHealth;
            forceSlider.value = playerController.playerForce;
            healthFillImage.color = CalculateHealthColor(playerController.playerHealth);
        }
    }

    private Color CalculateHealthColor(int health)
    {
        if (health > 50)
        {
            return Color.Lerp(Color.yellow, Color.green, (float)(health - 50) / 50f);
        }
        else
        {
            return Color.Lerp(Color.red, Color.yellow, (float)health / 50f);
        }
    }
}
