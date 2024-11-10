using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    // The prefabs list must be an updated list of all the possible cards available, in order
    [SerializeField] GameObject[] prefabs;

    // This is loaded from the dataManager, must share the order in the prefabs list
    private int[] current_deck;
    private int[] owned_cards;

    [SerializeField] GameObject highlightBox;
    [SerializeField] Text counterText;

    private bool[,] deck_positions;

    void Start()
    {
        EventBus.Subscribe<EventCardAddedToDeck>(OnEventCardAddedToDeck);
        EventBus.Subscribe<EventCardRemovedFromDeck>(OnEventCardRemovedFromDeck);

        prefabs = DataManager.Instance.all_card_prefabs;
        current_deck = DataManager.Instance.player_deck;
        owned_cards = DataManager.Instance.owned_cards;
        if (current_deck.Length != prefabs.Length)
        {
            Debug.Log("InventoryManager does not have the correct length prefabs list. " +
                "Please update to the same amount of card types in the player_deck list in DataManager.");
        }
        if (owned_cards.Length != prefabs.Length)
        {
            Debug.Log("InventoryManager does not have the correct length prefabs list. " +
                "Please update to the same amount of card types in the owned_cards list in DataManager.");
        }

        Vector3 spawnPosition;
        float row_offset = 9.5f;
        float column_offset = 7.2f;
        int num_per_row = 7;
        
        for (int i = 0; i < owned_cards.Length; i++)
        {
            spawnPosition = new Vector3(-45 + column_offset * (i % num_per_row), 11 - row_offset * (i / num_per_row), 100);
            SpawnCardVariant(spawnPosition, prefabs[i], false, owned_cards[i], i, Vector2.zero);
        }

        // Initialize deck_positions for the graphical deck display
        deck_positions = new bool[4, num_per_row];
        for(int i = 0; i < 4; i++)
        {
            for(int j = 0; j < num_per_row; j++)
            {
                deck_positions[i,j] = false;
            }
        }

        SpawnExistingCards();
    }

    void SpawnCardVariant(Vector3 position, GameObject prefab, bool in_deck, int num_cards, int variant_index, Vector2 deck_position)
    {
        // Spawn the prefab at a set location with desired rotation
        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.Euler(0, 0, 0));
        SceneManager.MoveGameObjectToScene(spawnedObject, SceneManager.GetSceneByName("InventoryMenu"));


        // Add the inventoryCard script to control the interactions in the inventory
        spawnedObject.AddComponent<InventoryCard>();
        InventoryCard card = spawnedObject.GetComponent<InventoryCard>();
        card.outline = highlightBox;
        card.cardCountText = counterText;
        card.inDeck = in_deck;
        card.cardCount = num_cards;
        card.variantIndex = variant_index;
        card.deckPosition = deck_position;

        // To make the object clickable, give it a collider
        spawnedObject.AddComponent<BoxCollider2D>();
        BoxCollider2D dragCollider = spawnedObject.GetComponent<BoxCollider2D>();
        dragCollider.offset = new Vector2(0, -0.05f);
        dragCollider.size = new Vector2(1.25f, 1.7f);

        // Removes the back of the card and makes the card line up properly in the scene
        SpriteRenderer[] backSprite = spawnedObject.GetComponentsInChildren<SpriteRenderer>();
        backSprite[0].sortingOrder = 4;
        backSprite[1].color = new Color(0, 0, 0, 0);
    }

    private void SpawnExistingCards()
    {
        // Add Current cards already into deck TODO: Landen

    }



    private void OnEventCardAddedToDeck(EventCardAddedToDeck e)
    {
        InventoryCard card = e.card.GetComponent<InventoryCard>();
        card.cardCount--;
        current_deck[card.variantIndex]++;

        if (current_deck[card.variantIndex] > 1)
        {
            return;
        }

        // Spawn the new card graphic at the first open spot in the deck box
        Vector3 spawnPosition;
        float row_offset = 9.5f;
        float column_offset = 7.2f;
        int num_per_row = 5;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < num_per_row; j++)
            {
                if (deck_positions[i,j] == false)
                {
                    print("deck slot empty");
                    spawnPosition = new Vector3(11 + column_offset * j, 6 - row_offset * i, 100);
                    SpawnCardVariant(spawnPosition, prefabs[card.variantIndex], true, 1, card.variantIndex, new Vector2(i, j));
                    deck_positions[i,j] = true;
                    return;
                }
                print("deck slot full");
            }
        }
    }

    private void OnEventCardRemovedFromDeck(EventCardRemovedFromDeck e)
    {
        InventoryCard card = e.card.GetComponent<InventoryCard>();
        card.cardCount--;
        current_deck[card.variantIndex]--;

        if (card.cardCount <= 0)
        {
            deck_positions[(int)card.deckPosition.x, (int)card.deckPosition.y] = false;
            Destroy(e.card);
        }
    }
}
