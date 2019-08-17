using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Minion : MonoBehaviour
{
    private Tower targetTower = null;
    private Vector3 targetPosition;
    private BaseManager theBase;

    private NavMeshAgent agent;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        theBase = FindObjectOfType<BaseManager>();
    }

    private void Start()
    {
        agent.Warp(transform.position);
    }

    private void Update()
    {
        var path = new List<Vector2Int>();
        
        if (!targetTower)
        {
            theBase.pathfinder.FindPath(path, theBase.GetTowerPos(transform.position));
            for(var i = path.Count - 1; i >= 0; i--)
            {
                var node = path[i];
                if (i - 1 < 0)
                {
                    targetPosition = theBase.GetPosition(node.x, node.y);
                    break;
                }
                
                var nextNode = path[i - 1];

                var tower = theBase.GetTower(nextNode);
                if (!tower)
                    continue;

                if (!tower.isActiveAndEnabled || !(tower.Health > 0))
                    continue;

                targetTower = tower;
                targetPosition = tower.transform.position;//new Vector3(node.x, 0, node.y);
                break;
            }
        }

        if ((transform.position - targetPosition).magnitude > 1) // if more than 1 unit away, walk to it
        {
            //print(targetPosition);
            agent.SetDestination(targetPosition);
        }
        else if(targetTower)
        {
            if (targetTower.Health > 0)
            {
                targetTower.Health -= Time.deltaTime * 10;
            }
            else
            {
                if (targetTower.name == "BaseTower")
                {
                    //Debug.Log("Base defeated");
                    FindObjectOfType<MinionPlacer>().EndRaid(true);
                }
                else
                {
                    targetTower.gameObject.SetActive(false);
                    targetTower = null;
                }
            }
        }
    }
}
