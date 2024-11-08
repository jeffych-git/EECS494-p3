using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    //public GameObject hand;
    public GameObject discard_object;
    private List<GameObject> cards = new List<GameObject>();
    public List<GameObject> current_pile = new List<GameObject>();
    public List<GameObject> discard_pile = new List<GameObject>();
    //private DiscardPileController dpc; 

/*    public GameObject current_pile_object;
    public GameObject discard_pile_object;*/

    /*private int hand_size;
    public int selected_card = 0;
    private int current_pile_index = 0;
    private int discard_pile_index = 0;*/
    // Start is called before the first frame update
    void Start()
    {
        //hand_size = hand.Length;
        //hand_size = GetChildren(hand).Count;
        cards = GetChildren(gameObject);
        current_pile = cards;
        //dpc = discard_object.GetComponent<DiscardPileController>();
        EventBus.Subscribe<DrawCard>(OnDrawCard);
        EventBus.Subscribe<PlaySelectedCard>(OnPlaySelectedCard);
        EventBus.Subscribe<ReshuffleDeck>(OnReshuffleDeck);
        ShuffleDeck();
        //FillHand();
    }

    void Update()
    {
        discard_pile = discard_object.GetComponent<DiscardPileController>().discard_pile;
    }

    /*    private void CopyCardsToCurrentPile(List<GameObject> card_list)
        { 
            int index = 0;
            foreach (GameObject card in card_list)
            {
                GameObject card_copy = new GameObject("card");
                card_copy = card;

                card_copy.transform.SetParent(current_pile_object.transform);
                card_copy.transform.localPosition = Vector3.zero;

                current_pile[index] = card;
                index++;
            }
        }*/

    private List<GameObject> GetChildren(GameObject g)
    {
        List<GameObject> children = new List<GameObject>();
        // Loop through all children of a GameObject
        foreach (Transform child in g.transform)
        {
            children.Add(child.gameObject);
        }
        return children;
    }

    public GameObject GetTopCard()
    {
        return current_pile[0];
    }

    public int GetRemainingCardCount()
    {
        return current_pile.Count;
    }
    private void RemoveCardFromDeck()
    {
        if (current_pile.Count <= 0)
        {
            print("NO CARDS LEFT IN DECK");
            return;
        }
        current_pile.RemoveAt(0);
    }

    private void ShuffleDeck()
    {
        System.Random random = new System.Random();
        int n = current_pile.Count;
        for (int i = 0; i < n; i++)
        {
            int j = random.Next(i, n);
            GameObject temp = current_pile[i];
            current_pile[i] = current_pile[j];
            current_pile[j] = temp;
        }
    }

    private void OnReshuffleDeck(ReshuffleDeck e)
    {
        if (e.deckController != this) return;
        ShuffleDeck();
        //dpc.discard_pile.Clear();
    }

    private void OnDrawCard(DrawCard e)
    {
        if (e.deckController != this) return;
        RemoveCardFromDeck();
    }
    private void OnPlaySelectedCard(PlaySelectedCard e)
    {
        
    }

}
