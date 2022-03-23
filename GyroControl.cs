using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour
{
    // Start is called before the first frame update
    private bool gyroEnabled;
  
    private Vector3 offsetRotation;
    private float normaliseRotation;
    private Vector3 currentRotCompass;
    private float currentHeading;
  
    private Options options;

    //values used for the low pass filter
    float accelerometerUpdateInterval = 1.0f / 90.0f;
    float lowPassKernelWidthInSeconds = 0.15f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;
 


    void Start()
    {
        //adds a delay since the compass data may not able to be gathered instantly
        StartCoroutine(CompassDelay());
        
        gyroEnabled = EnableGyro();

        normaliseRotation = 0;
       
        currentHeading = 0;
        options = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Options>();

        //gets the ratio to control the strength of the filter that will smooth out the values but slow down responsiveness
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        //gets the accelerometer values
        lowPassValue = Input.acceleration;









    }

    private IEnumerator CompassDelay()
    {
        yield return new WaitForSeconds(5.0f);
        //enabled the compass
        Input.location.Start();
        Input.compass.enabled = true;
    }

    //checks if the phone supports the gyroscope
    private bool EnableGyro()
    {

        if (SystemInfo.supportsGyroscope)
        {
            //sets the default rotation if the compass is at 0
            offsetRotation = new Vector3(90f, 360f, 0);

            return true;
        }

        else
        {
            return false;
        }
    }

    
    void Update()
    {

        

        if (gyroEnabled)
        {   
            //puts the acceleromter value through the filter
            lowPassValue = LowPassFilterAccelerometer(lowPassValue);
           //puts the compass value through the filter
            currentHeading = Mathf.LerpAngle(currentHeading, Input.compass.trueHeading, lowPassFilterFactor);
            //converts the accelerometer values into degrees that can be used as rotation
            normaliseRotation = Mathf.Atan2(lowPassValue.y, lowPassValue.z) * 180 / Mathf.PI+180;
           //gets the proper rotation values after conversions and the offsets
            currentRotCompass = new Vector3(-normaliseRotation, currentHeading + options.getCompassOffset(), 0)+offsetRotation;
            //rotates the character
            transform.localRotation = Quaternion.Euler(currentRotCompass);
        }
    }

    //sets the value of the roll through the filter
    Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
    {
        Vector3 newValue = Vector3.Lerp(prevValue, Input.acceleration, lowPassFilterFactor);
        return newValue;
    }


   






}

