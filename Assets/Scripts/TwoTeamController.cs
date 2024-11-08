using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoTeamController : MonoBehaviour
{
    public bool player2IsNPC;
    public GameObject enemySpawner;
    public GameObject player1Hand;
    public GameObject player2Hand;
    public List<GameObject> player1Chars; // characters start off as disabled, enable the first one in Start()
    public List<GameObject> player2Chars; // characters start off as disabled, enable the first one in Start()

    private HandController player1HandController;
    private HandController player2HandController;
    GameObject curPlayer1Char;
    GameObject curPlayer2Char;
    int player1CharsDead = 0;
    int player2CharsDead = 0;
    void Start()
    {
        EventBus.Subscribe<CharacterDead>(OnCharacterDead);

        if (player2Hand == null)
        {
            player2Hand = enemySpawner.GetComponentInChildren<HandController>().gameObject;
        }
        if (player2Chars[0] == null)
        {
            player2Chars[0] = enemySpawner.transform.GetChild(0).gameObject;
        }

        player1HandController = player1Hand.GetComponent<HandController>();
        player2HandController = player2Hand.GetComponent<HandController>();
        curPlayer1Char = player1Chars[0];
        curPlayer1Char.SetActive(true);
        curPlayer2Char = player2Chars[0];
        curPlayer2Char.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            EventBus.Publish(new SelectCard(player1HandController, 0));
        }
        else if (Input.GetKeyDown("2"))
        {
            EventBus.Publish(new SelectCard(player1HandController, 1));
        }
        else if (Input.GetKeyDown("3"))
        {
            EventBus.Publish(new SelectCard(player1HandController, 2));
        }
        else if (Input.GetKeyDown("4"))
        {
            EventBus.Publish(new SelectCard(player1HandController,3));
        }
        if (Input.GetMouseButtonDown(0))
        {
            player1HandController.TryPlayCard();
        }
        if (!player2IsNPC)
        {
            if (Input.GetKeyDown("7"))
            {
                EventBus.Publish(new SelectCard(player2HandController, 0));
            }
            else if (Input.GetKeyDown("8"))
            {
                EventBus.Publish(new SelectCard(player2HandController, 1));
            }
            else if (Input.GetKeyDown("9"))
            {
                EventBus.Publish(new SelectCard(player2HandController, 2));
            }
            else if (Input.GetKeyDown("0"))
            {
                EventBus.Publish(new SelectCard(player2HandController, 3));
            }
            if (Input.GetKeyDown("space"))
            {
                player2HandController.TryPlayCard();
            }
        } else
        {
            int healCardI = -1;
            Card healCard = null;
            int shieldCardI = -1;
            Card shieldCard = null;
            for (int i = 0; i < player2HandController.hand_size; i++)
            {
                if (player2HandController.hand[i] == null) return; // Avoids errors
                Card card = player2HandController.hand[i].GetComponent<Card>();
                if (card.healValue > 0)
                {
                    healCardI = i;
                    healCard = card;
                }
                else if (card.shieldValue > 0)
                {
                    shieldCardI = i;
                    shieldCard = card;
                }
                
            }
            if (curPlayer2Char == null) return; // Avoids errors
            CharacterHealth charHealth = curPlayer2Char.GetComponent<CharacterHealth>();
            float manaAmount = player2HandController.manaBar.GetComponent<ManaBar>().GetMana().GetManaAmount();
            //print(manaAmount);
            if (healCardI != -1 && charHealth.maxhealth - charHealth.health >= healCard.healValue) // If heal card exists and they can fully utilize the heal card, play it
            {
                if (manaAmount >= healCard.mana_cost)
                {
                    EventBus.Publish(new SelectCard(player2HandController, healCardI));
                    player2HandController.TryPlayCard();
                }
            } else if (shieldCardI != -1) // If shield card exists, play it
            {
                if (manaAmount >= shieldCard.mana_cost)
                {
                    EventBus.Publish(new SelectCard(player2HandController, shieldCardI));
                    player2HandController.TryPlayCard();
                }
            } else // Play the first card in hand
            {
                Card card = player2HandController.hand[0].GetComponent<Card>();
                if (manaAmount >= card.mana_cost)
                {
                    EventBus.Publish(new SelectCard(player2HandController, 0));
                    player2HandController.TryPlayCard();
                }
            }
        }
    }

    private void OnCharacterDead(CharacterDead e)
    {
        if (e.character == curPlayer1Char)
        {
            player1CharsDead++;
            if (player1CharsDead == player1Chars.Count)
            {
                EventBus.Publish(new EventTeamDead(e.character));
            } else
            {
                curPlayer1Char.SetActive(false);
                curPlayer1Char = player1Chars[player1CharsDead];
                curPlayer1Char.SetActive(true);
            }
        }
        if (e.character == curPlayer2Char)
        {
            player2CharsDead++;
            if (player2CharsDead == player2Chars.Count)
            {
                EventBus.Publish(new EventTeamDead(e.character));
            }
            else
            {
                curPlayer2Char.SetActive(false);
                curPlayer2Char = player1Chars[player1CharsDead];
                curPlayer2Char.SetActive(true);
            }
        }
    }
}
