using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutOnEncounter : MonoBehaviour
{
    private CanvasGroup cg;
    public float cutscene_duration = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        EventBus.Subscribe<BeginEncounter>(OnBeginEncounter);
    }

    // Update is called once per frame
    private void OnBeginEncounter(BeginEncounter e)
    {
        StartCoroutine(FadeOutAfter());
    }

    private IEnumerator FadeOutAfter()
    {
        yield return new WaitForSeconds(2 * cutscene_duration / 3);
        StartCoroutine(CoroutineUtilities.FadeCanvasGroupOverTime(cg, cutscene_duration / 3));
    }
}
