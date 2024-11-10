using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public Sprite[] sprites;
    public float frame_speed = 0.2f;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DataManager.Instance.respawn_point = transform.position;
            StartCoroutine(SpawnpointSetAnimation());
        }
    }

    private IEnumerator SpawnpointSetAnimation()
    {
        SpriteRenderer effect = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(CoroutineUtilities.LoopThroughSpritesOnce(
            effect,
            sprites,
            frame_speed));
        StartCoroutine(CoroutineUtilities.MakeSpriteColorOverTime(transform, Color.white, frame_speed * sprites.Length));
        yield return new WaitForSeconds(frame_speed * sprites.Length);
        effect.sprite = null;
    }
}
