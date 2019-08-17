using UnityEngine;
using UnityEngine.UI;

public class RaidUI : MonoBehaviour
{
    public RaidButton raidButtonPrefab;
    public GameObject raidWindow;
    public GameObject raidViewport;
    
    public GameObject raidPlayerWindow;

    public void ClearDisplayWindow()
    {
        foreach (Transform child in raidViewport.transform) {
            Destroy(child.gameObject);
        }
        
        raidWindow.SetActive(true);
    }
    
    public void RaidNearby()
    {
        ClearDisplayWindow();
        AuthenticationManager.Instance.GetNearbyUsers(list =>
        {
            foreach (var user in list)
            {
                var button = Instantiate(raidButtonPrefab);
                button.SetUser(user);
                button.transform.parent = raidViewport.transform;
            }
        });
    }

    public void RaidPlayer()
    {
        raidPlayerWindow.SetActive(true);
    }
    
    public void RaidPlayerInput(InputField input)
    {
        RaidManager.Instance.SetRaidTargetUser(input.text, succ =>
        {
            if (succ)
            {
                RaidManager.Instance.StartRaid();
            }
        });
    }

    public void CloseRaidWindow()
    {
        raidPlayerWindow.SetActive(false);
        raidWindow.SetActive(false);
    }
}