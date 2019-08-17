using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;

    private void Start()
    {
        print(welcomeText);
        print(AuthenticationManager.Instance.Myself);
        print(AuthenticationManager.Instance.Myself.username);
        welcomeText.text = $"Welcome, {AuthenticationManager.Instance.Myself.username}";
    }

    public void Logout()
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

    public void PlayGame()
    {
        SceneManager.LoadScene("BuildBase");
    }
}
