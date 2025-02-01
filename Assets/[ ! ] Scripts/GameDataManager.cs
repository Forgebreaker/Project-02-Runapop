using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int gems = 0;
    public int selectedCharId = 0;
    public List<int> purchasedChars = new List<int>();
    public int HighestCoinRecord = 0;
    public int NewestCoinNumberGathered = 0;

    // For Monthly Subscription
    public bool isMonthlySubscriptionActive = false;
    public System.DateTime subscriptionEndDate = System.DateTime.MinValue;
}

public static class GameDataManager
{
    static PlayerData playerData;
    static GameDataManager()
    {
        Initialize();
    }

    public static void Initialize()
    {
        if (playerData == null)
        {
            Debug.Log("<color=blue>[GameDataManager] Initializing playerData.</color>");
            LoadPlayerData();

            if (playerData.purchasedChars == null)
            {
                playerData.purchasedChars = new List<int>();
                SavePlayerData();
                Debug.Log("<color=orange>[GameDataManager] Fixed purchasedChars list and saved.</color>");
            }
        }
    }

    // Record Management ---------------------------------------------------------------------------

    public static void NewestCoinNumbGathered(int amount)
    {
        EnsureInitialized();
        playerData.NewestCoinNumberGathered = amount;
        SavePlayerData();
        
    }

    public static int MatchCoinNumbGathered() 
    {
        EnsureInitialized();
        return playerData.NewestCoinNumberGathered;
    }
    public static int HighestCoinNumbRecord()
    {
        EnsureInitialized();
        playerData.HighestCoinRecord = Mathf.Max(playerData.HighestCoinRecord, playerData.NewestCoinNumberGathered);
        SavePlayerData();
        return playerData.HighestCoinRecord;
    }

    // Coin Management ---------------------------------------------------------------------------
    public static int GetCoins()
    {
        EnsureInitialized();
        return playerData.coins;
    }

    public static void AddCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins += amount;
        SavePlayerData();
    }

    public static bool CanSpendCoins(int amount)
    {
        EnsureInitialized();
        return playerData.coins >= amount;
    }

    public static void SpendCoins(int amount)
    {
        EnsureInitialized();
        playerData.coins -= amount;
        SavePlayerData();
    }

    // Gems Management ---------------------------------------------------------------------------
    public static int GetGems()
    {
        EnsureInitialized();
        return playerData.gems;
    }

    public static void AddGems(int amount)
    {
        EnsureInitialized();
        playerData.gems += amount;
        SavePlayerData();
    }

    public static bool CanSpendGems(int amount)
    {
        EnsureInitialized();
        return playerData.gems >= amount;
    }

    public static void SpendGems(int amount)
    {
        EnsureInitialized();
        playerData.gems -= amount;
        SavePlayerData();
    }

    // Monthly Subscription Management -----------------------------------------------------------
    public static void ActivateMonthlySubscription(int days)
    {
        EnsureInitialized();
        playerData.isMonthlySubscriptionActive = true;
        playerData.subscriptionEndDate = System.DateTime.UtcNow.AddDays(days);
        SavePlayerData();
        Debug.Log($"<color=green>[GameDataManager] Monthly subscription activated for {days} days.</color>");
    }

    public static bool IsMonthlySubscriptionActive()
    {
        EnsureInitialized();

        if (playerData.isMonthlySubscriptionActive && System.DateTime.UtcNow <= playerData.subscriptionEndDate)
        {
            return true;
        }

        // Deactivate subscription if expired
        if (playerData.isMonthlySubscriptionActive && System.DateTime.UtcNow > playerData.subscriptionEndDate)
        {
            playerData.isMonthlySubscriptionActive = false;
            SavePlayerData();
            Debug.Log("<color=red>[GameDataManager] Monthly subscription expired.</color>");
        }

        return false;
    }

    public static string GetRemainingSubscriptionTime()
    {
        EnsureInitialized();

        if (playerData.isMonthlySubscriptionActive)
        {
            System.DateTime now = System.DateTime.UtcNow;
            if (now <= playerData.subscriptionEndDate)
            {
                System.TimeSpan remainingTime = playerData.subscriptionEndDate - now;
                return $"{remainingTime.Days} days {remainingTime.Hours} hours {remainingTime.Minutes} minutes left";
            }
            else
            {
                // Subscription expired
                playerData.isMonthlySubscriptionActive = false;
                SavePlayerData();
            }
        }
        return "Subscription expired";
    }

    // Chars Management ---------------------------------------------------------------------------
    public static bool IsCharPurchased(int charId)
    {
        EnsureInitialized();

        if (playerData.purchasedChars == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] purchasedChars list is null. Initializing it now.</color>");
            playerData.purchasedChars = new List<int>();
        }

        return playerData.purchasedChars.Contains(charId);
    }

    public static void PurchaseChar(int charId, int cost)
    {
        EnsureInitialized();

        if (CanSpendCoins(cost) && !IsCharPurchased(charId))
        {
            SpendCoins(cost);
            playerData.purchasedChars.Add(charId);
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Char {charId} purchased for {cost} coins!</color>");
        }
    }

    public static void PurchaseVIPChar(int charId, int cost)
    {
        EnsureInitialized();

        if (CanSpendGems(cost) && !IsCharPurchased(charId))
        {
            SpendGems(cost);
            playerData.purchasedChars.Add(charId);
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Char {charId} purchased for {cost} coins!</color>");
        }
    }

    public static void SetSelectedChar(int charId)
    {
        EnsureInitialized();

        if (IsCharPurchased(charId))
        {
            playerData.selectedCharId = charId;
            SavePlayerData();
            Debug.Log($"<color=green>[GameDataManager] Char {charId} selected!</color>");
        }
        else
        {
            Debug.LogWarning($"<color=red>[GameDataManager] Char {charId} is not purchased and cannot be selected.</color>");
        }
    }
    
    public static int GetSelectedChar()
    {
        EnsureInitialized();
        return playerData.selectedCharId;
    }

    public static int GetTotalPurchasedShips()
    {
        EnsureInitialized();
        return playerData.purchasedChars.Count;
    }


    // Save and Load Player Data -----------------------------------------------------------------
    static void SavePlayerData()
    {
        BinarySerializer.Save(playerData, "Runapop-data.txt");
        Debug.Log("<color=magenta>[PlayerData] Saved.</color>");
    }

    static void LoadPlayerData()
    {
        try
        {  
            if (BinarySerializer.HasSaved("Runapop-data.txt"))
            {
                playerData = BinarySerializer.Load<PlayerData>("Runapop-data.txt");
                Debug.Log("<color=green>[PlayerData] Loaded.</color>");
            }
            else
            {
                playerData = new PlayerData();
                playerData.coins = 0;
                playerData.gems = 0;
                playerData.purchasedChars.Add(0);
                playerData.selectedCharId = 0;
                SavePlayerData();
                Debug.Log("<color=yellow>[GameDataManager] Initialized new data.</color>");
            }
        }
        catch (System.Exception ex)
        {
            playerData = new PlayerData();
            Debug.LogError($"<color=red>[GameDataManager] Failed to load data: {ex.Message}</color>");
        }
    }

    // Ensure Initialized ------------------------------------------------------------------------
    static void EnsureInitialized()
    {
        if (playerData == null)
        {
            Debug.LogWarning("<color=orange>[GameDataManager] playerData was null. Reinitializing.</color>");
            Initialize();
        }
    }
}
