using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LevelDownloader : MonoBehaviour
{
    //the string used to change the level once it's downloaded
    public string LevelName;
    //the URL of where the file is to download on DigitalOcean
    public string LevelURL;
    //the text saying the word "downloading" while the level is being downloaded
    public GameObject downloadText;
    //the download bar showing how much of the level has been downloaded
    public GameObject DownloadSlider;
    //stores the image of the level button
    private GuiFadeInFadeOut thisImage;
    //stores the images for the download slider
    private GuiFadeInFadeOut[] sliderImages;
    //stores the scene changer script
    [SerializeField] private SceneChanger levelChanger;
    //checks if the level is downloaded
    private bool isDownloaded = false;
    //when true shows the progress of the download
    public bool showDownloadProgress = false;
    //stores the dependencies for the level
    AsyncOperationHandle downloadDependencies;
    
    void Start()
    {
        
        levelChanger = GameObject.FindGameObjectWithTag("Options").GetComponent<SceneChanger>();
        thisImage = transform.GetComponent<GuiFadeInFadeOut>();
        DownloadSlider.SetActive(false);
        sliderImages = DownloadSlider.GetComponentsInChildren<GuiFadeInFadeOut>();
        //makes sure the download slider images aren't shown on startup
        SliderImagesFadeOut();
        //checks if the level was already downloaded before hand
        StartCoroutine(CheckIfDownloaded(true)); 
        
    }

    
    void Update()
    {
        //shows download progress while the ;evel is downloading

        if (showDownloadProgress)
        {
            DownloadSlider.GetComponent<Slider>().value = downloadDependencies.PercentComplete;
            if (downloadDependencies.PercentComplete > 0.99f)
            {
                isDownloaded = true;
                DownloadFinished();
                
            }
        }
    
    }

    //fades out the download slider images
    public void SliderImagesFadeOut()
    {
        for (int i = 0; i < sliderImages.Length; i++)
        {
            sliderImages[i].FadeOut();
        }
    }
    //fades in the download slider images
    public void SliderImagesFadeIn()
    {
        for (int i = 0; i < sliderImages.Length; i++)
        {
            sliderImages[i].FadeIn();
        }
    }


    //makes changes when the download is complete
    private void DownloadFinished()
    {
        //changes level button
        thisImage.Downloaded();
        thisImage.ColorChange(thisImage.getColor().a);
        //removes "downloading" text
        downloadText.SetActive(false);
        //stops showing download progress
        SliderImagesFadeOut();
        showDownloadProgress = false;
    }

    IEnumerator CheckIfDownloaded(bool initialCheck)
    {
        if (initialCheck)
        {
            //Gets the data on the download size
            AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(LevelURL);
            yield return getDownloadSize;


            //checks if the file has not been downloaded
            if (getDownloadSize.Result > 0)
            {
                isDownloaded = false;

            }

            //if the filke has been downloaded
            else
            {
                isDownloaded = true;
            }
        }

        //shows the download text
        if (!isDownloaded)
        {
            downloadText.GetComponent<GuiFadeInFadeOut>().enabled = true;
        }

        //removes download text and the download slider
        else
        {

            downloadText.GetComponent<GuiFadeInFadeOut>().enabled = false;

            SliderImagesFadeOut();

            thisImage.Downloaded();
        }
    }


    public void ButtonClicked()
    {
        //changes the level
        if(isDownloaded)
        {
            levelChanger.changeScene(LevelName);
        }

        //starts the download
        else
        {
            DownloadSlider.SetActive(true);
            StartCoroutine(DownloadLevel());
        }
    }

    IEnumerator DownloadLevel()
    {
        //Check the download size
        AsyncOperationHandle<long> getDownloadSize = Addressables.GetDownloadSizeAsync(LevelURL);
        yield return getDownloadSize;

        //shows the download slider
        SliderImagesFadeIn();

        //If the download size is greater than 0, download the dependencies.

        if (getDownloadSize.Result > 0)
        {
            showDownloadProgress = true;
            //shows downloading text
            downloadText.GetComponent<TextMeshProUGUI>().text = "Downloading";
            //starts downloading the level
            downloadDependencies = Addressables.DownloadDependenciesAsync(LevelURL);
            yield return downloadDependencies;
        }
        
        
    }

    //removes level from device
    public void deleteLevel()
    {
        //removes level
        Addressables.ClearDependencyCacheAsync(LevelURL);
        //changes the text back
        downloadText.SetActive(true);
        downloadText.GetComponent<TextMeshProUGUI>().text = "Download";
        
    }
}
