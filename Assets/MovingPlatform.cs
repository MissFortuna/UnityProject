using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Vector3 MoveBy;
    public float timeToWait = 1; 
    public float speed = 1;

    private Vector3 pointA;
    private Vector3 pointB;

    private bool goingToA;
    private Vector3 destination; 

    void Start()
    {
        this.pointA = this.transform.position;
        this.pointB = this.pointA + MoveBy;

        goingToA = false;
        destination = pointB - pointA;
    }

    void FixedUpdate()
    {

        if (timeToWait <= 0)
        {
            Vector3 myPos = this.transform.position;
            Vector3 target;

            if (goingToA)
            {
                target = this.pointA;
            }
            else
            {
                target = this.pointB;
            }

            // check if platform has arrived to the current target. If yes, then set the wait time and change the current target
            if (isArrived(myPos, target))
            {
                timeToWait = 2;
                goingToA = goingToA ? false : true;

                if (goingToA)
                {
                    destination = this.pointA - this.pointB;
                }
                else
                {
                    destination = this.pointB - this.pointA;
                }
            }

            // move the platforms
            transform.Translate(destination * speed * Time.deltaTime);
        }
        else
        {
            // decrease wait time while platform stands in one place
            timeToWait -= Time.deltaTime;
        }
    }

    bool isArrived(Vector3 pos, Vector3 target)
    {
        pos.z = 0;
        target.z = 0;
        return Vector3.Distance(pos, target) < 0.02f;
    }
}