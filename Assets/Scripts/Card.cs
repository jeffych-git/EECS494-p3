using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int mana_cost;
    public int attackValue = 0;
    public int healValue = 0;
    public int shieldValue = 0;

    //After acting, time before damage affects enemy
    public float dmg_delay = 0;

    //Time Before Action after Card Played
    public float charge_duration = 0;

    //Blocks all incoming dmg for x seconds
    public float block_duration = 0;

    //Action Type
    public bool is_ranged = false; // - ranged means no contact

    public bool is_heavy = false; // - heavy charges before acting and is stunned while charging

    //Any Additional Visual FX when Card Played
    public GameObject visual_fx;
}
