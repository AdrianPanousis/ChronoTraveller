using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class CompanyLogo : MonoBehaviour
{
    
    public GuiFadeInFadeOut LogoText;
    public GuiFadeInFadeOut Logo;
    
    void Start()
    {
        StartCoroutine(DelayFadeIn());
        StartCoroutine(DelayFade());
    }

    //Fades in the company logo at the start of the app
    private IEnumerator DelayFadeIn()
    {
        yield return new WaitForSeconds(0.1f);
        LogoText.FadeIn();
        Logo.FadeIn();
    }

    //fades out the company logo after 3 seconds
    private IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(3.0f);
        LogoText.FadeOut();
        Logo.FadeOut();

        //disables the text and logo so they aren't stopping the buttons on lower layers
        yield return new WaitForSeconds(1.0f);
        LogoText.enabled = false;
        Logo.enabled = false;
    }
}
