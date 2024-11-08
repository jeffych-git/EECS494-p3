using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultScreen : MonoBehaviour
{
    public GameObject ResultScreen;
    public GameObject team1_win_text;
    public GameObject team2_win_text;
    public GameObject twoTeamController;

    void Start()
    {
        EventBus.Subscribe<EventTeamDead>(OnEventTeamDead);
    }

    void OnEventTeamDead(EventTeamDead e)
    {
        ResultScreen.SetActive(true);
        if (e.character.CompareTag("Team 1"))
        {
            print(e.character.tag);
            team2_win_text.SetActive(true);
        }
        else if(e.character.CompareTag("Team 2"))
        {
            print(e.character.tag);
            team1_win_text.SetActive(true);
        }
/*        Time.timeScale = 0;
        twoTeamController.GetComponent<TwoTeamController>().enabled = false;*/
    }
}
