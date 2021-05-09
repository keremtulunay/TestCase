using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccelerateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController player;
    public bool isAccelerate;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isAccelerate)
        {
            player.accelerating = true;
        }
        else
        {
            player.decelerating = true;
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        player.accelerating = false;
        player.decelerating = false;
    }

}
