using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameGetter : MonoBehaviour
{
    private Text text;
    
    void Start()
    {
        text = GetComponent<Text>();
        AuthenticationManager.Instance.GetApplicationName(s => text.text = s);
        AuthenticationManager.Instance.GetCurrentUser(user =>
        {
            if (user == null)
            {
                try
                {
                    AuthenticationManager.Redirect("https://hundred.kari.dev/logout");
                }
                catch (EntryPointNotFoundException)
                {
                    Debug.Log("Could not redirect");
                }
            }
            
            AuthenticationManager.Instance.Myself = user;
            SceneManager.LoadScene("Menu");
        });
    }
}