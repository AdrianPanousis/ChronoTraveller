using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadjustCameraImage : MonoBehaviour
{
    private float PosXRatio;
    private Vector2 position;
   
    // Start is called before the first frame update
    void Start()
    {
        //changes the sizeof the camera image to match the screen size and aspect ratio of the phone
        PosXRatio = ((float)Screen.width / (float)Screen.height) / (16.0f / 9.0f);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(transform.GetComponent<RectTransform>().sizeDelta.x * PosXRatio, transform.GetComponent<RectTransform>().sizeDelta.y);
    }


   
   
}
