using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapMenuRemoval : MonoBehaviour
{
    // array that stores all UI parts to be faded in and out
    private GameObject[] mapMenuParts;
    private GuiFadeInFadeOut[] optionsMenuParts;
    private GuiFadeInFadeOut[] descriptionScrollMenu;
    private GuiFadeInFadeOut[] chooseAreaMenu;
    private GuiFadeInFadeOut[] compassOffsetMenu;
    private GuiFadeInFadeOut[] noCameraMenu;
    private GuiFadeInFadeOut[] tutorialMenu;
    private GuiFadeInFadeOut[] openingMenu;

    //stores all the buttons for each UI part
    private Button[] optionsButtonParts;
    private Button[] descriptionScrollButton;
    private Button[] chooseAreaButton;
    private Button[] compassOffsetButton;
    private Button[] noCameraMenuButton;
    private Button[] tutorialMenuButton;
    private Button[] openingMenuButton;

    public bool isOpeningMenu;

    //stores the button to be invoked to show the map menu
    public Button MapButton;
    //stores the background images for the menu
    public GuiFadeInFadeOut Background;
    public GuiFadeInFadeOut BackgroundExtra;
    //stores the button that is invoked to load the tutorial menu
    public Button TutorialButton;
    //stores the swipe menu to be invoked
    public SwipeMenu MenuSwipe;

    void Start()
    {
        //fills array with UI Parts
        mapMenuParts = GameObject.FindGameObjectsWithTag("MapMenu");
        GatherData("Options",out optionsMenuParts,out optionsButtonParts);
        GatherData("Scroll Mask", out descriptionScrollMenu, out descriptionScrollButton);
        GatherData("Choose Area", out chooseAreaMenu, out chooseAreaButton);
        GatherData("Compass Offset", out compassOffsetMenu, out compassOffsetButton);
        GatherData("No Camera Image", out noCameraMenu, out noCameraMenuButton);
        GatherData("Tutorial", out tutorialMenu, out tutorialMenuButton);

        //fills array with opening menu parts
        if(isOpeningMenu == true)
        {
            GatherData("Opening Menu", out openingMenu, out openingMenuButton);
        }

        //adds a slight delay so it executes properly
        if (SceneManager.GetActiveScene().name != "Opening Menu")
        {
            StartCoroutine(DelayTimer());
        }

    }

    //either loads up the tutorial the moment a level loads or the map menu loads right away if they have done the tutorial before
    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.1f);

        //checks if tutorial has been seen
        if(PlayerPrefs.GetInt("IntroTutorialSeen") == 1)
        {
            //show the map menu
            MapButton.onClick.Invoke();
        }

        //shows the tutorial
        else
        {
            //loads the menu background
            Background.FadeIn();
            BackgroundExtra.FadeIn();
            //shows the tutorial
            TutorialButton.onClick.Invoke();
        }
        
        //opens the slide menu
        MenuSwipe.SlideMenu();

    }

    //used to find every object with a certain tag and store all the buttons and images into arrays to be controlled by the FadeSwitch function
    private void GatherData(string m, out GuiFadeInFadeOut[] thisthing, out Button[] otherThing)
    {
        GameObject thisObject = GameObject.FindGameObjectWithTag(m);
        thisthing = thisObject.GetComponentsInChildren<GuiFadeInFadeOut>();
        otherThing = thisObject.GetComponentsInChildren<Button>();
    }
   
    //executes the fade switch function for each UI part so they either fade in or out
    public void FadeSwitch(string menu)
    {
        switch(menu)
        {
            case "Map Menu":
                foreach (GameObject g in mapMenuParts)
                {
                    g.transform.GetComponent<GuiFadeInFadeOut>().FadeIn();
                    if (g.transform.GetComponent<Button>() != null)
                        {
                        
                        g.transform.GetComponent<Button>().enabled = true;
                        }
                }
                break;

            case "Options":
                foreach (GuiFadeInFadeOut g in optionsMenuParts)
                {
                    g.FadeIn();
                }

                foreach (Button b in optionsButtonParts)
                {
                    b.enabled = true;
                    
                }
                break;

            case "Scroll Mask":
                foreach (GuiFadeInFadeOut g in descriptionScrollMenu)
                {
                    g.FadeSwitch();
                }

                foreach (Button b in descriptionScrollButton)
                {
                    b.enabled = true;
                }
                break;

            case "Choose Area":
                foreach (GuiFadeInFadeOut g in chooseAreaMenu)
                {
                    g.FadeSwitch();
                }

                foreach (Button b in chooseAreaButton)
                {
                    b.enabled = true;
                }
                break;

            case "Compass Offset":
                foreach (GuiFadeInFadeOut g in compassOffsetMenu)
                {
                    g.FadeSwitch();
                }

                foreach (Button b in compassOffsetButton)
                {
                    b.enabled = true;
                }
                break;
            case "No Camera Image":
                foreach (GuiFadeInFadeOut g in noCameraMenu)
                {
                    g.FadeIn();
                }

                foreach (Button b in noCameraMenuButton)
                {
                    b.enabled = true;
                }
                break;
            case "Tutorial":
                foreach (GuiFadeInFadeOut g in tutorialMenu)
                {
                    g.FadeSwitch();
                }

                foreach (Button b in tutorialMenuButton)
                {
                    b.enabled = true;
                }
                break;
            case "Opening Menu":
                foreach (GuiFadeInFadeOut g in openingMenu)
                {
                    g.FadeSwitch();
                }

                foreach (Button b in openingMenuButton)
                {
                    b.enabled = true;
                }
                break;
        }
        
    }

    //executes the fade out function for each UI part since in some cases it needs to be turned off rather than relying o switching back and forth
    public void FadeOut(string menu)
    {

        switch (menu)
        {
            case "Map Menu":
                foreach (GameObject g in mapMenuParts)
                {
                    g.transform.GetComponent<GuiFadeInFadeOut>().FadeOut();
                    if (g.transform.GetComponent<Button>() != null)
                    {
                        g.transform.GetComponent<Button>().enabled = false;
                    }
                }
                break;

            case "Options":
                foreach (GuiFadeInFadeOut g in optionsMenuParts)
                {
                    g.FadeOut();
                }

                foreach (Button b in optionsButtonParts)
                {
                    b.enabled = false;
                }
                break;

            case "Scroll Mask":
                foreach (GuiFadeInFadeOut g in descriptionScrollMenu)
                {
                    g.FadeOut();
                }

                foreach (Button b in descriptionScrollButton)
                {
                    b.enabled = false;
                }
                break;

            case "Choose Area":
                foreach (GuiFadeInFadeOut g in chooseAreaMenu)
                {
                    g.FadeOut();
                }

             
                break;

            case "Compass Offset":
                foreach (GuiFadeInFadeOut g in compassOffsetMenu)
                {
                    g.FadeOut();
                }

                foreach (Button b in compassOffsetButton)
                {
                    b.enabled = false;
                }
                break;

            case "No Camera Image":
                foreach (GuiFadeInFadeOut g in noCameraMenu)
                {
                    g.FadeOut();
                }

                foreach (Button b in noCameraMenuButton)
                {
                    b.enabled = false;
                }
                break;
            case "Tutorial":
                foreach (GuiFadeInFadeOut g in tutorialMenu)
                {
                    g.FadeOut();
                }

                foreach (Button b in tutorialMenuButton)
                {
                    b.enabled = false;
                }
                break;
            case "Opening Menu":
                foreach (GuiFadeInFadeOut g in openingMenu)
                {
                    g.FadeOut();
                }

                foreach (Button b in openingMenuButton)
                {
                    b.enabled = false;
                }
                break;
        }
    }

  
}
