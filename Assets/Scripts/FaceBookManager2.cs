using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FaceBookManager2 : MonoBehaviour
{

    public Text friendsText;
    private void Awake()
    {
        if(!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.Log("Couldn't initialise Facebook.");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1f;
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }

     // Login/logout
     public void FacebookLogin()
    {
        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions);
    }

    public void FaceBookLogout()
    {
        FB.LogOut();
    }

    //facebook shares
    //change later
    public void FacebookShare()
    {
        FB.ShareLink(new System.Uri("http://resocoder.com"), "Check it out", "good nice",
            new System.Uri("http://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
    }

    //fb invites
    public void FacebookGameRequest()
    {
        FB.AppRequest(
    "Here is a free gift!",
    null,
    new List<object>() { "app_users" },
    null, null, null, null,
    delegate (IAppRequestResult result) {
        Debug.Log(result.RawResult);
    }
);
    }

    public void InviteToPlayGame()
    {
        FB.AppRequest(
    "Come play this great game!",
    null, null, null, null, null, null,
    delegate (IAppRequestResult result) {
        Debug.Log(result.RawResult);
    }
);
    }

    public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
         {
             var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
             var friendsList = (List<object>)dictionary["data"];
             friendsText.text = string.Empty;

             foreach (var dict in friendsList)
             {
                 Debug.Log("friends is " + ((Dictionary<string, object>)dict)["name"]);
                 friendsText.text += ((Dictionary<string, object>)dict)["name"] +"\n";
             }
         });
    }
}
