using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    Rigidbody rb;
    public Transform wheel;
    public Transform target;
    public LayerMask layerMask;
    public GameObject currentHitObject;
    private Vector3 origin;
    private Vector3 direction;
    public float sphereRadius;
    public float maxDistance;
    private float currentHitDistance;
    public Transform center;
    public GameManager gameManager;

    public float velocityZ;
    public float velocityX;
    public bool isSuicidal = false;
    public float suicideTime = 0;
    public bool isAggresive = true;
    public float stateChangeTime = 0;
    public float stateChangeCooldown = 5;
    public int collisionPushForce = 300;
    void Start()
    {      

        rb = GetComponent<Rigidbody>();
        center = GameObject.FindGameObjectWithTag("Center").transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        stateChangeTime += Time.deltaTime;
        suicideTime += Time.deltaTime;

        if (stateChangeTime > stateChangeCooldown)
        {
            isAggresive = !isAggresive;
        }
        if (suicideTime > 30f) 
        {
            isSuicidal = true;      // prevent stuck game for demo
        }


        float distance = Vector3.Distance(transform.position, center.position);
        if (distance > 30)      //fallen from edge
        {
            GetEliminated();
        }
        else if (distance > 13 && !isSuicidal) //Get away from edges
        {
            LookTowards(center);
            Accelerate();
        }
        else if (distance < 3)      //Don't stick to center
        {
            LookAwayFrom(center);
            Accelerate();
        }
        else
        {           
            if (target && isAggresive) //attack mode
            {
                FindTarget();
                LookTowards(target);
            }
            else if(target && !isAggresive) // dodge mode
            {
                LookAwayFrom(target);
            }
            Accelerate();
        }
    }

    private void GetEliminated()
    {
        gameManager.DecreaseNumOfCars(gameObject);
        Destroy(gameObject);
    }

    private void FindTarget()       //find nearest car at front
    {
        RaycastHit hit;
        origin = transform.position;
        direction = transform.forward;
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            target = hit.transform;
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
        }
    }

    void Accelerate()
    {
        velocityZ = transform.InverseTransformDirection(rb.velocity).z; //local z velocity
        velocityX = transform.InverseTransformDirection(rb.velocity).x; //local x velocity

        if(velocityZ < 2) // speed limit
        {
            rb.AddRelativeForce(Vector3.forward * 15);
        }

    }
    void AccelerateBackwards()
    {
            rb.AddRelativeForce(Vector3.back * 15);
    }
    void TurnRight()
    {
        transform.Rotate(new Vector3(0, 1, 0), 3);
        if (wheel.localRotation.z > -0.5)
        {
            wheel.Rotate(new Vector3(0, 0, -1), 2);
        }
    }
    void TurnLeft()
    {
        if (wheel.localRotation.z < 0.5)
        {
            wheel.Rotate(new Vector3(0, 0, 1), 2);
        }
        transform.Rotate(new Vector3(0, 1, 0), -3);
    }
   void LookTowards(Transform target)
    {
        var lookPos = target.position - transform.position;
        RotateTo(lookPos);
    }

    void LookAwayFrom(Transform target)
    {
        var lookPos = transform.position - target.position;
        RotateTo(lookPos);
    }

    private void RotateTo(Vector3 lookPos)
    {
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.Equals(gameObject) && ((collision.gameObject.CompareTag("Enemy")) || (collision.gameObject.CompareTag("Player"))))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir * collisionPushForce);  
            rb.AddRelativeForce(Vector3.up * 30);
        }
    }
}
