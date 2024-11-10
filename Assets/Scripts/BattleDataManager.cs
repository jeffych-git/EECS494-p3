using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleDataManager : MonoBehaviour
{
    public static BattleDataManager Instance { get; private set; }
    public bool is_victory = true;
    //exp value needs to be added as well.

    // Start is called before the first frame update
    void Awake()
    {
        EventBus.Subscribe<EventTeamDead>(OnEventTeamDead);
        // Set up singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Makes sure the BattleDataManager persists across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEventTeamDead(EventTeamDead e)
    {
        if (e.character.CompareTag("Team 2"))
        {
            is_victory = true;
        }
        else
        {
            is_victory = false;
        }
    }

}
