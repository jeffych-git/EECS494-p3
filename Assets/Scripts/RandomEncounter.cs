using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEncounter : MonoBehaviour
{
    public float encounterRate = 0.1f; //Odds of an encounter
    public string[] possible_enemies; // e.g. "Slime" or "Wolf"

    private float cut_scene_duration = 0.75f;
    private bool is_frozen = false;

    private void LateUpdate()
    {
        if (is_frozen)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= encounterRate)
            {

                string enemy_name = RandomlySelectEnemy();
                InitiateEnemy(enemy_name);
                EventBus.Publish(new BeginEncounter());

                StartCoroutine(DisableMovementAfter(other));
                if (encounterRate < 1)
                {
                    RandomBattleCutscene(other);
                }
                else
                {
                    StartCoroutine(BossBattleCutscene());
                }
                StartCoroutine(InitiateBattleAfter());
            }
        }
    }

    private string RandomlySelectEnemy()
    {
        int randomValue = Random.Range(0, possible_enemies.Length);

        for (int i = 0; i < possible_enemies.Length; i++)
        {
            if (i == randomValue)
            {
                Debug.Log("Randomly selected enemy: " + possible_enemies[i]);
                return possible_enemies[i];
            }
        }
        Debug.LogWarning("no enemies selected");
        return null;
    }

    private void InitiateEnemy(string enemy_name)
    {
        DataManager.Instance.encountered_enemy = enemy_name;
    }

    private void RandomBattleCutscene(Collider other)
    {
        //string prefab_path = "Prefabs/Enemy_Overworld_Prefabs/" + DataManager.Instance.encountered_enemy + "_Overworld";
        string prefab_path = "Prefabs/Enemy_Overworld_Prefabs/RandomEncounter_Overworld";


        // Load the prefab from the Resources folder
        GameObject prefab = Resources.Load<GameObject>(prefab_path);
        if (prefab != null)
        {
            // Instantiate the prefab at the specified position and rotation
            GameObject newObject = Instantiate(prefab);

            // Set the current GameObject as the parent of the new object
            newObject.transform.position = other.gameObject.transform.position + (Vector3.right * 15);

            StartCoroutine(CoroutineUtilities.MoveObjectOverTime(
                newObject.transform,
                newObject.transform.position,
                other.gameObject.transform.position,
                2 * cut_scene_duration / 3));
            StartCoroutine(CoroutineUtilities.ShowSpriteOverTimeAfter(
                newObject.transform,
                cut_scene_duration / 3,
                cut_scene_duration / 3));
        }
        else
        {
            Debug.LogWarning("Prefab not found at path: " + prefab_path);
        }
    }
    private IEnumerator BossBattleCutscene()
    {
        yield return null;
    }

    private IEnumerator InitiateBattleAfter()
    {
        yield return new WaitForSeconds(cut_scene_duration);
        SceneManager.LoadScene("PvE");
    }

/*    private IEnumerator ShowSpriteAfter(GameObject newObject, float duration)
    {
        Color color = newObject.GetComponent<SpriteRenderer>().color;
        color.a = 0;
        newObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(duration);
        StartCoroutine(CoroutineUtilities.ShowSpriteOverTime(
            newObject.transform,
            duration));
    }*/

    private IEnumerator DisableMovementAfter(Collider other)
    {
        yield return new WaitForSeconds(0);
        other.gameObject.GetComponent<PlayerMovement>().enabled = false;
        DataManager.Instance.player_position = other.transform.position;
    }
}

/*public class RandomEncounter : MonoBehaviour
{
    public float encounterRate = 0.1f; //Odds of an encounter
    public float stepRate = 0.01f; //How much odds of encounter increase with each step
    public float distance = 5f; //Minimum distance between encounters
    private Vector3 position;
    private bool moved = false; //Can only trigger encounter on movement
    private float moveDistance = 0f; //How far player has moved since last encounter check

    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMovedThisFrame = Vector3.Distance(transform.position, position);
        if (distanceMovedThisFrame > 0f)
        {
            moveDistance += distanceMovedThisFrame;
            moved = true;
        }
        else
        {
            moved = false;
        }
        position = transform.position;
        if (moveDistance >= distance && moved)
        {
            if (Random.value > encounterRate)
            {
                moveDistance = 0f;
                encounterRate = 0.1f;
                SceneManager.LoadScene("PvE");
            }
            else
            {
                encounterRate += stepRate;
            }
        }
    }
}
*/