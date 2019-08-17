using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GraphQL;
using UnityEngine;

public class User
{
    public string _id;
//    public Name name;
    public string username;
    public float xp;
    public int coins;
}

public class Base
{
    public string _id;
    public string data;
    public string owner;
    public string geo;
}

public class AuthenticationManager : Singleton<AuthenticationManager>
{
    [DllImport("__Internal")]
    private static extern string GetSavedJWT();
    
    [DllImport("__Internal")]
    public static extern void Redirect(string url);

    public User Myself;
    private string myToken = "";

    protected override void OnAwake()
    {
        var token = "";

        try
        {
            token = GetSavedJWT();
        }
        catch (EntryPointNotFoundException ex)
        {
            
        }

        if(token == "")
            token = PlayerPrefs.GetString("jwt");
        
        if (token != "")
        {
            myToken = token;
        }
    }
    
    private string getApplicationName =
        @"
            query {
                nameOfTheGame
            }
        ";
    public void GetApplicationName(Action<string> callback)
    {
        APIGraphQL.Query(getApplicationName, null, response => { callback(response.Get<string>("nameOfTheGame")); });
    }
    
    private string getMyBase =
        @"
            query($jwt: String!) {
              getBase(jwt: $jwt)
            }
        ";
    public void GetMyBase(Action<string> callback)
    {
        APIGraphQL.Query(getMyBase, new {jwt = myToken}, response => { callback(response.Get<string>("getBase")); });
    }
    
    private string getBase =
        @"
            query($id: String!) {
                getUserBase(_id: $id) {
                    _id
                    data
                    owner
                    geo
                }
            }
        ";
    public void GetBase(string id, Action<Base> callback)
    {
        APIGraphQL.Query(getBase, new {id = id}, response => { print(response.Raw); callback(response.Get<Base>("getUserBase")); });
    }
    
    private string updateBase =
        @"
            mutation ($jwt: String!, $data: String!){
                updateBase(jwt: $jwt, data: $data)
            }
        ";
    public void UpdateBase(string baseData, Action<bool> callback)
    {
        APIGraphQL.Query(updateBase, new {jwt = myToken, data = baseData}, response => { callback(response.Get<bool>("updateBase")); });
    }
    
    private string addCoins =
        @"
            mutation ($jwt: String!, $amount: Int!){
                addCoins(jwt: $jwt, amount: $amount)
            }
        ";
    public void AddCoins(int amount, Action<bool> callback)
    {
        APIGraphQL.Query(addCoins, new {jwt = myToken, amount = amount}, response => { callback(response.Get<bool>("addCoins")); });
    }
    
    private string getCurrentUser =
        @"
            query($jwt: String!) {
              getCurrentUser(jwt: $jwt) {
                    _id
                    username
                    xp
                    coins
                }
            }
        ";
    public void GetCurrentUser(Action<User> callback)
    {
        APIGraphQL.Query(getCurrentUser, new {jwt = myToken}, response =>
        {
            print(response.Raw);
            callback(response.Get<User>("getCurrentUser"));
        });
    }
    
    private string getNearbyUsers =
        @"
            query($jwt: String!) {
              getNearbyPlayers(jwt: $jwt) {
                    _id
                    username
                    xp
                    coins
                }
            }
        ";
    public void GetNearbyUsers(Action<List<User>> callback)
    {
        APIGraphQL.Query(getNearbyUsers, new {jwt = myToken}, response => { callback(response.Get<List<User>>("getNearbyPlayers")); });
    }
    
    private string getUser =
        @"
            query($username: String!) {
              getUser(username: $username) {
                    _id
                    username
                    xp
                    coins
                }
            }
        ";
    public void GetUser(string username, Action<User> callback)
    {
        APIGraphQL.Query(getUser, new {username = username}, response => { callback(response.Get<User>("getUser")); });
    }
        
    private string login =
        @"
            mutation($username: String!, $password: String!) {
              login(username: $username, password: $password)
            }
        ";
    public void Login(string username, string password, Action<string> callback)
    {
        APIGraphQL.Query(login, new {username = username, password = password}, response =>
        {
            var jwt = response.Get<string>("login");

            if (jwt != "Null")
            {
                myToken = jwt;

                PlayerPrefs.SetString("jwt", jwt);
                PlayerPrefs.Save();
            }

            callback(jwt);
        });
    }
}
