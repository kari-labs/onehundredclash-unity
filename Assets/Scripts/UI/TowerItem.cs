using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerItem : MonoBehaviour
{
    public bool selected;
    
    public Tower towerPrefab;
    public TowerBuildManager buildManager;
    
    public void Select()
    {
        if(buildManager.currenttlySelected)
            buildManager.currenttlySelected.GetComponent<Image>().color = Color.white;
        buildManager.currenttlySelected = this;
        GetComponent<Image>().color = new Color(99f/255, 99f/255, 99f/255);
        selected = true;
    }
}
