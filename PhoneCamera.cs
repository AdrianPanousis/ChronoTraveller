using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NatSuite.Devices;

public class PhoneCamera : MonoBehaviour
{
    private bool camAvailable;

    //texture that will show video
    private WebCamTexture backCam;

    //original background
    private Texture defaultBackground;

    //background image that will use camera texture
    public RawImage background;

    //image mask that hides the camera image
    public RectTransform CamMask;

    //value that determines how much of the camera image is shown over time
    private float slideTimer;

    //value used to determine which direction the camera mask slides
    private int slideSwitch;

    //used for the update to run the function to move the camera mask
    public bool isSliding;

    //checks if it is in test mode for the Unity Editor since the camera won't work with Unity Remote
    public bool isTesting;

    //position when the camera mask is not being used
    private Vector3 hiddenPosition;
    //position when the camera mask is being used to show the image
    private Vector3 shownPosition;
    //values used to scale the mask to create the effect of it sliding back and forth between camera mode and normal mode
    private Vector2 CamMaskScaled;
    private Vector2 CamMaskNotScaled;

    //stores the GUI object
    private GameObject thisCanvas;
    //stores the ratio of the X value
    private float aspectRatioX;


    
    private void Start()
    {
        //stores default camera ratio based on the screen size
        aspectRatioX = ((float)Screen.width / (float)Screen.height) / (16.0f / 9.0f);
        defaultBackground = background.texture;
        slideTimer = 0;
        slideSwitch = 0;
        isSliding = false;
        //values used for the camera mask sliding back and forth
        hiddenPosition = new Vector3(0, 540, 0);
        shownPosition = new Vector3(0,0,0);
        CamMaskScaled = new Vector2(1920*aspectRatioX, 1080);
        CamMaskNotScaled = new Vector2(1920*aspectRatioX, 0);


        thisCanvas = GameObject.FindGameObjectWithTag("Canvas");

        //stores cameras of device
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            camAvailable = false;
            return;
        }

        //checks for back camera and stores it
        for (int i = 0; i < devices.Length; i++)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                

            }
        }


        if (backCam == null)
        {
            Debug.Log("No backcam");
            
            return;
        }

        //starts using backcam

        backCam.Play();
        background.texture = backCam;

        camAvailable = false;
    }

    private void Update()
    {
        //executes sliding back and forth showing the image coming from the back camera
        if (isSliding == true)
        {
           
            Slide(slideSwitch);
        }

        else
        {

        }

        
        if (backCam.isPlaying)
        {

            //matches the scale of the image to the camera
            float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
            
            
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

                


            //orient camera so it's not upside down or in reverse
            int orient = -backCam.videoRotationAngle;

            background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
            
        }

        

        else
        {

        }


    }

    public void TurnOnCamera()
    {
        
        camAvailable = true;
        //shows the image coming from the camera
        backCam.Play();
        background.texture = backCam;


        isSliding = true;

        
        
    }

   

    public void TurnOffCamera()
    {     
        isSliding = true;
    }

    //when called it switches back and forth between the camera image being on and off
    public void CameraSwitch()
    {
        //checks if the camera either works or has been given permission to be used by the phone
        if (backCam != null) 
        {
            slideSwitch = Mathf.Abs(slideSwitch - 1);
            if (slideSwitch == 1)
            {
                TurnOnCamera();
            }

            else
            {
                TurnOffCamera();
            }
        }
        //executes when the camera isn't working
        else
        {
            //is used  when testing in the Unity Editor
            if (isTesting)
            {
                slideSwitch = Mathf.Abs(slideSwitch - 1);
                isSliding = true;
                camAvailable = true;
            }
            //used when the camea is not working
            else
            {
                thisCanvas.GetComponent<MapMenuRemoval>().FadeSwitch("No Camera Image");
            }
           
        }
    }

    //slides the camera image back and forth
    public void Slide(int s)
    {
        //slowly shows the camera image
        if(s == 1)
        {
            if (slideTimer < 1)
            {
                slideTimer += Time.deltaTime;
            }

            else
            {
                //stops the sliding and sets the values to the shown positions
                slideTimer = 1;
                isSliding = false;
                CamMask.anchoredPosition = Vector3.Lerp(hiddenPosition, shownPosition, slideTimer);
                background.rectTransform.anchoredPosition = Vector3.Lerp(hiddenPosition*-1,shownPosition,slideTimer);
                CamMask.sizeDelta = Vector2.Lerp(CamMaskNotScaled, CamMaskScaled, slideTimer);
            }
        }

        //slowly removes the camera image
        else
        {
            if (slideTimer > 0)
            {
                slideTimer -= Time.deltaTime;
            }

            else
            {
                //stops the sliding and sets the values to the hidden positions
                slideTimer = 0;
                isSliding = false;
                CamMask.anchoredPosition = Vector3.Lerp(hiddenPosition, shownPosition, slideTimer);
                background.rectTransform.anchoredPosition = Vector3.Lerp(hiddenPosition * -1, shownPosition, slideTimer);
                CamMask.sizeDelta = Vector2.Lerp(CamMaskNotScaled, CamMaskScaled, slideTimer);

                //changes the background color and turns off the camera
                background.texture = defaultBackground;
                background.color = new Color(1, 1, 1, 1);
                camAvailable = false;
                backCam.Stop();
            }
        }

        //changes the camera mask and the camera image over time
        CamMask.anchoredPosition = Vector3.Lerp(hiddenPosition, shownPosition, slideTimer);
        background.rectTransform.anchoredPosition = Vector3.Lerp(hiddenPosition * -1, shownPosition, slideTimer);
        CamMask.sizeDelta = Vector2.Lerp(CamMaskNotScaled, CamMaskScaled, slideTimer);

    }
}
