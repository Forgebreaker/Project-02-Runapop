using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Playables;

public class ButtonUIManager : MonoBehaviour
{
    [SerializeField] public GameObject PauseMenuPanel;
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button BackToHomeButton;
    [SerializeField] private Button PauseButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ReplayButton;
    [SerializeField] private Button QuitButton;
    [SerializeField] private Button AddGemsButton;
    [SerializeField] private Button AddCoinsButton;
    [SerializeField] private GameObject[] CharList;
    [SerializeField] private TMP_Text RecordData;

    [SerializeField] private AudioClip ButtonClickSound;
    private AudioSource _audioSource;

    [Header("AddGems Screen")]
    private string Add25Gems_ID = "com.OutbreakCompany.Runapop.25gemspack";
    private string Add75Gems_ID = "com.OutbreakCompany.Runapop.75gemspack";
    private string Add150Gems_ID = "com.OutbreakCompany.Runapop.150gemspack";
    private string Add250Gems_ID = "com.OutbreakCompany.Runapop.250gemspack";
    private string Add700Gems_ID = "com.OutbreakCompany.Runapop.700gemspack";

    [Header("Subscriptions")]
    private string SubX2CoinsDaily_ID = "com.OutbreakCompany.Runapop.subx2coinsdaily";
    private string SubX2CoinsWeekly_ID = "com.OutbreakCompany.Runapop.subx2coinsweekly";
    private string SubX2CoinsMonthly_ID = "com.OutbreakCompany.Runapop.subx2coinsmonthly";

    [SerializeField] private TMP_Text Subscribe_Notification;
    [SerializeField] private GameObject DailySubButton;
    [SerializeField] private GameObject WeeklySubButton;
    [SerializeField] private GameObject MonthlySubButton;

    private void Start()
    {
        _audioSource = this.gameObject.GetComponent<AudioSource>();

        if (ResumeButton != null)
            ResumeButton.onClick.AddListener(Resume);
        if (BackToHomeButton != null)
            BackToHomeButton.onClick.AddListener(BackToHome);
        if (PauseButton != null)
            PauseButton.onClick.AddListener(Pause);
        if (StartButton != null)
            StartButton.onClick.AddListener(StartGame);
        if (ReplayButton != null)
            ReplayButton.onClick.AddListener(ReplayGame);
        if (QuitButton != null)
            QuitButton.onClick.AddListener(QuitGame);
        if (AddGemsButton != null)
            AddGemsButton.onClick.AddListener(AddGems);
        if (AddCoinsButton != null)
            AddCoinsButton.onClick.AddListener(AddCoins);

        if (RecordData != null && GameDataManager.IsMonthlySubscriptionActive() != true)
        {
            RecordData.text = $"[*] Point Gather Record: {GameDataManager.HighestCoinNumbRecord()}\r\n\r\n[*] Point Gathered this match: {GameDataManager.MatchCoinNumbGathered()}";
        }

        if (RecordData != null && GameDataManager.IsMonthlySubscriptionActive() == true)
        {
            RecordData.text = $"[*] Point Gather Record: {GameDataManager.HighestCoinNumbRecord()}\r\n\r\n[*] Point Gathered this match: {GameDataManager.MatchCoinNumbGathered()}\r\n\r\n( + {GameDataManager.MatchCoinNumbGathered()} Point - x2 Pack Bonus )";
            GameDataManager.AddCoins(GameDataManager.MatchCoinNumbGathered());
        }

        if (CharList != null)
        {
            for (int i = 0; i < CharList.Length; i++)
            {
                if (i == GameDataManager.GetSelectedChar())
                {
                    CharList[i].SetActive(true);
                }
                else
                {
                    CharList[i].SetActive(false);
                }
            }

        }
    }

    private void Update()
    {
        if (GameDataManager.IsMonthlySubscriptionActive() == true && DailySubButton != null
            && WeeklySubButton != null && MonthlySubButton != null && Subscribe_Notification != null)
        {
            DailySubButton.gameObject.SetActive(false);
            WeeklySubButton.gameObject.SetActive(false);
            MonthlySubButton.gameObject.SetActive(false);
            Subscribe_Notification.text = $"X2 Reward Pack Subscribed \r\n{GameDataManager.GetRemainingSubscriptionTime()}";
        }

        if (GameDataManager.IsMonthlySubscriptionActive() != true && DailySubButton != null
            && WeeklySubButton != null && MonthlySubButton != null && Subscribe_Notification != null)
        {
            DailySubButton.gameObject.SetActive(true);
            WeeklySubButton.gameObject.SetActive(true);
            MonthlySubButton.gameObject.SetActive(true);
            Subscribe_Notification.text = "";
        }
    }

    public void Pause()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void BackToHome()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Lobby");
    }

    public void StartGame()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        SceneManager.LoadScene("Gameplay");
    }

    public void ReplayGame()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        SceneManager.LoadScene("Gameplay");
    }

    public void AddGems()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        SceneManager.LoadScene("AddGems");
    }

    public void AddCoins()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        SceneManager.LoadScene("AddCoins");
    }

    public void QuitGame()
    {
        _audioSource.PlayOneShot(ButtonClickSound);
        Debug.Log("Quit Game Button Pressed");
        Application.Quit();
    }

    public void OnPurchaseComplete(Product product)
    {
        _audioSource.PlayOneShot(ButtonClickSound);

        if (product.definition.id == Add25Gems_ID)
        {
            GameDataManager.AddGems(25);
        }

        if (product.definition.id == Add75Gems_ID)
        {
            GameDataManager.AddGems(75);
        }

        if (product.definition.id == Add150Gems_ID)
        {
            GameDataManager.AddGems(150);
        }

        if (product.definition.id == Add250Gems_ID)
        {
            GameDataManager.AddGems(250);
        }

        if (product.definition.id == Add700Gems_ID)
        {
            GameDataManager.AddGems(700);
        }

        if (product.definition.id == SubX2CoinsDaily_ID)
        {
            GameDataManager.ActivateMonthlySubscription(1);
        }

        if (product.definition.id == SubX2CoinsWeekly_ID)
        {
            GameDataManager.ActivateMonthlySubscription(7);
        }

        if (product.definition.id == SubX2CoinsMonthly_ID)
        {
            GameDataManager.ActivateMonthlySubscription(30);
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason)
    {
        Debug.Log(reason);
    }
}
