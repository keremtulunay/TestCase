using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController player;
    public bool isRight;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRight)
        {
            player.isTurningRight = true;
        }
        else
        {
            player.isTurningLeft = true;
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        player.isTurningRight = false;
        player.isTurningLeft = false;
    }
}
