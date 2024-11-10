using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckSelection : MonoBehaviour
{
    public int minCards = 5;
    public int currentCards = 0;
    [SerializeField] Text deckCounter;

    private void Start()
    {
        EventBus.Subscribe<EventCardAddedToDeck>(OnEventCardAddedToDeck);
        EventBus.Subscribe<EventCardRemovedFromDeck>(OnEventCardRemovedFromDeck);
    }

    private void Update()
    {
        // Update display count for cards in deck
        deckCounter.text = currentCards + "/" + minCards;
    }

    // If a card with a collider enters the deck selection box,
    // that gameObject will be added to the deck
    private void OnEventCardAddedToDeck(EventCardAddedToDeck e)
    {
        currentCards += 1;
        if (currentCards >= minCards)
        {
            deckCounter.color = Color.green;
        }
    }

    // If a card with a collider exits the deck selection box,
    // that gameObject will be removed from the deck
    private void OnEventCardRemovedFromDeck(EventCardRemovedFromDeck e)
    {
        currentCards -= 1;
        if (currentCards < minCards)
        {
            deckCounter.color = Color.red;
        }
    }

    public void OnConfirmSelection()
    {
        if(currentCards >= minCards)
        {
            //SceneManager.LoadScene("Overworld");
            EventBus.Publish(new CloseInventory());
        }
        else
        {
            StartCoroutine(FlashDeckCounter());
        }
    }

    private IEnumerator FlashDeckCounter()
    {
        for (int i = 0; i < 4; i++)
        {
            deckCounter.color = Color.white;
            yield return new WaitForSeconds(0.1f);
            deckCounter.color = Color.red;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
