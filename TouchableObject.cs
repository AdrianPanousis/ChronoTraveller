using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class TouchableObject : MonoBehaviour
{
 
    public Text currentDescription;

    public Text[] descriptions;
    
    private string[] languages = new string[] {"English", "French", "German", "Spanish","Greek"};

   
    private Text textBox;
    private Text textShadowBox;
    private GuiFadeInFadeOut descriptionTextBackground;
    private GameObject scrollMask;
    private RectTransform scrollBox;
    private GameObject cancelDescriptionText;


   

    void Start()
    {
        textBox = GameObject.FindGameObjectWithTag("DescriptionText").GetComponent<Text>();
        textShadowBox = GameObject.FindGameObjectWithTag("Description Text Shadow").GetComponent<Text>();
        scrollMask = GameObject.FindGameObjectWithTag("Scroll Mask");
        scrollBox = scrollMask.transform.GetChild(0).transform.GetChild(0).GetComponent<RectTransform>();
        descriptionTextBackground = GameObject.FindGameObjectWithTag("Description Text Background").GetComponent<GuiFadeInFadeOut>();
        cancelDescriptionText = GameObject.FindGameObjectWithTag("Cancel Description Text");
      

    }

   

    

    //executes when the objects is tapped on screen
    private void OnMouseDown()
    {
        //checks if there is UI in front of the Canvas
        if (!IsPointerOverUIObject())
        {
            changeText();
            showText();
         
        }
        
    }

    //fades in all the components for the building description
    private void showText()
    {
        //fades in the mask used to show the text and hide parts not inside the background
        scrollMask.GetComponent<GuiFadeInFadeOut>().FadeIn();

        //fades in the button used to remove the text
        cancelDescriptionText.GetComponent<GuiFadeInFadeOut>().FadeIn();
        cancelDescriptionText.GetComponent<Button>().enabled = true;

        //fades in the text
        textBox.GetComponent<GuiFadeInFadeOut>().FadeIn();
        textShadowBox.GetComponent<GuiFadeInFadeOut>().FadeIn();

        //fades in the background for the text
        descriptionTextBackground.GetComponent<GuiFadeInFadeOut>().FadeIn();
    }

   
    private void changeText()
    {
        textBox.text = currentDescription.text;
        textShadowBox.text = currentDescription.text;

        //changes the position of the container to it alwasy starts from the beginning of the text
        scrollBox.localPosition = new Vector3(scrollBox.localPosition.x, 0, scrollBox.localPosition.z);
        
    }

    //checks if any UI objects are in front of the object when the screen is touched
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //returns true if it finds any objects in the way
        return results.Count > 0;
    }


    //changes the language of the text for this object
    public void changeLanguage(string l)
    {
        for (int i = 0; i < languages.Length; i++)
        {
            if(l == languages[i])
            {
                currentDescription = descriptions[i];
                changeText();
            }
            
        }

    }
    

}
