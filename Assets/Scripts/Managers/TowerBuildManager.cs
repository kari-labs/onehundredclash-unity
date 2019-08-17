using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    [HideInInspector]
    public TowerItem currenttlySelected = null;
    
    public BaseManager theBase;

    private void Start()
    {
        theBase.LoadBase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(theBase.Serialize());
        }
        
        var camera = Camera.main;
        if (camera == null)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!currenttlySelected)
        {
        } else if (currenttlySelected is DeleteTowerItem)
        {
            if (Physics.Raycast(ray, out var hit, 100.0f, 1 << 9))
            {
                var tower = hit.collider.GetComponent<Tower>();
                if(!tower)
                    return;

                theBase.grid[tower.xPos, tower.zPos] = null;
                Destroy(tower.gameObject);
            }
        } else {
            if (Physics.Raycast(ray, out var hit, 100.0f, 1 << 8))
            {
                var loc = theBase.GetTowerPos(hit.point);
                var tower = Instantiate(currenttlySelected.towerPrefab);
                tower.name = currenttlySelected.towerPrefab.name;
                var success = theBase.PlaceTower(tower, loc.x, loc.y);
                if (!success)
                    Destroy(tower.gameObject);
            }
        }
    }
}
