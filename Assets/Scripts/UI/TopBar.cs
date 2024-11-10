using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Find("EnemyText").GetComponent<TextMeshProUGUI>().text = DataManager.Instance.encountered_enemy;
    }

}
