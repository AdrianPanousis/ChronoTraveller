using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoadingScreenImageSwitcher : MonoBehaviour
{
    // stores the loading screen images
    public Sprite[] LoadingScreenImages;
    //the number in the array of images
    private int imageNumber;
    //controls the amount of time passed to change the image
    private float timer;
    //stores the message that will be used
    public Text Message;
    //displays the message that will be used
    public TextMeshProUGUI displayText;

    private void Start()  
    {
        imageNumber = 0;
        displayText.text = Message.text;
    }
    //changes the image to the next one in the array 
    public void changeImages()
    {
        //checks if it is not the last image in the array
        if(imageNumber<(LoadingScreenImages.Length-1))
        {
            imageNumber += 1;
        }

        //resets to the first image number in the array
        else
        {
            imageNumber = 0;
        }

        //changes the image
        transform.GetComponent<Image>().sprite = LoadingScreenImages[imageNumber];
        displayText.text = Message.text;
    }

    // changes the image every 5 seconds
    void Update()
    {
        if(timer<5)
        {
            timer += Time.deltaTime;
        }

        else
        {
            changeImages();
            
            timer = 0;
            
        }

        
    }
}
