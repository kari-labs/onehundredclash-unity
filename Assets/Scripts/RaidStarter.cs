using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidStarter : MonoBehaviour
{
    private BaseManager theBase;
    public GameObject loading;
    void Start()
    {
        loading.SetActive(true);
        theBase = FindObjectOfType<BaseManager>();
        AuthenticationManager.Instance.GetBase(RaidManager.Instance.RaidId, baseData =>
        {
            theBase.Deserialize(baseData.data);
            loading.SetActive(false);
        });
    }
}
