using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineUtilities : MonoBehaviour
{

    public static IEnumerator MoveObjectOverTime(
        Transform target, Vector3 initial_pos, Vector3 dest_pos, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;
            Vector3 new_position = Vector3.Lerp(initial_pos, dest_pos, progress);

            target.position = new_position;

            yield return null;
        }
        if (target)
        {
            target.position = dest_pos;
        }
    }

    public static IEnumerator RotateObjectOverTime(
        Transform target, char axis, float degrees, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;
        float time_elapsed = initial_time;

        while (progress < 1.0f)
        {
            time_elapsed = Time.time - time_elapsed;
            progress = (Time.time - initial_time) / duration_sec;


            switch (axis)
            {
                case 'x':
                    target.Rotate((time_elapsed / duration_sec) * degrees, 0, 0, Space.Self);
                    break;
                case 'y':
                    target.Rotate(0, (time_elapsed / duration_sec) * degrees, 0, Space.Self);
                    break;
                case 'z':
                    target.Rotate(0, 0, (time_elapsed / duration_sec) * degrees, Space.Self);
                    break;
                default:
                    print("invalid axis");
                    break;
            }

            time_elapsed = Time.time;

            yield return null;
        }
        target.rotation = Quaternion.identity;
    }

    public static IEnumerator FadeSpriteOverTime(
        Transform target, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;
        Color initial_color = target.gameObject.GetComponent<SpriteRenderer>().color;
        Color color = target.gameObject.GetComponent<SpriteRenderer>().color;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            color.a = initial_color.a * (1 - progress);
            target.gameObject.GetComponent<SpriteRenderer>().color = color;

            yield return null;
        }
    }

    public static IEnumerator ShowSpriteOverTime(
        Transform target, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;
        Color initial_color = target.gameObject.GetComponent<SpriteRenderer>().color;
        Color color = target.gameObject.GetComponent<SpriteRenderer>().color;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            color.a = initial_color.a * (progress);
            target.gameObject.GetComponent<SpriteRenderer>().color = color;

            yield return null;
        }
    }

    public static IEnumerator ShowSpriteOverTimeAfter(
        Transform target, float wait_for, float fade_duration)
    {
        Color initial_color = target.gameObject.GetComponent<SpriteRenderer>().color;
        Color color = target.gameObject.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        target.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(wait_for);
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / fade_duration;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / fade_duration;

            color.a = initial_color.a * (progress);
            target.gameObject.GetComponent<SpriteRenderer>().color = color;

            yield return null;
        }
    }

    public static IEnumerator FadeCanvasGroupOverTime(
        CanvasGroup target, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            target.alpha = (1 - progress);

            yield return null;
        }
        target.alpha = 0;
    }

    public static IEnumerator ShowCanvasGroupOverTime(
        CanvasGroup target, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            target.alpha = (progress);

            yield return null;
        }
        target.alpha = 1;
    }
    public static IEnumerator ToggleSpriteHalfway(
        SpriteRenderer sr, 
        float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;
        Transform card_back = sr.transform.GetChild(0);

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            if (progress >= 0.5f)
            {
                card_back.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }

            yield return null;
        }
    }

    public static IEnumerator LoopThroughSpritesOnce(
        SpriteRenderer sr, 
        Sprite [] sprites, 
        float frame_speed)
    {
        foreach (Sprite sprite in sprites)
        {
            sr.sprite = sprite;
            yield return new WaitForSeconds(frame_speed);
        }
    }


    public static IEnumerator MakeSpriteColorOverTime(
        Transform target, Color targetColor, float duration_sec)
    {
        float initial_time = Time.time;
        float progress = (Time.time - initial_time) / duration_sec;
        Color startColor = target.gameObject.GetComponent<SpriteRenderer>().color;

        while (progress < 1.0f)
        {
            progress = (Time.time - initial_time) / duration_sec;

            target.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, targetColor, progress);

            yield return null;
        }
    }
    public static IEnumerator StartContinuousAnimation(SpriteRenderer sr, Sprite[] sprites, float frame_speed)
    {
        while (true)
        {
            foreach (Sprite sprite in sprites)
            {
                sr.sprite = sprite;
                yield return new WaitForSeconds(frame_speed);
            }
        }
    }

    public static IEnumerator SetActiveAfterSeconds(GameObject g, float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        g.SetActive(true);
    }
}
