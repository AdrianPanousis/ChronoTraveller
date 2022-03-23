using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCompassOffsetText : MonoBehaviour
{
    private Text thisText;
    private bool textLoaded;
   
    void Start()
    {
        textLoaded = false;
        thisText = transform.GetComponent<Text>();
        StartCoroutine(Delay());
    }

    //delays the text being displayed and changed until the GetComponent for thistext is complete
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(2.0f);

        if(thisText!= null)
        {
            textLoaded = true;

        }
    }

    //changes the value of the compass offset value
    public void changeText()
    {
        if (textLoaded == true)
        {
            float value = Mathf.Round(PlayerPrefs.GetFloat("compassOffset") * 10) / 10;
            thisText.text = value.ToString();
        }
    }

    
}
