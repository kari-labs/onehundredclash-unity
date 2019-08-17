
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseAStar : AStar<Vector2Int>
{
    private BaseManager theBase;
    
    public BaseAStar(BaseManager theBase)
    {
        this.theBase = theBase;
    }
    
    protected override void Neighbors(Vector2Int p, List<Vector2Int> neighbors) {
        neighbors.Add(p + new Vector2Int(1, 0));
        neighbors.Add(p + new Vector2Int(-1, 0));
        neighbors.Add(p + new Vector2Int(0, 1));
        neighbors.Add(p + new Vector2Int(0, -1));
    }

    protected override float Cost(Vector2Int p1, Vector2Int p2)
    {
        var tower = theBase.GetTower(p2);
        
        //if (p2.x < 0 || p2.y < 0 || p2.x >= theBase.grid.GetLength(0) || p2.y >= theBase.grid.GetLength(1))
        //    tower = null;
        //else
        //    tower = theBase.grid[p2.x, p2.y];
        if (tower == null)
            return 1;

        return tower.Health;
    }

    protected override float Heuristic(Vector2Int p)
    {
        var nearestGoal = new Vector2Int(Int32.MaxValue, Int32.MaxValue);
        var nearestDistance = float.MaxValue;
        
        for (var x = 0; x < theBase.grid.GetLength(0); x++)
        {
            for (var z = 0; z < theBase.grid.GetLength(1); z++)
            {
                var tower = theBase.grid[x, z];
                if (!tower)
                    continue;
                if(tower.name != "BaseTower")
                    continue;
                var towerPos = new Vector2Int(x, z);
                var distance = (p - towerPos).magnitude;
                if (distance >= nearestDistance)
                    continue;
                nearestGoal = towerPos;
                nearestDistance = distance;
            }
        }

        return nearestDistance;
        /*
        Tower tower;
        
        if (p.x < 0 || p.y < 0 || p.x > theBase.grid.GetLength(0) || p.y > theBase.grid.GetLength(1))
            tower = null;
        else
            tower = theBase.grid[p.x, p.y];
        
        if (tower && tower.Health > 0 && tower.name == "BaseTower")
            return 0f; // BaseTowers are the goal
        
        return 1f; // todo: this is a vast underestimate and will cause a lot of extra checks */
    }
}