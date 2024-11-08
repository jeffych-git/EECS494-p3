using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private float zCoord;
    private bool inDeck;

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    private void OnMouseUp()
    {
        Vector2 point = transform.position;
        Vector2 size = new Vector2(42, 39);
        Vector2 center = new Vector2(28f, -3.2f);

        // Check if the card is inside the deck selection box
        if (point.x >= center.x - size.x / 2 && point.x <= center.x + size.x / 2 &&
               point.y >= center.y - size.y / 2 && point.y <= center.y + size.y / 2)
        {
            if (!inDeck)
            {
                EventBus.Publish(new EventCardAddedToDeck(gameObject));
                inDeck = true;
            }
        }
        else if(inDeck)
        {
            EventBus.Publish(new EventCardRemovedFromDeck(gameObject));
            inDeck = false;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}