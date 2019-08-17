using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MinionPlacer : MonoBehaviour
{
    public Minion selectedMinion;

    public List<Minion> minions;

    public GameObject victoryScreen;
    public GameObject defeatScreen;
    
    public TextMeshProUGUI countdown;
    public float timer = 100;

    public bool raidActive = true;

    private void OnEnable()
    {
        var theBase = FindObjectOfType<BaseManager>();
        var okayRaid = false;

        for (var x = 0; x < theBase.grid.GetLength(0); x++)
        {
            for (var z = 0; z < theBase.grid.GetLength(1); z++)
            {
                var tower = theBase.grid[x, z];
                
                if (tower && tower.name == "BaseTower")
                {
                    print(tower.name);
                    okayRaid = true;
                }
            }
        }
        print("okayraid: " + okayRaid);
        if (!okayRaid)
            SceneManager.LoadScene("BuildBase");
    }

    private void Update()
    {
        timer = Mathf.Clamp(timer, 0, 100);
        countdown.text = ((int) Math.Round(timer)).ToString();
        
        if (timer <= 0 && raidActive)
        {
            EndRaid(false);
        }
        else if (raidActive)
        {
            timer -= Time.deltaTime;
            
            var camera = Camera.main;
            if (camera == null)
                return;
            if (!Input.GetMouseButtonDown(0))
                return;
            var ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit, 100.0f, 1 << 10))
            {
                var minion = Instantiate(selectedMinion);
                minions.Add(minion);
                minion.name = selectedMinion.name;
                minion.transform.position = hit.point;
                timer -= 10;
            }
        }
    }

    public void KillMinions()
    {
        foreach (var minion in minions)
        {
            if (!minion) continue;
            
            var agent = minion.GetComponent<NavMeshAgent>();
            if(agent)
                Destroy(agent);
                
            Destroy(minion);
        }
    }
    
    public void EndRaid(bool victory)
    {
        KillMinions();
        raidActive = false;

        int coinReward = 1;
        
        if (victory)
        {
            victoryScreen.SetActive(true);
            coinReward = 6;
        }
        else
        {
            defeatScreen.SetActive(true);
        }

        AuthenticationManager.Instance.AddCoins(6, b => { });
    }

    public void BackToBase()
    {
        SceneManager.LoadScene("BuildBase");
    }
}
