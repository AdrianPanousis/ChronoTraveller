using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectionHighlight : MonoBehaviour
{
    //stores the text
    private Text shadowText;
    private TextMeshProUGUI dynamicText;
    private GuiFadeInFadeOut fadeScript;

    //stores the shadow object
    public GameObject shadowChild;

    //stores the name of which button this stores
    public string theName;

    public bool defaultHighlight;
    public bool isDynamicText;

    //the two different dynamic text fonts that the app will switch between
    public TMP_FontAsset highlightedFont;
    public TMP_FontAsset unhighlightedFont;
   
    //this script changes text in the menu to be highlighted or not and is used for things like the current language selected or the quality setting
    void Start()
    {
        
        StartCoroutine(DelayTimer());

        //gets the dynamic text
        if (isDynamicText == true)
        {
            dynamicText = shadowChild.GetComponent<TextMeshProUGUI>();
            //checks if the text is highlighted by default and changes the font
            if (defaultHighlight == false)
            {
                dynamicText.font = unhighlightedFont; 
            }

            else
            {
                dynamicText.font = highlightedFont;
            }
        }

        //gets the standard text
        else
        {
            shadowText = shadowChild.GetComponent<Text>();
            fadeScript = shadowChild.GetComponent<GuiFadeInFadeOut>();
            
            //removes the alpha of the shadow text so it is not highlighted
            if (defaultHighlight == false)
            {
                fadeScript.changeAlpha(0);     
            }

        }
        


    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(2.0f);
    }

    

    //changes the text so it looks highlighted
    public void Highlighted()
    {
       
        if (isDynamicText == true)
        {
            //changes the dynmaic font
            dynamicText.font = highlightedFont;
        }
        else
        {
         
            //changes the standard font so the shadow is visible
            fadeScript.changeAlpha(0.6f);
            shadowText.enabled = true;
            shadowText.color = new Color(0, 0, 0, 0.5f);
            
        }
    }

    //changes the text so it looks unhighlighted
    public void Unhighlighted()
    {
        if (isDynamicText == true)
        {
            //changes the dynmaic font
            dynamicText.font = unhighlightedFont;
        }
        else
        {
            //makes the shadow text invisible
            fadeScript.changeAlpha(0);
            shadowText.enabled = false;
        }
    }


}
