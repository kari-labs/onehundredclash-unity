using UnityEngine;
using UnityEngine.UI;

public class RaidUI : MonoBehaviour
{
    public InputField usernameInput;

    public void Raid()
    {
        RaidManager.Instance.SetRaidTargetUser(usernameInput.text, succ =>
        {
            if (succ)
            {
                RaidManager.Instance.StartRaid();
            }
        });
    }
}