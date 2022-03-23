using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    private int currentSlide;
    //stores objects in the scene
    private GameObject tutorialTextStorage;
    private Image tutorialSlide;
    private TutorialSlideStorage slideStorage;

    //stores all of the text for the ttorial
    private Text[] tutorialTexts;
    //stores objects that display the text
    private GameObject[] displayTexts;
    public GameObject dynamicText;

    //determines the offset value that determines which langauge the text will use
    private int languageOffset = 0;

    //stores the map button
    public Button MapButton;

    private bool isOpeningMenu;
   



  
    void Start()
    {
        currentSlide = 1;
        languageOffset = 0;
       
        //gets all the data for the tutorial
        isOpeningMenu = transform.GetComponent<MapMenuRemoval>().isOpeningMenu;
        tutorialTextStorage = GameObject.FindGameObjectWithTag("Tutorial Text Storage");
        tutorialSlide = GameObject.FindGameObjectWithTag("Tutorial Image").GetComponent<Image>();
        displayTexts = GameObject.FindGameObjectsWithTag("Tutorial Text");
        slideStorage = tutorialSlide.GetComponent<TutorialSlideStorage>();
        fillArray();
        


    }

    //changes to the next slide button is pressed
    public void nextSlide()
    {
        //checks if the user is already on the last slide
        if (currentSlide < slideStorage.Slides.Length)
        {
            //changes the slide
            currentSlide += 1;
            int number = ((currentSlide-1)*5) + languageOffset;
            //changes the image
            tutorialSlide.sprite = slideStorage.Slides[currentSlide - 1];
            //changes the text
            displayTexts[0].GetComponent<Text>().text = tutorialTexts[number].text;
            displayTexts[1].GetComponent<Text>().text = tutorialTexts[number].text;
            if(dynamicText != null)
            {
                dynamicText.GetComponent<TextMeshProUGUI>().text = tutorialTexts[number].text;
            }
        }

        //executes when the next button is pressed on the last slide
        else
        {
            if (!isOpeningMenu)
            {
                //goes straight to the map menu and sets the global variable stating that the tutorial has been seen, preventing it from loading by default
                MapButton.onClick.Invoke();
                PlayerPrefs.SetInt("IntroTutorialSeen", 1);
            }
        }

        
    }
    //changes to the back slide button is pressed
    public void previousSlide()
    {
        //checks if the user is on the first slide
        if (currentSlide>1)
        {
            //changes the slide
            currentSlide -= 1;
            int number = ((currentSlide - 1) * 5) + languageOffset;
            //changes the image
            tutorialSlide.sprite = slideStorage.Slides[currentSlide - 1];
            //changes the text
            displayTexts[0].GetComponent<Text>().text = tutorialTexts[number].text;
            displayTexts[1].GetComponent<Text>().text = tutorialTexts[number].text;
            if (dynamicText != null)
            {
                dynamicText.GetComponent<TextMeshProUGUI>().text = tutorialTexts[number].text;
            }
        }
        

    }

    //changes the language of the text using the offset number 
    private void changeCurrentSlideLanguage()
    {
        int number = ((currentSlide - 1) * 5) + languageOffset;
        displayTexts[0].GetComponent<Text>().text = tutorialTexts[number].text;
        displayTexts[1].GetComponent<Text>().text = tutorialTexts[number].text;
    }

    
    //changes the offset value since each slide has a list of 5 language texts
    public void changeLanguage(string language)
    {
        switch(language)
        {
            case "English":
                languageOffset = 0;
                break;
            case "Spanish":
                languageOffset = 1;
                break;
            case "French":
                languageOffset = 2;
                break;
            case "German":
                languageOffset = 3;
                break;
            case "Greek":
                languageOffset = 4;
                break;
        }

        changeCurrentSlideLanguage();
    }
    
    //gathers all of the text for each step in the tutorial and stores it
    private void fillArray()
    {
        tutorialTexts = tutorialTextStorage.transform.GetComponentsInChildren<Text>();
    }

    
}
