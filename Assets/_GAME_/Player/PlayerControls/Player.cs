using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public int playerCurrency;
    public int developmentPoints = 5;

    public void AddCoins(int amount)
    {
        playerCurrency += amount;
    }






    //public int damage;
    //public int maxHP;
    //public int currentHP;

    //public bool TakeDamage(int dmg)
    //{
    //    currentHP -= dmg;

    //    if (currentHP <= 0)
    //        return true;
    //    else
    //        return false;
    //}

    //public void Heal(int amount)
    //{
    //    currentHP += amount;
    //    if (currentHP > maxHP)
    //        currentHP = maxHP;
    //}
}
