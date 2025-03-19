using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private Player_Controller playerController;

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();

        if (playerController == null)
        {
            Debug.LogError("Player_Controller not found in the scene!");
            return;
        }

        UpdateCurrencyUI(playerController.currency);
    }

    public void UpdateCurrencyUI(int amount)
    {
        coinText.text = "Coins: " + amount.ToString();
    }
}
