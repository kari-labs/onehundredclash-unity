using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameGetter : MonoBehaviour
{
    private Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
        AuthenticationManager.Instance.GetApplicationName(s => text.text = s);
        //AuthenticationManager.Instance.Login("ianfabs", "password", s => Debug.Log(s));
    }
}