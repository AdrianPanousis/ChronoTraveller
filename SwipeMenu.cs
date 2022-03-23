using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    
 

    private GameObject menu;
    private bool MenuOnScreen;
    private bool MenuOffScreen;
    private int buttonSwitch;

    private float time;

    //positions for the menu
    private Vector3 MenuNotShown;
    private Vector3 MenuShown;
    public Button SlideButton;


    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("SwipeMenu");
        MenuOnScreen = false;
        MenuOffScreen = false;

        //positions for the menu when it is off screen and when it is visibile
        MenuNotShown = new Vector3(1750f, -461f, 0);
        MenuShown = new Vector3(-319f, -461f, 0);

        time = 0;
        buttonSwitch = 0;

        //sets the menu's position so it is off screen by default
        menu.transform.localPosition = MenuNotShown;
    }

    void Update()
    {
        //executes the menu movement
        if(MenuOnScreen == true)
        {
            moveMenu(1);
            
        }

        if(MenuOffScreen == true)
        {
            moveMenu(-1);
           
        }
    }

    //switches between the menu being on and off screen every time the menu button is pressed
    public void SlideMenu()
    {

        buttonSwitch = Mathf.Abs(buttonSwitch-1);

        if(buttonSwitch == 1)
        {
            MenuOnScreen = true;
            SlideButton.enabled = false;
        }

        else
        {
            MenuOffScreen = true;
            SlideButton.enabled = false;
        }

    }

    private void moveMenu(float direction)
    {
        //controls the speed the menu moves
        time += Time.deltaTime * 1.5f * direction;

        //executes when the menu is sliding on screen
        if (direction == 1)
        {
            //moves the menu
            if (time <= 1)
            {

                menu.transform.localPosition = Vector3.Lerp(MenuNotShown, MenuShown, time);
            }

            //stops moving the menu
            else
            {
                MenuOnScreen = false;
                time = 1;
                menu.transform.localPosition = Vector3.Lerp(MenuNotShown, MenuShown, 1);
                SlideButton.enabled = true;


            }
        }

        //executes when the menu is sliding off screen
        if (direction == -1)
        {
            //moves the menu
            if (time >= 0)
            {

                menu.transform.localPosition = Vector3.Lerp(MenuNotShown, MenuShown, time);
            }

            //stops moving the menu
            else
            {
                MenuOffScreen = false;
                time = 0;
                menu.transform.localPosition = Vector3.Lerp(MenuNotShown, MenuShown, 0);
                SlideButton.enabled = true;


            }
        }



    }

    //removes the menu
    public void removeMenu()
    {
        MenuOffScreen = true;
        buttonSwitch = Mathf.Abs(buttonSwitch - 1);
    }

    

    

    
}
