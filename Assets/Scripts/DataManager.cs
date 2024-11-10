using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public Vector3 player_position;
    public Vector3 respawn_point;
    public string encountered_enemy;

    //These three arrays should all be the same length
    public GameObject[] all_card_prefabs; 
    public int[] player_deck;
    public int[] owned_cards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        respawn_point = player_position;
    }
}
