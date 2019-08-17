using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    void Start()
    {
        AuthenticationManager.Instance.GetCurrentUser(user =>
        {
            AuthenticationManager.Instance.Myself = user;
            coinText.text = $"{user.coins} coins";
        });
    }
}
