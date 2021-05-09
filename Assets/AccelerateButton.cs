using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccelerateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public PlayerController player;
    public bool isAccelerate;
    //void FixedUpdate()
    //{
    //    if (Input.GetMouseButton(0))
    //    {

    //        if (isAccelerate)
    //        {
    //            player.Accelerate();
    //        }
    //        else
    //        {
    //            player.Decelerate();
    //        }

    //    }
    //}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isAccelerate)
        {
            player.Accelerate();
        }
        else
        {
            player.Decelerate();
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        player.Stop();
    }

}
