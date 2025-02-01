using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject Player_GO;
    [SerializeField] private GameObject[] CharactersList;
    [SerializeField] private int SelectedCharID;

    void Start()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");
        if (Player_GO == null)
        {
            Instantiate(CharactersList[GameDataManager.GetSelectedChar()],
                        this.gameObject.transform.position,
                        CharactersList[GameDataManager.GetSelectedChar()].transform.rotation);
        }
    }
}
