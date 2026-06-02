using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int coins = 0;
    
    public void AddCoin(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Coin adding amount is negative.");
        }
        coins += amount;
    }
}