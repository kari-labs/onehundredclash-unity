using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteTowerItem : TowerItem
{
    public void Select()
    {
        if(buildManager.currenttlySelected)
            buildManager.currenttlySelected.GetComponent<Image>().color = Color.white;
        buildManager.currenttlySelected = this;
        GetComponent<Image>().color = new Color(99f/255, 99f/255, 99f/255);
        selected = true;
    }
}
