using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public Transform wheel;
    public Transform center;
    public GameManager gameManager;
    bool IsEleminated = false;
    public bool accelerating = false;
    public bool decelerating = false;
    public bool isTurningRight = false;
    public bool isTurningLeft = false;

    private float vibrationTimer = 0f;
    private float vibrationCoolDown = 2f;

    public int collisionPushForce = 200;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        center = GameObject.FindGameObjectWithTag("Center").transform;
        gameManager = FindObjectOfType<GameManager>();
    }
    private void FixedUpdate()
    {
        vibrationTimer += Time.deltaTime;
        if (!IsEleminated)
        {
            //HandlePCInput();
            float distance = Vector3.Distance(transform.position, center.position);
            if (distance > 30)
            {
                GetEliminated();
            }
            if (accelerating)
            {
                Accelerate();
            }
            else if(decelerating)
            {
                Decelerate();
            }
            if(accelerating || decelerating)
            {
                if (isTurningRight)
                {
                    TurnRight();
                }
                else if (isTurningLeft)
                {
                    TurnLeft();
                }
            }

        }
        
    }

    private void GetEliminated()
    {
        gameManager.DecreaseNumOfCars(gameObject);
        rb.isKinematic = true;
        gameManager.showLoseScreen();
        IsEleminated = true;
    }

    //private void HandlePCInput()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        Accelerate();
    //    }
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        Decelerate();
    //    }
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        TurnRight();
    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        TurnLeft();
    //    }
    //}

    public void Accelerate()
    {
        rb.AddRelativeForce(Vector3.forward * 16);
    }
    public void Decelerate()
    {

        rb.AddRelativeForce(Vector3.back * 15);
    }
    public void TurnRight()
    {

        transform.Rotate(new Vector3(0, 1, 0), 3);
        if (wheel.localRotation.z > -0.5)
        {
            wheel.Rotate(new Vector3(0, 0, -1), 2);
        }
    }
    public void TurnLeft()
    {
        if (wheel.localRotation.z < 0.5)
        {
            wheel.Rotate(new Vector3(0, 0, 1), 2);
        }
        transform.Rotate(new Vector3(0, 1, 0), -3);
    }

    public void Stop()
    {
        accelerating = false;
        decelerating = false;
    }
    public void StopTurn()
    {
        isTurningRight = false;
        isTurningLeft = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(vibrationTimer > vibrationCoolDown)
            {
                Handheld.Vibrate();
                vibrationTimer = 0f;
            }

            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;      //push away from collision point
            rb.AddForce(dir * collisionPushForce);

            if (rb.velocity.y < 1)      //prevent jumping too much
            {
                rb.AddRelativeForce(Vector3.up * 50); //get shaken
            }

        }

    }
}
