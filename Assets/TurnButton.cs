using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController player;
    public bool isRight;
    //void Update()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        if(player.accelerating || player.decelerating)
    //        {
    //            if (isRight)
    //            {
    //                player.TurnRight();
    //            }
    //            else
    //            {
    //                player.TurnLeft();
    //            }
    //        }
    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isRight)
        {
            player.TurnRight();
        }
        else
        {
            player.TurnLeft();
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        player.StopTurn();
    }
}
