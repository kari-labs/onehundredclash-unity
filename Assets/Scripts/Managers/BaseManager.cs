using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public Tower[] towers;
    
    [HideInInspector]
    public BaseAStar pathfinder;
    
    public Tower[,] grid = new Tower[10, 10];
    public Vector2 gridOffset;
    public float gridScale;

    public GameObject LoadingIndicator;
    
    private void Awake()
    {
        pathfinder = new BaseAStar(this);
    }

    public void LoadBase()
    {
        ClearBase();
        LoadingIndicator.SetActive(true);
        AuthenticationManager.Instance.GetMyBase(s =>
        {
            Deserialize(s);
            LoadingIndicator.SetActive(false);
        });
    }

    public void ClearBase()
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var z = 0; z < grid.GetLength(1); z++)
            {
                var tower = grid[x, z];
                if(tower)
                    Destroy(tower.gameObject);
                grid[x, z] = null;
            }
        }
    }
    
    public void SaveBase()
    {
        LoadingIndicator.SetActive(true);
        AuthenticationManager.Instance.UpdateBase(Serialize(), b => { LoadingIndicator.SetActive(false); });
    }
    
    public Vector2Int GetTowerPos(Vector3 hitPos)
    {
        var pos = new Vector2Int();
        pos.x = (int) Math.Round((hitPos.x - gridOffset.x) * gridScale);
        pos.y = (int) Math.Round((hitPos.z - gridOffset.y) * gridScale);
        return pos;
    }

    public Tower GetTower(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= grid.GetLength(0) || pos.y >= grid.GetLength(1))
            return null;
        
        return grid[pos.x, pos.y];
    }
    
    public bool PlaceTower(Tower tower, int x, int z)
    {
        var existingTower = GetTower(new Vector2Int(x, z));

        if (existingTower)
            return false;
        
        tower.transform.parent = transform;
        tower.transform.localPosition = GetPosition(x, z);
        tower.xPos = x;
        tower.zPos = z;
        grid[x, z] = tower;

        return true;
    }

    public Tower MoveTower(int fromX, int fromZ, int toX, int toZ)
    {
        var tower = grid[fromX, fromZ];
        return tower != null ? MoveTower(tower, toX, toZ) : null;
    }

    public Tower MoveTower(Tower tower, int toX, int toZ)
    {
        grid[tower.xPos, tower.zPos] = null;
        PlaceTower(tower, toX, toZ);
        return tower;
    }
    
    public Vector3 GetPosition(int x, int z)
    {
        return new Vector3(x * gridScale + gridOffset.x, 0, z * gridScale + gridOffset.y);
    }
    
    private void OnDrawGizmosSelected()
    {
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var z = 0; z < grid.GetLength(1); z++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(transform.position + GetPosition(x, z), 0.1f);
            }
        }
    }

    public void Deserialize(string code)
    {
        var rows = code.Split(';');
        for (var x = 0; x < rows.Length; x++)
        {
            var row = rows[x];
            var cols = row.Split(',');
            for (var z = 0; z < cols.Length; z++)
            {
                var item = cols[z];

                Tower prefab = null;
                foreach (var t in towers)
                {
                    if (t.name == item)
                    {
                        prefab = t;
                    }
                }
                
                if(!prefab)
                    continue;
                
                
                var tower = Instantiate(prefab);
                tower.name = prefab.name;
                var success = PlaceTower(tower, x, z);
                if(!success)
                    Destroy(tower.gameObject);
                
                //Debug.Log($"[{x}, {z}]: {item}");
            }
        }
    }
    
    public string Serialize()
    {
        var sb = new StringBuilder();
        
        for (var x = 0; x < grid.GetLength(0); x++)
        {
            for (var z = 0; z < grid.GetLength(1); z++)
            {
                var tower = grid[x, z];
                if (tower)
                    sb.Append(tower.name);
                
                if(z < grid.GetLength(1) - 1)
                    sb.Append(",");
            }
            if(x < grid.GetLength(0) - 1)
                sb.Append(";");
        }

        return sb.ToString();
    }
}
