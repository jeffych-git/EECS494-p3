using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateSprite : MonoBehaviour
{
    public Sprite[] idle_animation;
    public Sprite[] attack_animation;
    public Sprite[] block_animation;
    public Sprite[] shield_animation;
    public Sprite[] charge_attack_animation;

    private float frame_speed = 0.5f;

    private GameObject shield;
    private SpriteRenderer sr;
    private Coroutine idle_animation_coroutine;
    private Color original_color;
    private int projectile_dist = 120;
    private bool is_shielded = false;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        EventBus.Subscribe<Attack>(OnAttack);
        EventBus.Subscribe<Heal>(OnHeal);
        EventBus.Subscribe<Block>(OnBlock);
        EventBus.Subscribe<EventAddShield>(OnEventAddShield);
        EventBus.Subscribe<ShieldDestroyed>(OnShieldDestroyed);
        EventBus.Subscribe<TakeDMG>(OnTakeDMG);
        EventBus.Subscribe<Charge>(OnCharge);
        idle_animation_coroutine = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(
            sr,
            idle_animation,
            frame_speed));

        original_color = sr.color;
    }
    private void OnAttack(Attack e)
    {
        if (CompareTag("Team " + e.team))
        {
            if (e.is_ranged)
            {
                StartCoroutine(ShootProjectile(e));
                StartCoroutine(PlayAnimation(attack_animation, 0));
            }
            else
            {
                StartCoroutine(PlayAnimation(attack_animation, e.dmg_delay));
                StartCoroutine(MoveCharacter(new string[] { "forward", "back" }, frame_speed / 4, 5));
            }
        }
        return;
    }

    private void OnHeal(Heal e)
    {
        if (CompareTag("Team " + e.team))
        {
            Color green = new Color(0.5f, 1f, 0.5f, 1f); // Light green
            StartCoroutine(ChangeSpriteColorFor(green, frame_speed));
        }
        return;
    }

    private void OnBlock(Block e)
    {
        if (CompareTag("Team " + e.team))
        {
            Color blue = new Color(0.68f, 0.85f, 1f, 1f); // Light blue with full opacity
            StartCoroutine(ChangeSpriteColorFor(blue, e.block_duration));
            StartCoroutine(PlayAnimationFor(block_animation, e.block_duration));
        }
    }

    private void OnEventAddShield(EventAddShield e)
    {
        if (CompareTag("Team " + e.team) && !is_shielded)
        {
            StartCoroutine(PlayAnimation(shield_animation, 0));
            DisplayShield(e);
            is_shielded = true;
        }
    }

    private void OnShieldDestroyed(ShieldDestroyed e)
    {
        if (shield != null)
        {
            Destroy(shield);
            is_shielded = false;
        }
    }

    private void OnTakeDMG(TakeDMG e)
    {
        if (!CompareTag("Team " + e.team))
        {
            Color red = Color.red;
            StartCoroutine(ChangeSpriteColorFor(red, frame_speed / 3));
        }
    } 

    private void OnCharge(Charge e)
    {
        if (CompareTag("Team " + e.team))
        {
            Color yellow = Color.yellow;
            StartCoroutine(ChangeSpriteColorFor(yellow, e.duration));
            StartCoroutine(PlayAnimationFor(charge_attack_animation, e.duration));
            StartCoroutine(MoveCharacter(new string[] { "back", "forward" }, e.duration, 7));
        }
        return;
    }

    private void DisplayShield(EventAddShield e)
    {
        shield = Instantiate(e.visual_fx, transform.position, Quaternion.identity);
    }

    private IEnumerator PlayAnimation(Sprite [] animation, float delay)
    {
        if (idle_animation_coroutine != null)
        {
            StopCoroutine(idle_animation_coroutine);
        }
        yield return new WaitForSeconds(delay);
        StartCoroutine(CoroutineUtilities.LoopThroughSpritesOnce(sr, animation, frame_speed));
        yield return new WaitForSeconds(frame_speed * animation.Length);
        idle_animation_coroutine = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(
            sr, 
            idle_animation, 
            frame_speed));
    }

    private IEnumerator ShootProjectile(Attack e)
    {
        GameObject proj = Instantiate(e.visual_fx, transform.position, Quaternion.identity);
        int dist;
        if (e.team == 2)
        {
            dist = -projectile_dist;
            proj.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            dist = projectile_dist;
        }

        StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
            proj.transform,
            transform.position,
            transform.position + new Vector3(dist, 0, 0),
            e.dmg_delay));
        yield return new WaitForSeconds(e.dmg_delay);
        Destroy(proj);
    }

    private IEnumerator ChangeSpriteColorFor(Color color, float duration)
    {
        sr.color = color;
        yield return new WaitForSeconds(duration);
        sr.color = original_color;
    }

    private IEnumerator PlayAnimationFor(Sprite [] sprites, float duration)
    {
        if (idle_animation_coroutine != null)
        {
            StopCoroutine(idle_animation_coroutine);
        }
        StartCoroutine(CoroutineUtilities.LoopThroughSpritesOnce(sr, sprites, duration / sprites.Length));
        yield return new WaitForSeconds(duration);
        idle_animation_coroutine = StartCoroutine(CoroutineUtilities.StartContinuousAnimation(
            sr,
            idle_animation,
            frame_speed));
    }

    private IEnumerator MoveCharacter(string[] directions, float duration, int distance) // direcions: "forward" or "back"
    {
        if (CompareTag("Team 2"))
        {
            distance = -distance;
        }
        foreach (string direction in directions)
        {
            if (direction == "forward")
            {
                StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
                    transform,
                    transform.position,
                    transform.position + new Vector3(distance, 0, 0),
                    duration / directions.Length));
            }
            else if (direction == "back")
            {
                StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
                    transform,
                    transform.position,
                    transform.position + new Vector3(-distance, 0, 0),
                    duration / directions.Length));
            }
            yield return new WaitForSeconds(duration / directions.Length);
        }
    }
}
