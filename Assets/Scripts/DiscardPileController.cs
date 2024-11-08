using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardPileController : MonoBehaviour
{
    public List<GameObject> discard_pile = new List<GameObject>();
    //public GameObject hand;
    public GameObject deck;
    private DeckController dc;
    //private HandController hc;

    private void Start()
    {
        //hc = hand.GetComponent<HandController>();
        dc = deck.GetComponent<DeckController>();
    }

    private void Update()
    {
        if(dc.GetRemainingCardCount() <= 0)
        {
            MoveCardsBack();
            EventBus.Publish(new ReshuffleDeck(dc));
            discard_pile.Clear();
        }
    }

    public void DiscardCard(GameObject card)
    {
        discard_pile.Add(card);
    }

    public void MoveCardtoDiscardPile(GameObject card)
    {
        //this puts card to the current pile now because the animation time messes it up. 
        //you can try to fix if you would like.
        card.transform.position = deck.transform.position;
    }

    private void MoveCardsBack()
    {
        foreach(GameObject card in discard_pile)
        {
            card.transform.position = deck.transform.position;
            dc.current_pile.Add(card);
        }
    }
}
