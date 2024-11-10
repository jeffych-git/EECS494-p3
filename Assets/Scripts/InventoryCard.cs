using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCard : MonoBehaviour
{
    public GameObject outline;
    public Text cardCountText;

    public bool inDeck = false;
    public int cardCount;
    public int variantIndex;
    public Vector2 deckPosition;

    private void Start()
    {
        EventBus.Subscribe<EventCardAddedToDeck>(OnEventCardAddedToDeck);
        EventBus.Subscribe<EventCardRemovedFromDeck>(OnEventCardRemovedFromDeck);
    }

    private void OnMouseOver()
    {
        cardCountText.text = "*" + cardCount;
        outline.transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, 0);
    }

    private void OnMouseExit()
    {
        outline.transform.position = new Vector3(200, 200, 0);
    }

    void OnMouseDown()
    {
        if (cardCount <= 0)
        {
            return;
        }

        if (!inDeck)
        {
            print("card is not in deck slot already");
            EventBus.Publish(new EventCardAddedToDeck(gameObject));
        }
        else if (inDeck)
        {
            print("card is in deck slot already");
            EventBus.Publish(new EventCardRemovedFromDeck(gameObject));
        }
    }

    private void OnEventCardAddedToDeck(EventCardAddedToDeck e)
    {
        if (inDeck && variantIndex == e.card.GetComponent<InventoryCard>().variantIndex)
        {
            cardCount++;
        }
    }
    private void OnEventCardRemovedFromDeck(EventCardRemovedFromDeck e)
    {
        if (!inDeck && variantIndex == e.card.GetComponent<InventoryCard>().variantIndex)
        {
            cardCount++;
        }
    }
}