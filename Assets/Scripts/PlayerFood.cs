using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFood : MonoBehaviour
{
    public TextMeshProUGUI foodText;
    private Player_Controller playerController;

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();

        if (playerController == null)
        {
            Debug.LogError("Player_Controller not found in the scene!");
            return;
        }

        UpdateFoodUI(playerController.food);
    }

    public void UpdateFoodUI(int amount)
    {
        foodText.text = amount.ToString();
    }
}
