using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.Advertisements;

public class SceneTransition : MonoBehaviour
{
    

    public Slider loadingBar;
    public bool testing;
    public string testScene;
    private bool adShown;
    private string LevelURL;
    [SerializeField] private GameObject DownloadErrorImage;

    private void Awake()
    {
       
        adShown = false;
        //checks if it is testing fotr the editor
        if (!testing)
        {
            //gets the global variable for which scene to load and loads it dring the transition scene
            string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
            LoadingLevel(sceneToLoad);
        }

        else
        {
            LoadImages(testScene);
        }
    }


    //loads the images and starts the coroutine to load the level
    private void LoadingLevel(string s)
    {
        LevelURL = "Assets/Scenes/" + s + ".unity";
        LoadImages(s);
        StartCoroutine(LoadingProgress());
    }

    private IEnumerator LoadingProgress()
    {
        //starts loading the next scene
        var loadingProgress = Addressables.LoadSceneAsync(LevelURL, LoadSceneMode.Additive);

        //checks if there has been an error and the level didn;t download properly
        if(loadingProgress.OperationException != null)
        {
            
            yield return new WaitForSeconds(3.0f);

            //transitions back to the title screen
            SceneManager.LoadSceneAsync("Opening Menu");
            SceneManager.UnloadSceneAsync("TransitionScene");
        }

        else
        {
            //removes the download error message
            DownloadErrorImage.SetActive(false);
        }
       

        while(!loadingProgress.IsDone)
        {
            //shows how much of the level has been loaded
            loadingBar.value = loadingProgress.PercentComplete;
            //shows the ad
            if (Advertisement.IsReady())
            {
                if (adShown == false)
                {
                    Advertisement.Show("video");
                    adShown = true;
                }
            }
            yield return null;
        }
       
        //unloads the transition scene once the other scene is loaded
        SceneManager.UnloadSceneAsync("TransitionScene");
       
    }

    //shows the images for the level being loaded and rotates between them
    private void LoadImages(string s)
    {
        GameObject.FindGameObjectWithTag(s + " Images").GetComponent<Image>().enabled = true;
        GameObject.FindGameObjectWithTag(s + " Images").GetComponent<LoadingScreenImageSwitcher>().enabled = true;

    }
}
