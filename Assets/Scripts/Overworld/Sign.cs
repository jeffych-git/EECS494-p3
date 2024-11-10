using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    public GameObject dialog;
    public GameObject guidance;

    private Coroutine cr;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (guidance.activeInHierarchy == false && cr == null)
            {
                cr = StartCoroutine(CoroutineUtilities.SetActiveAfterSeconds(guidance, 3));
            }
            if (Input.GetKeyDown("q"))
            {
                guidance.SetActive(false);
                StopCoroutine(cr);
                if (dialog.activeInHierarchy)
                {
                    dialog.SetActive(false);
                }
                else
                {
                    dialog.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guidance.SetActive(false);
            dialog.SetActive(false);
        }
    }
}
