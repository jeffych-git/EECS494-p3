using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] Image redbar;
    public float maxhealth = 10;
    public float health = 10;
    public int team;

    [SerializeField] Image yellowbar;
    [SerializeField] float maxshield = 10;
    [SerializeField] float shield = 2;
    [SerializeField] float shieldDecayAmount = 0.3f;

    private bool is_blocking = false;
    private bool is_shielding = false;
    private void Start()
    {
        if (health <= 0)
        {
            EventBus.Publish(new CharacterDead(gameObject));
        }
        redbar.transform.localScale = new Vector3(health / maxhealth, 1, 1);
        yellowbar.transform.localScale = new Vector3(shield / maxshield, 1, 1);
        EventBus.Subscribe<Attack>(OnAttack);
        EventBus.Subscribe<Heal>(OnHeal);
        EventBus.Subscribe<Block>(OnBlock);
        EventBus.Subscribe<EventAddShield>(OnEventAddShield);
    }

    private void FixedUpdate()
    {
        health = Mathf.Clamp(health, 0, maxhealth);
        shield = Mathf.Clamp(shield, 0, maxshield);
        redbar.transform.localScale = new Vector3(health / maxhealth, 1, 1);
        yellowbar.transform.localScale = new Vector3(shield / maxshield, 1, 1);

        if (shield > 0)
        {
            shield -= shieldDecayAmount * Time.deltaTime;
            is_shielding = true;
        }
        else if (shield <= 0 && is_shielding)
        {
            EventBus.Publish(new ShieldDestroyed());
            is_shielding = false;
        }
    }

    private void OnAttack(Attack e)
    {
        if (e.team != team)
        {
            StartCoroutine(DelayTakeDamage(e));
        }
    }

    private void OnHeal(Heal e)
    {
        if (e.team == team)
        {
            health += e.amount;
        }
    }
    private void OnEventAddShield(EventAddShield e)
    {
        if (e.team == team)
        {
            shield += e.amount;
            is_shielding = true;
        }
    }

    private void OnBlock(Block e)
    {
        if (e.team == team)
        {
            StartCoroutine(BlockForSeconds(e));
        }
    }

    private IEnumerator DelayTakeDamage(Attack e)
    {
        yield return new WaitForSeconds(e.dmg_delay);

        if (!is_blocking)
        {
            if(shield > 0)
            {
                shield += e.amount;
            }
            else
            {
                health += e.amount;
                EventBus.Publish(new TakeDMG(e.team));
            }
        }

        if (health <= 0)
        {
            EventBus.Publish(new CharacterDead(gameObject));
            Destroy(gameObject); // For now to make it obvious
        }
    }

    private IEnumerator BlockForSeconds(Block e)
    {
        is_blocking = true;
        yield return new WaitForSeconds(e.block_duration);
        is_blocking = false;
    }
}
