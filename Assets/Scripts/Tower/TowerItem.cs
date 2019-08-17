using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerItem : MonoBehaviour
{
    public bool selected;
    
    public Tower towerPrefab;
    public TowerBuildManager buildManager;
    
    public void Select()
    {
        buildManager.currenttlySelected = this;
        selected = true;
    }
}
