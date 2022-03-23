using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageBox : MonoBehaviour
{
    //stores the text for each language
    private Text[] translations;
    //accesses the shadow layer for the text
    private Text shadowText;
    //accesses the normal text object
    private Text normalText;
    //accesses the dynamic text object
    public TextMeshProUGUI dynamicText;
    //checks whether it is dynamic text if true or not
    public bool isTextMesh;

    
    enum languages
    {
        English,
        French,
        German,
        Spanish,
        Greek
    }

    void Start()
    {
        //checks if the object is dynamic text or not and gathers the text that will be changed
        if (isTextMesh)
        {
            translations = gameObject.transform.GetChild(0).GetComponentsInChildren<Text>();
        }

        else
        {
            translations = gameObject.transform.GetChild(1).GetComponentsInChildren<Text>();
            normalText = gameObject.transform.GetChild(0).GetComponent<Text>();
            shadowText = gameObject.GetComponent<Text>();
        }
        
        
        
    }

   
    
    //used to change the language of the text
    public void ChangeText(string language)
    {
        //converts the string for the language to change into a number using the "languages" enum
        languages l = (languages)System.Enum.Parse(typeof(languages), language);
        int languageNum = (int)l;
        
        //changes the text by using the converted number to access the text in the array
        if(isTextMesh == true)
        {
            dynamicText.text = translations[languageNum].text;
        }

        else
        {
            shadowText.text = translations[languageNum].text;
            normalText.text = translations[languageNum].text;
        }
        
    }
}
