using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] prefabs;

    void Start()
    {
        // Spawn the available cards and deck cards
        // TODO: Use saved positions for each card
        for (int i = 0; i < 8; i++)
        {
            SpawnDraggableCard(new Vector3(-40+i*5, 11, 100+i), prefabs[i%5]);
        }
    }

    void SpawnDraggableCard(Vector3 position, GameObject prefab)
    {
        GameObject spawnedObject = Instantiate(prefab, position, Quaternion.Euler(0, 0, 0));
        spawnedObject.AddComponent<Draggable>();
        spawnedObject.AddComponent<BoxCollider2D>();
        BoxCollider2D dragCollider = spawnedObject.GetComponent<BoxCollider2D>();
        dragCollider.offset = new Vector2(0, -0.05f);
        dragCollider.size = new Vector2(1.25f, 1.7f);

        SpriteRenderer[] backSprite = spawnedObject.GetComponentsInChildren<SpriteRenderer>();
        backSprite[0].sortingOrder = 4;
        backSprite[1].color = new Color(0, 0, 0, 0);
    }
}
