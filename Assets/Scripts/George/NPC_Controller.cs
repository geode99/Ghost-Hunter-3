using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth;
    public int panicMultiplier = 1;
    public bool JustSpawned = true;
    public BoxCollider2D BC2d;


    public Node currentNode;
    public List<Node> path = new List<Node>();

    public enum StateMachine
    {
        Patrol,
        Engage,
        Evade
    }
    
    public StateMachine currentState;

    public GridMovement player;
    public float speed = 3f;

    private void Start()
    {
        BC2d = GetComponent<BoxCollider2D>();
        curHealth = maxHealth;
        player = FindAnyObjectByType<GridMovement>();
    }

    private void Update()
    {
        if (JustSpawned && Vector2.Distance(transform.position, player.gameObject.transform.position) < 5)
        {

            currentNode = FindAnyObjectByType<Node>();
            gameObject.transform.position = currentNode.transform.position;
        }
        else {
            BC2d.enabled = true;
            JustSpawned = false;
        }
        switch (currentState)
        {
            case StateMachine.Patrol:
                Patrol();
                break;
            case StateMachine.Engage: 
                Engage(); 
                break;
            case StateMachine.Evade: 
                Evade(); 
                break;
        }

        bool playerSeen = Vector2.Distance(transform.position, player.transform.position) < 15.0f;

        if(!playerSeen && currentState != StateMachine.Patrol && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Patrol;
            path.Clear();
        }else if(playerSeen && currentState != StateMachine.Engage && curHealth > (maxHealth * 20) / 100)
        {
            currentState = StateMachine.Engage;
            path.Clear();
        }else if(currentState != StateMachine.Evade && curHealth <= (maxHealth * 20) / 100)
        {
            panicMultiplier = 2;
            currentState = StateMachine.Evade;
            path.Clear();
        }

        CreatePath();
    }

    void Patrol()
    {
        if(path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.AllNodes()[Random.Range(0, AStarManager.instance.AllNodes().Length)]);
        }
    }

    void Engage()
    {
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindNearestNode(player.transform.position));
        }
    }

    void Evade()
    {
        if (path.Count == 0)
        {
            path = AStarManager.instance.GeneratePath(currentNode, AStarManager.instance.FindFurthestNode(player.transform.position));
        }
    }

    public void CreatePath()
    {
        if (path.Count > 0)
        {
            int x = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(path[x].transform.position.x, path[x].transform.position.y, -2), (speed * panicMultiplier) * Time.deltaTime);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }
    }
}
