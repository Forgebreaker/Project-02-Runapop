using UnityEngine;
using UnityEngine.UI;

public class CharSelector : MonoBehaviour
{
    [SerializeField] GameObject[] CharSelectors;
    [SerializeField] Button NextButton_Left;
    [SerializeField] Button NextButton_Right;

    private int currentIndex = 0;

    private void Start()
    {
        currentIndex = GameDataManager.GetSelectedChar();

        if (NextButton_Left != null)
        {
            NextButton_Left.onClick.AddListener(ShowPreviousShip);
        }

        if (NextButton_Right != null)
        {
            NextButton_Right.onClick.AddListener(ShowNextShip);
        }

        UpdateShipModelSelectors();
    }

    private void ShowPreviousShip()
    {
        currentIndex = (currentIndex - 1 + CharSelectors.Length) % CharSelectors.Length;
        UpdateShipModelSelectors();
    }

    private void ShowNextShip()
    {
        currentIndex = (currentIndex + 1) % CharSelectors.Length;
        UpdateShipModelSelectors();
    }

    private void UpdateShipModelSelectors()
    {
        for (int i = 0; i < CharSelectors.Length; i++)
        {
            if (i == currentIndex)
                CharSelectors[i].SetActive(true);
            else
                CharSelectors[i].SetActive(false);
        }
    }
}
