using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tutorialScript : MonoBehaviour
{
    public GameObject tutorialUI;
    bool tutorialActive = false;
    public GameObject manaBar;
    private Mana p1Mana;
    private bool pauseScene = false;
    public GameObject twoTeamController;
    public GameObject playerHand;

    private bool allowAdvance = true;
    private bool manaOn = true;

    public GameObject enemy;

    public GameObject panel;
    //text box
    public GameObject textBox;

    private int[] cardFinder = { 0, 0, 0, 0 };

    private int index = -1;



    private string[] dialogue = { 
        "Welcome to the tutorial!", 
        "Here I will bring you valubale insight into how to defeat your many enemies", 
        "The first thing you want to look at each battles are the card in our hand",
        "Each card has a unique ability",
        "To play card you need to use mana, your mana bar is located just above your cards",
        "Each card has it's own mana cost, found on the top right of each card",
        "Let's refill a bit more mana to use an attack",
        "Now that we have enough mana we can use an attack card",
        "You can select and use different cards with the controls at the top of the screen, select the card",
        "Try playing the card",
        "Nice job that was a brilliant attack",
        "There are a few other cards I want to show you",
        "Let's cycle through the deck until we find all of our cards",
        "To do this keep using the attack cards and you will draw new cards from your deck",
        "If you play a special card do not worry it will be reshuffled into your deck and can be pulled again",
        "Great Job! It looks like you found all of them, whether that was luck of the draw or a tedious task is part of the game",
        "These are the cards that will make up your base deck",
        "The heal card heals you for a set amout of points",
        "The shield card gives you a bonus shield to your health that decays over time",
        "The block card is a timing card that requires it to be used just before the player is damaged, when used in time it negates all damage",
        "These are the things you must know to succeed now we battle...",
        "RAHHHHHHHHHHHHHHHHHH"
    };
    // Start is called before the first frame update
    void Start()
    {
        p1Mana = manaBar.GetComponent<ManaBar>().GetMana();
        twoTeamController.GetComponent<TwoTeamController>().enabled = false;

        enemy.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutorialActive && p1Mana.GetManaAmount() >= 15)
        {
            tutorial();
        }

        if ((index == 7) && p1Mana.GetManaAmount() >= 20)
        {
            pauseScene = true;
            tutorialUI.SetActive(true);
        }

        if ((index == 10) && p1Mana.GetManaAmount() >= 20 && manaOn)
        {
            p1Mana.toggleManaRegen();
            manaOn = false;
            if (allowAdvance)
            {
                pauseScene = true;
            }
        }


        
        if (pauseScene)
        {
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Space) && allowAdvance)
            {
                textBox.GetComponent<TextMeshProUGUI>().text = dialogue[++index];
                if (index == 7)
                {
                    pauseScene = false;
                    Time.timeScale = 1;
                    tutorialUI.SetActive(false);
                      
                }
                if (index == 8)
                {
                    twoTeamController.GetComponent<TwoTeamController>().enabled = true;
                    twoTeamController.GetComponent<TwoTeamController>().tutorialActive = true;
                }
                if(index == 10)
                {
                    tutorialUI.SetActive(false);
                    twoTeamController.GetComponent<TwoTeamController>().tutorialActive = false;
                    pauseScene = false;
                    Time.timeScale = 1;
                    p1Mana.toggleManaRegen();
                    manaOn = false;
                    allowAdvance = false;

                    cardFinder = findCardLocation('a');
                }

                if(index == 14)
                {
                    pauseScene = false;
                    Time.timeScale = 1;
                    allowAdvance = false;
                    twoTeamController.GetComponent<TwoTeamController>().tutorialActive = false;
                    p1Mana.toggleManaRegen();
                    manaOn = true;
                    panel.SetActive(false);
                }

                if(index == 21)
                {
                    pauseScene = false;
                    Time.timeScale = 1;
                    allowAdvance = false;
                    twoTeamController.GetComponent<TwoTeamController>().tutorialActive = false;
                    tutorialUI.SetActive(false);
                    enemy.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                }

            }
        }

        if (index == 10)
        {
            if (cardFinder[twoTeamController.GetComponent<TwoTeamController>().p1SelectedCard] == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    allowAdvance = true;
                    tutorialUI.SetActive(true);
                    twoTeamController.GetComponent<TwoTeamController>().tutorialActive = true;
                    if(!manaOn)
                    {
                        p1Mana.toggleManaRegen();
                        manaOn = true;
                    }
                }
            }
            else if(Input.GetMouseButtonDown(0))
            {
                if(p1Mana.GetManaAmount() < 15 && !manaOn)
                {
                    p1Mana.toggleManaRegen();
                    manaOn = true;  
                }
            }
        }

        if (index == 14)
        {
            if(findCardBool('b') && findCardBool('h') && findCardBool('s'))
            {
                allowAdvance = true;
                pauseScene = true;
                twoTeamController.GetComponent<TwoTeamController>().tutorialActive = true;
                panel.SetActive(true);
            }
        }


    }

    public void tutorial()
    {
        pauseScene = true;
        tutorialActive = true;
        tutorialUI.SetActive(true);
    }

    private int[] findCardLocation(char attackType)
    {
        int[] location = { 0, 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            if (playerHand.GetComponent<HandController>().hand[i].name[0] == attackType)
            {
                location[i] = 1;
            }
        }
        return location;

    }
    private bool findCardBool(char attackType)
    {
        for (int i = 0; i < 4; i++)
        {
            if(playerHand.GetComponent<HandController>().hand[i] == null)
            {
                return false;
            }
            if (playerHand.GetComponent<HandController>().hand[i].name[0] == attackType)
            {
                return true;
            }
        }
        return false;

    }

}
