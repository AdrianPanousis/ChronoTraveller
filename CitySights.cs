using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CitySights : MonoBehaviour
{
    private GuiFadeInFadeOut[] images;
    private Button[] buttons;
    
    void Start()
    {
        //Gets the images of the level buttons
        images = transform.GetComponentsInChildren<GuiFadeInFadeOut>();
        //Gets the buttons
        buttons = transform.GetComponentsInChildren<Button>();
    }

    private IEnumerator LoadingProgress()
    {
        yield return new WaitForSeconds(0.05f);
        //fades in the level select buttons and makes them functional
        foreach (GuiFadeInFadeOut g in images)
        {
            g.FadeIn();
        }

        foreach (Button b in buttons)
        {
            b.enabled = true;

        }
    }

    public void fadeIn()
    {
        //delays slightly due to a Unity bug
        StartCoroutine(LoadingProgress());
    }

    public void fadeOut()
    {
        //fades out the level select buttons and turns them off
        foreach (GuiFadeInFadeOut g in images)
        {
            g.FadeOut();
        }

        foreach (Button b in buttons)
        {
            b.enabled = false;

        }
    }
}
