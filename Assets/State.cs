using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class State : MonoBehaviour
{

    public FirstPersonController controller;

    public NavMeshAgent enemy;

    public GameObject PointOne;

    public GameObject PointTwo;

    public Vector3 Point1;

    public Vector3 Point2;

    public GameObject Player;

    public bool detectsPlayer=false;

    public Vector3 PlayerPos;

    public bool Seen = false;

    public enum CURRENTSTATE
    { PATROL, CHASE, ATTACK, SEARCH, RETREAT };

    public enum PATROLGOAL
    { goalOne, goalTwo };

    public float speed;

    public float timer=0;

    CURRENTSTATE CurrentState = CURRENTSTATE.PATROL;

    PATROLGOAL Goal = PATROLGOAL.goalOne;

    


    // Start is called before the first frame update
    void Start()
    {
        Point1 = PointOne.transform.position;

        Point2 = PointTwo.transform.position;

    }

// Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (CurrentState==CURRENTSTATE.PATROL)
        {
            runPatrol();
        }

        else if (CurrentState==CURRENTSTATE.CHASE)
        {
            runChase();
        }

        else if (CurrentState==CURRENTSTATE.ATTACK)
        {
            runAttack();
        }

        else if (CurrentState==CURRENTSTATE.SEARCH)
        {
            runSearch();
        }

        else if (CurrentState==CURRENTSTATE.RETREAT)
        {
            runRetreat();
        }

    }


    //travelling between two points
    public void runPatrol()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;

        if (Seen)
        {
            CurrentState = CURRENTSTATE.CHASE;
        }

        if (Goal==PATROLGOAL.goalOne)
        {
            enemy.SetDestination(Point1);
        }

        if (Vector3.Distance(Point1, enemy.transform.position)<=0.5)
        {
            Goal = PATROLGOAL.goalTwo;
        }

        if (Goal == PATROLGOAL.goalTwo)
        {
            enemy.SetDestination(Point2);
        }

        if (Vector3.Distance(Point2, enemy.transform.position)<=0.5)
        {
            Goal = PATROLGOAL.goalOne;
        }

        if (detectsPlayer)
        {
            CurrentState = CURRENTSTATE.CHASE;
        }

        if (Vector3.Distance(Player.transform.position, enemy.transform.position) <= 2)
        {
            detectsPlayer = true;
        }

        //controller._speed >= 1f &&
        //checking if the player is moving, I had to make _speed from FirstPersonController public
        if (controller.currentSpeed>=1.0f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 5)
        {
            detectsPlayer = true;
        }

        if (controller.currentSpeed >= 4.5f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 8)
        {
            detectsPlayer = true;
        }
    }

    //travelling after player
    public void runChase()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;

        enemy.SetDestination(Player.transform.position);

        if (Vector3.Distance(Player.transform.position, enemy.transform.position) >= 10)
        {
            CurrentState = CURRENTSTATE.SEARCH;

            //add player's last known location here
        }

        if (Vector3.Distance(Player.transform.position, enemy.transform.position) <= 2)
        {
            CurrentState=CURRENTSTATE.ATTACK;

            timer = 3;
        }
    }

    //stopping and attacking player
    public void runAttack()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;

        if (timer <= 0)
        {
            CurrentState=CURRENTSTATE.CHASE;
        }

    }

    //going to last known player location and spinning for a while
    public void runSearch()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.yellow;


        if (Seen)
        {
            CurrentState = CURRENTSTATE.CHASE;
        }

        if (Vector3.Distance(Player.transform.position, enemy.transform.position) <= 2)
        {
            detectsPlayer = true;
        }

        if (controller.currentSpeed >= 1.0f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 5)
        {
            detectsPlayer = true;
        }

        if (controller.currentSpeed >= 4.5f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 8)
        {
            detectsPlayer = true;
        }
    }

    //returning to nearest patrol point
    public void runRetreat()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;

        enemy.SetDestination(Point1);
        
        if (Vector3.Distance(Point1, enemy.transform.position) <= 0.5)
        {
            CurrentState=CURRENTSTATE.PATROL;
        }

        if (Seen)
        {
            CurrentState = CURRENTSTATE.CHASE;
        }

        if (Vector3.Distance(Player.transform.position, enemy.transform.position) <= 2)
        {
            detectsPlayer = true;
        }

        if (controller.currentSpeed >= 1.0f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 5)
        {
            detectsPlayer = true;
        }

        if (controller.currentSpeed >= 4.5f && Vector3.Distance(Player.transform.position, enemy.transform.position) <= 8)
        {
            detectsPlayer = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other == Player)
        {
            Seen = true;
        }
    }
}
