using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class SceneChanger : MonoBehaviour
{
    // Stores the current scene
    private Scene scene;
    //checks if it is the apple version of the app
    public bool isAppleVersion;
    //turns on and off test mode for ads
    public bool testMode;
    //app store IDs for the Unity ads to load
    private string GooglePlayID = "";
    private string AppStoreID = "";

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        StartCoroutine(CheckPlatform());
        testMode = false;
       
    }

   
    private IEnumerator CheckPlatform()
    {
        yield return new WaitForSeconds(1.0f);
        //initialises the ad so it is ready to play the moment the user changes the scene
        if (isAppleVersion)
        {
            Advertisement.Initialize(AppStoreID, testMode);

        }

        else
        {
            Advertisement.Initialize(GooglePlayID, testMode);     
            
        }

    }


    public void changeScene(string s)
    {
        
        if (scene.name != s)
        {
            //change the global variable to the scene you want to load to
            PlayerPrefs.SetString("SceneToLoad", s);
            //change to the transition scene while other scene loads
            SceneManager.LoadSceneAsync("TransitionScene");
            
        }
    }


}
