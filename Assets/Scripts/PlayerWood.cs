using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWood : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    private Player_Controller playerController;

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();

        if (playerController == null)
        {
            Debug.LogError("Player_Controller not found in the scene!");
            return;
        }

        UpdateWoodUI(playerController.wood);
    }

    public void UpdateWoodUI(int amount)
    {
        woodText.text = amount.ToString();
    }
}
