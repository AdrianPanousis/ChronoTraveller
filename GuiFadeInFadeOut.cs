using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GuiFadeInFadeOut : MonoBehaviour
{
    //booleans storing if the object will be fading in or out
    private bool isFadingIn;
    private bool isFadingOut;
   

    //the value that will determine how transparent the object is
    private float fadeValue;

    //the value that will determine whether a button will switch the UI element on or off
    private float fadeSwitchValue;

    //stores the property that controls the color of the object
    private Image thisImage;
    private Text thisText;
    private Button thisButton;
    private TextMeshProUGUI thisDynamicText;
    private ScrollRect thisScrollRect;

    private bool isButton;

    //string storing the type of object it is
    public string objectType;

    //stores the original color so any changes can be reverted
    private Color originalColor;

    //checks if the text is the shadow object
    public bool isShadow;

    //checks if the object is the dirt over the menu
    public bool isDirtMask;

    //checks if the object is in the opening scene and not hidden by the sparkle mask
    public bool isOpening;

    //checks if the object raycasts
    public bool willRaycast;

    //checks if the button is a level button that changes which environment you go to
    public bool isLevelButton;

    //checks if the level for the level button is downloaded
    private bool isDownloaded;

    //stores the original alpha value so any changes can be reverted
    private float alphaValue;

    void Start()
    {
        isFadingIn = false;
        isFadingOut = false;
        isButton = false;
        fadeValue = 0;
        fadeSwitchValue = 0;
        isDownloaded = false;

        if (isShadow == true)
        {
            alphaValue = 0.6f;
        }
        else
        {
            alphaValue = 1;
        }

        
        

        //checks if the object is an image
        if (transform.GetComponent<Image>() != null)
        { 
            thisImage = transform.GetComponent<Image>();
            if (isLevelButton)
            {
                objectType = "Level Button";
            }
            else
            {
                objectType = "Image";
            }
            thisImage.raycastTarget = false;
        }

        //checks if the object is text
        if (transform.GetComponent<Text>() != null)
        {
            thisText = transform.GetComponent<Text>();
            objectType = "Text";
            originalColor = thisText.color;
            thisText.raycastTarget = false;
        }

        //checks if the object has a button component and disables the button on startup
        if (transform.GetComponent<Button>() != null)
        {
            thisButton = transform.GetComponent<Button>();
            isButton = true;
            thisButton.interactable = false;
        }


        if (isOpening == true)
        {
            Raycasting(true);
        }

        //checks if the object is dynamic text
        if (transform.GetComponent<TextMeshProUGUI>() != null)
        {
            thisDynamicText = transform.GetComponent<TextMeshProUGUI>();
            objectType = "DynamicText";
            originalColor = thisDynamicText.color;
            thisDynamicText.raycastTarget = false;
        }

        //checks if the object is a scrolling object
        if(transform.GetComponent<ScrollRect>() != null)
        {
            thisScrollRect = transform.GetComponent<ScrollRect>();
            thisImage = transform.GetComponent<Image>();
            objectType = "Scroll";
        }
        

        
        
           
        
    }

    
    void Update()
    {
        //executes the fading in or fading out functions
        if(isFadingIn == true)
        {
            FadingIn();
            
        }

        if(isFadingOut == true)
        {
            FadingOut();
        }
    }

    public void Downloaded()
    {
        isDownloaded = true;
    }
    public void FadeSwitch()
    {
        //makes sure the value will switch to 0 or 1 whenever a button is clicked
        fadeSwitchValue = Mathf.Abs(fadeSwitchValue - 1);

        //checks if the fade switch is set to fading in
        if(fadeSwitchValue == 1)
        {
            isFadingIn = true;
            isFadingOut = false;
            Raycasting(true);

        }

        //checks if the fade switch is set to fading out
        else
        {
            isFadingOut = true;
            isFadingIn = false;
            Raycasting(false);
        }
    }

    //makes sure the object always fades out when called and the switch value goes back to zero to avoid the switch value breaking the menu
    public void FadeOut()
    {
        isFadingOut = true;
        isFadingIn = false;
        fadeSwitchValue = 0;
        Raycasting(false);
    
    }

    public void FadeIn()
    {
        isFadingIn = true;
        isFadingOut = false;
        fadeSwitchValue = 1;
        Raycasting(true);
        
        

    }

    //Turns off raycasting for the object
    private void Raycasting(bool r)
    {
        if (objectType == "Image")
        {
            if (willRaycast)
            {
                thisImage.raycastTarget = r;
            }
            if(isButton == true)
            {
                thisButton.interactable = r;
            }
        }

        if(objectType == "Level Button")
        {
            if (willRaycast)
            {
                thisImage.raycastTarget = r;
            }
            if (isButton == true)
            {
                thisButton.interactable = r;
            }
        }


        if (objectType == "Text")
        {
            thisText.raycastTarget = r;
        }

        if (objectType == "Scroll")
        {
            thisImage.raycastTarget = r;
            thisScrollRect.enabled = r;
        }
    }

    private void FadingOut()
    {
        //decreases the fade value over time to slowly dissappear
        if (fadeValue > 0)
        {
            fadeValue -= 1 * Time.deltaTime;
            ColorChange(fadeValue);

        }

        //stops object from fading once invisible
        else
        {
            fadeValue = 0;
            ColorChange(fadeValue);
            EnabledSwitch(false);
            isFadingOut = false;
            
        }
    }

    private void FadingIn()
    {
        //increases the fade value over time to slowly appear
        
        if (fadeValue < 1)
        {
            
            fadeValue += 1 * Time.deltaTime;
            ColorChange(fadeValue);
            EnabledSwitch(true);
            

        }

        // stops object fading in once fully visible
        else
        {
            fadeValue = 1;
            ColorChange(fadeValue);
            isFadingIn = false;

            //turns off raycasting for the dirt mask so it doesn't block other objects from being touched
            if (isDirtMask == true)
            {
                thisImage.raycastTarget = false;
            }

           
        }
    
    }

    public Color getColor()
    {
        return thisImage.color;
    }

    //controls the color value change over time
    public void ColorChange(float f)
    {
        //changes only the alpha value
        if(objectType == "Image")
        {
            thisImage.color = new Color(1, 1, 1, f*alphaValue);
            
        }

        if(objectType == "Level Button")
        {
            //checks if the level the button sends you too has not been downloaded
            if (!isDownloaded)
            {
                thisImage.color = new Color(0.3f, 0.3f, 0.3f, f * alphaValue);
            }

            //the full color is shown
            else
            {
                thisImage.color = new Color(1, 1, 1, f * alphaValue);
            }
        }

        //changes all of the colors
        else if (objectType == "Text")
        {
            thisText.color = new Color((originalColor.r), originalColor.g, originalColor.b, f*alphaValue);
        }

        else if (objectType == "DynamicText")
        {
            thisDynamicText.color = new Color((originalColor.r), originalColor.g, originalColor.b, f * alphaValue);
        }
    }

    //changes the defualt alpha value when the objects is visible
    public void changeAlpha(float f)
    {
        alphaValue = f;
    }

    //controls whether the image or text object is enabled in the scene. Is needed for images since even if it is transparent it makes the virtual joystick unusable since it is on top of it.
    private void EnabledSwitch(bool b)
    {
        if (objectType == "Image")
        {
            thisImage.enabled = b;
        }

        else if(objectType == "Text")
        {
            thisText.enabled = b;
            
        }

        else if (objectType == "DynamicText")
        {
            thisDynamicText.enabled = b;

        }
    }

  

    
}
