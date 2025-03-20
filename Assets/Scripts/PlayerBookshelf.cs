using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBookshelf : MonoBehaviour
{
    public TextMeshProUGUI bookshelfText;
    private Player_Controller playerController;

    private void Start()
    {
        playerController = FindObjectOfType<Player_Controller>();

        if (playerController == null)
        {
            Debug.LogError("Player_Controller not found in the scene!");
            return;
        }

        UpdateBookshelfUI(playerController.food);
    }

    public void UpdateBookshelfUI(int amount)
    {
        bookshelfText.text = amount.ToString() + "/3";
    }
}
