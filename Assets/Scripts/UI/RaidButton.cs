using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaidButton : MonoBehaviour
{
    public User user;
    public Text userText;

    public void SetUser(User user)
    {
        this.user = user;
        userText.text = user.username;
    }
    
    public void Raid()
    {
        RaidManager.Instance.SetRaidTargetUser(user.username, b =>
        {
            if (b)
            {
                RaidManager.Instance.StartRaid();
            }
        });
    }
}
