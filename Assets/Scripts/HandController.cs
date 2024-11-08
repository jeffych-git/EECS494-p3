using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public GameObject deck;
    public GameObject discard_pile;
    public GameObject manaBar;
    public int hand_size = 4;
    public int team;

    private List<GameObject> card_slots = new List<GameObject>();
    public List<GameObject> hand = new List<GameObject>();
    public List<GameObject> temp = new List<GameObject>();
    private GameObject top_card;
    private DeckController dc;
    private DiscardPileController dpc;
    private Mana mana;
    //private float health;
    private bool is_stunned = false;
    private float card_move_duration = 0.5f;
    private float card_play_duration = 0.5f;
    private int selected_card_slot = 0;

    // Start is called before the first frame update
    void Start()
    {
        dc = deck.GetComponent<DeckController>();
        dpc = discard_pile.GetComponent<DiscardPileController>();
        mana = manaBar.GetComponent<ManaBar>().GetMana();
        card_slots = GetChildren(gameObject);
        EventBus.Subscribe<PlaySelectedCard>(OnPlaySelectedCard);
        EventBus.Subscribe<SelectCard>(OnSelectCard);
        EventBus.Subscribe<Charge>(OnCharge);
        DrawFromDeck();
    }

    //temorary controls for testing purposes.
    private void Update()
    {
        /*
        if(Input.GetKeyDown("0"))
        {
            EventBus.Publish(new SelectCard(0));
        }
        else if (Input.GetKeyDown("1"))
        {
            EventBus.Publish(new SelectCard(1));
        }
        else if (Input.GetKeyDown("2"))
        {
            EventBus.Publish(new SelectCard(2));
        }
        else if (Input.GetKeyDown("3"))
        {
            EventBus.Publish(new SelectCard(3));
        }
        if (Input.GetMouseButtonDown(0))
        {
            TryPlayCard();
        }*/

        //raise selected card
        if (hand[selected_card_slot])
        {
            hand[selected_card_slot].GetComponent<Card>().transform.position =
                new Vector3(hand[selected_card_slot].GetComponent<Card>().transform.position.x, -43, hand[selected_card_slot].GetComponent<Card>().transform.position.z);
        }
        for (int i = 0; i < hand_size; i++)
        {
            //lower all other cards
            if (hand[i])
            {
                if (i != selected_card_slot && hand[i])
                {
                    hand[i].GetComponent<Card>().transform.position = new Vector3(hand[i].GetComponent<Card>().transform.position.x, -45, hand[i].GetComponent<Card>().transform.position.z);
                }
                //dim if not enough mana
                if (hand[i].GetComponent<Card>().mana_cost * 10 >= mana.GetManaAmount())
                {
                    Color color = hand[i].GetComponent<SpriteRenderer>().color;
                    color.a = 0.7f;
                    hand[i].GetComponent<SpriteRenderer>().color = color;
                    hand[i].GetComponent<CanvasGroup>().alpha = 0.7f;
                }
                else
                {
                    Color color = hand[i].GetComponent<SpriteRenderer>().color;
                    color.a = 1;
                    hand[i].GetComponent<SpriteRenderer>().color = color;
                    hand[i].GetComponent<CanvasGroup>().alpha = 1f;
                }
            }
        }

    }

    private void DrawFromDeck()
    {
        for (int i = 0; i < hand_size; i++)
        {
            if (card_slots[i].GetComponent<CardSlot>().is_empty)
            {
                hand.Add(null);
                temp.Add(null);
                DrawCardToSlot(card_slots[i], i);
                EventBus.Publish(new DrawCard(dc));
            }
        }
    }
    private void DrawCardToSlot(GameObject card_slot, int slot_index)
    {
        top_card = dc.GetTopCard();
        //top_card.transform.position = card_slot.transform.position;
        StartCoroutine(AnimateCardDraw(top_card, card_slot));
        temp[slot_index] = top_card;
        StartCoroutine(DelayCardDraw(slot_index));
        //print("draw card to: " + slot_index);
        card_slots[slot_index].GetComponent<CardSlot>().is_empty = false;
    }

    private void PutCardInDiscardPile()
    {
        //print("discard this card: " + selected_card_slot);
        GameObject card = hand[selected_card_slot];
        StartCoroutine(AnimateDiscardCard(card));
        dpc.DiscardCard(card);
        hand[selected_card_slot] = null;
        card_slots[selected_card_slot].GetComponent<CardSlot>().is_empty = true;
    }
    /*private void FillHand()
    {
        for (int i = 0; i < card_slots.Count; i++)
        {
            DrawFromDeck();
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
    public void TryPlayCard()
    {
        //times 10 because each bar of mana is techincally 10 mana
        if (hand[selected_card_slot] != null && !is_stunned)
        {
            EventBus.Publish(new UseMana(mana, hand[selected_card_slot].GetComponent<Card>().mana_cost * 10));
        }
    }


    private void OnPlaySelectedCard(PlaySelectedCard e)
    {
        if (e.handController != this) return;

        Card card = hand[selected_card_slot].GetComponent<Card>();

        //Note: KEEP ALL POSSIBLE EVENTS CAUSED BY CARDS HERE:
        //---------------------------------------------------------
        if (card.attackValue > 0)
        {
            if (card.is_heavy)
            {
                //Publishes Heavy Attack Event (meaning with charge time)
                StartCoroutine(HeavyAttack(card));
            }
            else {
                //Publishes Light Attack Event
                EventBus.Publish(new Attack(
                    team,
                    card.attackValue,
                    card.dmg_delay,
                    card.is_ranged,
                    card.is_heavy,
                    card.visual_fx));
            }
        }
        if (card.healValue > 0)
        {
            if (card.is_heavy)
            {
                //Publishes Heavy Heal Event (meaning with charge time)
                StartCoroutine(HeavyHeal(card));
            }
            else
            {
                //Publishes Light Heal Event
                EventBus.Publish(new Heal(
                    team,
                    card.healValue,
                    card.is_heavy,
                    card.visual_fx));
            }
        }
        if (card.block_duration > 0)
        {
            EventBus.Publish(new Block(team, card.block_duration));
        }
        if (card.shieldValue > 0)
        {
            if (!card.is_heavy)
            {
                //Light Shield
                EventBus.Publish(new EventAddShield(card.shieldValue, team, card.is_heavy, card.visual_fx));
            }
        }
        //---------------------------------------------------------

        PutCardInDiscardPile();
        DrawCardToSlot(card_slots[selected_card_slot], selected_card_slot);
        EventBus.Publish(new DrawCard(dc));
    }

    private void OnSelectCard(SelectCard e)
    {
        if (e.handController != this) return;
        selected_card_slot = e.card_slot_index;
    }

    private void OnCharge(Charge e)
    {
        if (e.team == team)
        {
            StartCoroutine(StunForSeconds(e.duration));
        }
    }

    private IEnumerator AnimateDiscardCard(GameObject card)
    {
        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
            card.transform,
            card.transform.position,
            card.transform.position + new Vector3(0, 20, 0),
            card_play_duration));
        StartCoroutine(CoroutineUtilities.FadeSpriteOverTime(
            card.transform,
            card_play_duration));
        StartCoroutine(CoroutineUtilities.FadeCanvasGroupOverTime(
            card.GetComponent<CanvasGroup>(),
            card_play_duration));
        yield return new WaitForSeconds(card_play_duration + 0.05f);
        ResetCard(card);
    }

    private IEnumerator AnimateCardDraw(GameObject top_card, GameObject card_slot)
    {
        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
            top_card.transform,
            top_card.transform.position,
            card_slot.transform.position,
            card_move_duration));
        yield return new WaitForSeconds(card_move_duration);
        top_card.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
        StartCoroutine(CoroutineUtilities.RotateObjectOverTime(
            top_card.transform,
            'y',
            180f,
            card_move_duration));
        StartCoroutine(CoroutineUtilities.ToggleSpriteHalfway(
            top_card.GetComponent<SpriteRenderer>(),
            card_move_duration));
    }

    private void ResetCard(GameObject card)
    {
        //card.GetComponent<SpriteRenderer>().enabled = false;
        //reset opacity
        Color color = card.GetComponent<SpriteRenderer>().color;
        color.a = 1;
        card.GetComponent<SpriteRenderer>().color = color;
        card.GetComponent<CanvasGroup>().alpha = 1;

        //enable cardback
        card.transform.GetChild(0).gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;

        //reset rotation
        card.transform.rotation = Quaternion.Euler(0, 180, 0);

        dpc.MoveCardtoDiscardPile(card);
    }


    private IEnumerator DelayCardDraw(int slot_index)
    {
        yield return new WaitForSeconds(card_move_duration * 2);
        hand[slot_index] = temp[slot_index];
        yield return null;
    }

    private IEnumerator HeavyAttack(Card card)
    {
        float duration = card.charge_duration;
        if (duration > 0)
        {
            EventBus.Publish(new Charge(team, duration));
            yield return new WaitForSeconds(duration);
        }
        EventBus.Publish(new Attack(
            team,
            card.attackValue,
            card.dmg_delay,
            card.is_ranged,
            card.is_heavy,
            card.visual_fx));
    }
    private IEnumerator HeavyHeal(Card card)
    {
        float duration = card.charge_duration;
        if (duration > 0)
        {
            EventBus.Publish(new Charge(team, duration));
            yield return new WaitForSeconds(duration);
        }
        EventBus.Publish(new Heal(
            team,
            card.healValue,
            card.is_heavy,
            card.visual_fx));
    }

    private IEnumerator StunForSeconds(float duration)
    {
        is_stunned = true;
        yield return new WaitForSeconds(duration);
        is_stunned = false;
    }
}

