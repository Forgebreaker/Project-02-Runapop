using UnityEngine;
using TMPro;
public class GameShareUI : MonoBehaviour
{
    public static GameShareUI Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] TMP_Text[] coinsUIText;
    [SerializeField] TMP_Text[] gemsUIText;

    void Start()
    {
        if (coinsUIText.Length >= 1)
        {
            UpdateCoinsUIText();
        }

        if (gemsUIText.Length >= 1)
        {
            UpdateGemsUIText();
        }
    }

    private void Update()
    {
        if (coinsUIText.Length >= 1)
        {
            UpdateCoinsUIText();
        }

        if (gemsUIText.Length >= 1)
        {
            UpdateGemsUIText();
        }
    }

    public void UpdateCoinsUIText()
    {
        for (int i = 0; i < coinsUIText.Length; i++)
        {
            SetCoinsText(coinsUIText[i], GameDataManager.GetCoins());
        }
    }

    void SetCoinsText(TMP_Text textMesh, int value)
    {
        if (coinsUIText.Length >= 1)
        {
            textMesh.text = value.ToString();
        }
    }

    public void UpdateGemsUIText() 
    {
        for (int i = 0; i < gemsUIText.Length; i++)
        {
            SetGemsText(gemsUIText[i], GameDataManager.GetGems());
        }
    }

    void SetGemsText(TMP_Text textMesh, int value)
    {
        if (gemsUIText.Length >= 1)
        {
            textMesh.text = value.ToString();
        }
    }
}
