using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour, Car
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
            isSuicidal = true;
        }
        float distance = Vector3.Distance(transform.position, center.position);
        if (distance > 30)
        {
            GetEliminated();
        }
        else if (distance > 13 && !isSuicidal)
        {
            LookTowards(center);
            AccelerateForwards();
        }
        else if (distance < 3)
        {
            LookAwayFrom(center);
            AccelerateForwards();
        }
        else
        {
            
            if (target && isAggresive)
            {
                FindTarget();
                LookTowards(target);
            }
            else if(target && !isAggresive)
            {
                LookAwayFrom(target);
            }
            AccelerateForwards();
        }
    }

    private void GetEliminated()
    {
        gameManager.DecreaseNumOfCars(gameObject);
        Destroy(gameObject);
    }

    private void FindTarget()
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

    void AccelerateForwards()
    {
        velocityZ = transform.InverseTransformDirection(rb.velocity).z;
        velocityX = transform.InverseTransformDirection(rb.velocity).x;
        if (velocityZ < -2)
        {
            rb.AddRelativeForce(Vector3.forward  * 15);
        }else if(velocityZ < 2)
        {
            rb.AddRelativeForce(Vector3.forward * 15);
        }else if(velocityZ > 5)
        {
            AccelerateBackwards();
        }

        if(velocityX > 5)
        {
            rb.AddRelativeForce(Vector3.left *  15);
        }
        else if (rb.velocity.x < -5)
        {
            rb.AddRelativeForce(Vector3.right *  15);
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
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);
    }

    void LookAwayFrom(Transform target)
    {
        var lookPos = transform.position - target.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.Equals(gameObject) && ((collision.gameObject.CompareTag("Enemy")) || (collision.gameObject.CompareTag("Player"))))
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir * 300);
            rb.AddRelativeForce(Vector3.up * 30);
        }
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!collision.gameObject.Equals(gameObject) && ((collision.gameObject.CompareTag("Enemy")) || (collision.gameObject.CompareTag("Player"))))
    //    {
    //        Vector3 dir = collision.contacts[0].point - transform.position;
    //        dir = -dir.normalized;
    //        rb.AddForce(dir * 100);
    //    }

    //}
}
