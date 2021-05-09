using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, Car
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
            float distance = Vector3.Distance(transform.position, center.position);
            if (distance > 30)
            {
                GetEliminated();
            }
            //HandleInput();
            if (accelerating)
            {
                rb.AddRelativeForce(Vector3.forward * 16);
            }
            else if(decelerating)
            {
                rb.AddRelativeForce(Vector3.back * 15);
            }

            if (isTurningRight)
            {
                transform.Rotate(new Vector3(0, 1, 0), 3);
                if (wheel.localRotation.z > -0.5)
                {
                    wheel.Rotate(new Vector3(0, 0, -1), 2);
                }
            }
            else if (isTurningLeft)
            {
                if (wheel.localRotation.z < 0.5)
                {
                    wheel.Rotate(new Vector3(0, 0, 1), 2);
                }
                transform.Rotate(new Vector3(0, 1, 0), -3);
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

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Accelerate();
        }
        if (Input.GetKey(KeyCode.S))
        {
            Decelerate();
        }
        if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
    }

    public void Accelerate()
    {
        print(rb.gameObject.GetInstanceID());
        accelerating = true;
        //rb.AddRelativeForce(Vector3.forward * 16);       
    }
    public void Decelerate()
    {
        decelerating = true;
        //rb.AddRelativeForce(Vector3.back * 15);
    }
    public void TurnRight()
    {
        isTurningRight = true;
        //transform.Rotate(new Vector3(0, 1, 0), 3);
        //if (wheel.localRotation.z > -0.5)
        //{
        //    wheel.Rotate(new Vector3(0, 0, -1), 2);
        //}
    }
    public void TurnLeft()
    {
        isTurningLeft = true;
        //if (wheel.localRotation.z < 0.5)
        //{
        //    wheel.Rotate(new Vector3(0, 0, 1), 2);
        //}
        //transform.Rotate(new Vector3(0, 1, 0), -3);
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
            dir = -dir.normalized;
            rb.AddForce(dir * 200);
            if(rb.velocity.y<1)
            rb.AddRelativeForce(Vector3.up * 50);
        }

    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy"))
    //    {
    //        Vector3 dir = collision.contacts[0].point - transform.position;
    //        dir = -dir.normalized;
    //        rb.AddForce(dir * 100);
    //    }

    //}
}
