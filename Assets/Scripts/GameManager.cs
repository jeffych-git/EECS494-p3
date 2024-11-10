using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance;

    public float manaRegenSpeed = 1;
    public string leftTeamName;
    public string rightTeamName;
    public bool is_pvp;

    // Need to be set to all the prefabs in the card_variants folder
    [SerializeField] GameObject[] prefabs;
    // Set to the player deck for cards to be instantiated
    [SerializeField] GameObject player_1_deck;

    /*private void Awake()
    {
        // Setting up the singleton pattern so only one of this gameobject may exist in a scene
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }*/

    void Start()
    {
        EventBus.Subscribe<EventTeamDead>(OnEventTeamDead);

        if (!is_pvp)
        {
            SetPlayerDeck();
        }
    }

    void SetPlayerDeck()
    {
        for(int i = 0; i < DataManager.Instance.player_deck.Length; i++)
        {
            for(int j = 0; j < DataManager.Instance.player_deck[i]; j++)
            {
                Instantiate(prefabs[i], player_1_deck.transform);
            }
        }
    }

    void OnEventTeamDead(EventTeamDead e)
    {
        if (is_pvp)
        {
            StartCoroutine(LoadMenuOnDelay());
        }
    }

    //for now will wait for x seconds before loading main menu
    private IEnumerator LoadMenuOnDelay()
    {
        yield return new WaitForSeconds(3);
        // When one team is dead, end the versus match
        SceneManager.LoadScene("MainMenu");
        yield return null;
    }
}
