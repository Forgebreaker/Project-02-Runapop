using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EachCharInSelector : MonoBehaviour
{
    public int charId;
    
    [SerializeField] private Button stateButton;
    [SerializeField] private TMP_Text stateButtonText;
    
    [SerializeField] private Button buyButton;
    [SerializeField] private int CharPrice;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip ButtonPressSound;
    [SerializeField] private bool IsVIPChar;
    void Start()
    {
        GameDataManager.Initialize();
        UpdateUI();
        SetupListeners();
        _audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    void SetupListeners()
    {
        if (buyButton != null)
            buyButton.onClick.AddListener(BuyChar);
        else
            Debug.LogWarning("Buy Button is not assigned.");

        if (stateButton != null)
            stateButton.onClick.AddListener(SelectChar);
        else
            Debug.LogWarning("State Button is not assigned.");

    }

    void UpdateUI()
    {
        if (GameDataManager.GetCoins() == null)
        {
            Debug.LogWarning("GameDataManager not initialized yet.");
            return;
        }

        bool isPurchased = GameDataManager.IsCharPurchased(charId);
        int selectedCharId = GameDataManager.GetSelectedChar();

        buyButton.gameObject.SetActive(!isPurchased);
        stateButton.gameObject.SetActive(isPurchased);

        if (isPurchased)
        {
            stateButtonText.text = (selectedCharId == charId) ? "In Use" : "Select";
        }
    }

    void BuyChar()
    {
        if (GameDataManager.CanSpendCoins(CharPrice) && IsVIPChar == false)
        {
            GameDataManager.PurchaseChar(charId, CharPrice);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning($"<color=red>Not enough coins to purchase Char ID: {charId}</color>");
        }

        if (GameDataManager.CanSpendGems(CharPrice) && IsVIPChar == true)
        {
            GameDataManager.PurchaseVIPChar(charId, CharPrice);
            UpdateUI();
        }
        else
        {
            Debug.LogWarning($"<color=red>Not enough gems to purchase Char ID: {charId}</color>");
        }

        _audioSource.PlayOneShot(ButtonPressSound);
    }

    void SelectChar()
    {
        GameDataManager.SetSelectedChar(charId);
        UpdateAllCharUIs();
        _audioSource.PlayOneShot(ButtonPressSound);

    }

    void UpdateAllCharUIs()
    {
        EachCharInSelector[] allCharUIs = FindObjectsByType<EachCharInSelector>(FindObjectsSortMode.None);

        foreach (var charUI in allCharUIs)
        {
            charUI.UpdateUI();
        }
    }
}
