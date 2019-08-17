using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaidManager : Singleton<RaidManager>
{
    public string RaidId => raidId;

    private string raidId = "5d571d8a6544d7fde168f79c"; // ""

    public bool StartRaid()
    {
        if (raidId == "")
            return false;

        SceneManager.LoadScene("Raid");
        return true;
    }

    public void SetRaidTargetUser(string username, Action<bool> callback)
    {
        AuthenticationManager.Instance.GetUser(username, user =>
        {
            if (user == null)
            {
                callback(false);
                return;
            }

            raidId = user._id;
            print($"Raiding {user.username} ({user._id})");
            callback(true);
        });
    }
}
