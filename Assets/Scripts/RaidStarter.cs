using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidStarter : MonoBehaviour
{
    private BaseManager theBase;
    public GameObject loading;
    public GameObject MinionPlacer;
    void Start()
    {
        loading.SetActive(true);
        theBase = FindObjectOfType<BaseManager>();
        AuthenticationManager.Instance.GetBase(RaidManager.Instance.RaidId, baseData =>
        {
            if(baseData != null)
                theBase.Deserialize(baseData.data);
            if(MinionPlacer)
                MinionPlacer.SetActive(true);
            if(loading)
                loading.SetActive(false);
        });
    }
}
