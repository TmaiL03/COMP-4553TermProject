using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrency : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    public void SetCurrency(Player player)
    {
        coinText.text = "Coins: " + player.playerCurrency.ToString();
    }
}
