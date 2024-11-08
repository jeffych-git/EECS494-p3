using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public GameObject hand;

    private Mana mana;
    private Image barImage;
    private void Awake()
    {
        barImage = transform.Find("bar").GetComponent<Image>();

        mana = new Mana(hand.GetComponent<HandController>());
    }

    private void Update()
    {
        mana.Update();

        barImage.fillAmount = mana.GatManaNormalized();
    }

    public Mana GetMana()
    {
        return mana;
    }
}

public class Mana
{
    public const int MANA_MAX = 100;

    private float manaAmount;
    private float manaRegenAmount;
    private HandController handController;

    public Mana(HandController h)
    {
        manaAmount = 0;
        manaRegenAmount = 10f;
        handController = h;
        EventBus.Subscribe<UseMana>(OnUseMana);
    }
    public void Update()
    {
        if (manaAmount <= MANA_MAX)
        {
            manaAmount += manaRegenAmount * Time.deltaTime;
        }
    }

    public void OnUseMana(UseMana e)
    {
        if (e.mana == this)
        {
            if (manaAmount >= e.amount)
            {
                manaAmount -= e.amount;
                EventBus.Publish(new PlaySelectedCard(handController));
            }
        }
    }

    public float GatManaNormalized()
    {
        return manaAmount / MANA_MAX;
    }

    public float GetManaAmount()
    {
        return manaAmount;
    }
}

