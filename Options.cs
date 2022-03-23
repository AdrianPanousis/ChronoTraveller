using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Options : MonoBehaviour
{
    private AudioSource sound;
    private int soundSwitch;

    private float compassOffset;

    //stores all the hitboxes that can be tapped on screen to show a description of each object
    private TouchableObject[] hitboxes;

    //stores data to change the highlight of text options in the menu
    private SelectionHighlight[] languageOptionTexts;
    private SelectionHighlight[] QualityTexts;

    //
    private LanguageBox[] languageBoxes;
    private Image soundButton;

    //stores the two sound symbols showing it on or muted
    public Sprite SoundOff;
    public Sprite SoundOn;

    public Tutorial TutorialSettings;
    public Button XButton;

    //stores the button information to change the quality
    public Button lowQuality;
    public Button MediumQuality;
    public Button HighQuality;

    //stores what the current scene is
    Scene scene;

    //stores the settings for post processing effects
    public PostProcessVolume postProcess;
    private AmbientOcclusion occl = null;
    private ColorGrading colorgrading = null;
    private DepthOfField dof = null;
    private MotionBlur mblur = null;

    //stores the setting for the current language
    private string currentLaguage;

    //stores the setting for the current quality
    private string qualityLevel;
    
    
    enum qualityLevels
    {
        Low,
        Medium,
        High
    }
    void Start()
    {
        //sets global variables to their defaults if they aren't already set
        PlayerPrefs.GetString("SceneToLoad", "Opening Menu");
        PlayerPrefs.GetInt("IntroTutorialSeen", 0);
        soundSwitch = PlayerPrefs.GetInt("SoundSwitch",1);
        compassOffset = PlayerPrefs.GetFloat("compassOffset", 0);
        currentLaguage = PlayerPrefs.GetString("Language", "English");
        qualityLevel = PlayerPrefs.GetString("Quality", "Medium");

        //stores the sound options
        sound = GameObject.FindObjectOfType<AudioSource>();
        //gathers all the hitboxes in the scene and stores them into the array
        hitboxes = GameObject.FindObjectsOfType<TouchableObject>();
        //grabs the sound button
        soundButton = GameObject.FindGameObjectWithTag("Sound Button").GetComponent<Image>();
        
        //gathers language buttons in the scene
        languageOptionTexts = new SelectionHighlight[5];
        languageOptionTexts = GameObject.FindGameObjectWithTag("Language Button Container").GetComponentsInChildren<SelectionHighlight>();

        //gathers quality buttons in the scene
        QualityTexts = new SelectionHighlight[3];
        QualityTexts = GameObject.FindGameObjectWithTag("Quality Button Container").GetComponentsInChildren<SelectionHighlight>();

        //grabs the scene name
        scene = SceneManager.GetActiveScene();
        languageBoxes = gameObject.GetComponentsInChildren<LanguageBox>();

        //gets the initial settings for each post processing effect and stores it into the variables
        postProcess.profile.TryGetSettings(out occl);
        postProcess.profile.TryGetSettings(out mblur);
        postProcess.profile.TryGetSettings(out dof);
        postProcess.profile.TryGetSettings(out colorgrading);

        //checks if the sound is on or off by default
        if (soundSwitch==1)
        {
            sound.volume = 1;
            soundButton.sprite = SoundOn;
        }

        else
        {
            sound.volume = 0;
            soundButton.sprite = SoundOff;
        }

        

        StartCoroutine(DelayTimer());
        



    }

   
    //removes the swipe menu from the button of the screen
    public void RemoveSwipeMenu()
    {
        XButton.onClick.Invoke();
    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.05f);
        
        //changes the language to the global setting
        changeLanguage(currentLaguage);
        
        if(scene.name == "Opening Menu")
        {
            changeHighlight();
        }

        //changes the quality setting to the global setting
        if (qualityLevel == "Low")
        {
            lowQuality.onClick.Invoke();
            
            //settings to change
            PostProcessEffectsOn(false);
        }

        //removes the shadow on the text to show it is not set to low
        else
        {
            lowQuality.transform.GetChild(0).GetComponent<Text>().enabled = false;
        }

        if (qualityLevel == "Medium")
        {
            MediumQuality.onClick.Invoke();
            
            //settings to change
            PostProcessEffectsOn(true);
        }

        //removes the shadow on the text to show it is not set to medium
        else
        {
            MediumQuality.transform.GetChild(0).GetComponent<Text>().enabled = false;
        }

        if (qualityLevel == "High")
        {
            HighQuality.onClick.Invoke();
           
            //settings to change
            PostProcessEffectsOn(true);
        }

        //removes the shadow on the text to show it is not set to high
        else
        {
            HighQuality.transform.GetChild(0).GetComponent<Text>().enabled = false;
        }

    }

    //changes the shadow text so only the setting turned on is visible for languages
    public void changeHighlight()
    {
        RemoveLangageShadows(PlayerPrefs.GetString("Language"));
    }

    public void RemoveLangageShadows(string exemption)
    {
        for(int i = 0;i<languageOptionTexts.Length;i++)
        {
            //removes the shadow of the options not chosen
            if (exemption != languageOptionTexts[i].theName)
            {
                languageOptionTexts[i].Unhighlighted();
                
            }

            //adds the shadow of the option chosen
            else
            {
                languageOptionTexts[i].Highlighted();
                //sets the option show the shadow by default when opening the menu
                languageOptionTexts[i].defaultHighlight = true;

            }

        }
    }

    //changes the shadow text so only the setting turned on is visible for quality settings
    public void RemoveQualityTextShadows(string exemptions)
    {
        
        for (int i = 0; i < QualityTexts.Length; i++)
        {
            //removes the shadow of the options not chosen
            if (exemptions != QualityTexts[i].theName)
            {
                QualityTexts[i].Unhighlighted();
                
            }

            //adds the shadow of the option chosen
            else
            {
                QualityTexts[i].Highlighted();   
            }
        }
    }

    //switches the sound on and off
    public void ChangeSound()
    {
        //uses the absolute value so it only switches on and off with a 1 and 0
        soundSwitch = Mathf.Abs(soundSwitch - 1);

        //changes the sound
        if(soundSwitch == 1)
        {
            sound.volume = 1;
            
            soundButton.sprite = SoundOn;

        }

        else
        {
            sound.volume = 0;
            soundButton.sprite = SoundOff;
        }

        //changes the global variable for the sound switch
        PlayerPrefs.SetInt("SoundSwitch", soundSwitch);
    }

    //used to change the quality settings
    public void qualityChange(string exemptions)
    {
        //changes the global variable for the quality setting
        PlayerPrefs.SetString("Quality", exemptions);

        //converts the enum into a number
        qualityLevels l = (qualityLevels)System.Enum.Parse(typeof(qualityLevels), exemptions);
        int quality = (int)l;
        //changes the quality setting
        QualitySettings.SetQualityLevel(quality);
        
        //changes the post processing valus based on the quality setting
        if(exemptions == "Low")
        {
            PostProcessEffectsOn(false);
            dof.enabled.value = false;
        }

        if (exemptions == "Medium")
        {
            PostProcessEffectsOn(true);
            mblur.sampleCount.value = 5;
            dof.enabled.value = false;

        }

        if (exemptions == "High")
        {
            PostProcessEffectsOn(true);
            mblur.sampleCount.value = 10;
            dof.enabled.value = true;
        }

    }

    //turns on motion blue and ambient occlusion
    private void PostProcessEffectsOn(bool t)
    {
        mblur.enabled.value = t;
        occl.enabled.value = t;
    }

   
    public void setCompassOffset(float degrees)
    {
        compassOffset = degrees;
        PlayerPrefs.SetFloat("compassOffset", degrees);
        
        
    }

    public float getCompassOffset()
    {
        return PlayerPrefs.GetFloat("compassOffset");
    }

    //changes the language of all the text in the app
    public void changeLanguage(string l)
    {
        //changes the global variable for the language
        PlayerPrefs.SetString("Language", l);

        //changes the text of all the descriptions of each building
        for (int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i].changeLanguage(l);
        }

        //changes the language of all the menu text
        for (int i = 0; i < languageBoxes.Length; i++)
        {
            languageBoxes[i].ChangeText(l);
        }

        //changes the language of the tutorial
        TutorialSettings.changeLanguage(l);
    }

   
}
