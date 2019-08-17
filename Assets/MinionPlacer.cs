using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionPlacer : MonoBehaviour
{
    public Minion selectedMinion;

    private void Update()
    {
        var camera = Camera.main;
        if (camera == null)
            return;
        if (!Input.GetMouseButtonDown(0))
            return;
        var ray = camera.ScreenPointToRay(Input.mousePosition); 
        if (Physics.Raycast(ray, out var hit,100.0f, 1 << 10))
        {
            var minion = Instantiate(selectedMinion);
            minion.name = selectedMinion.name;
            minion.transform.position = hit.point;
        }
    }
}
