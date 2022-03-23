using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeRotateCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float distanceToTarget = 10;
    public float longitude;
    public float latitude;

    private float rotationSpeed = 180;
    private Vector3 previousPosition;
    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;
    Vector2 firstTouchPrevPos, secondTouchPrevPos;
    private bool doubleTouched = false;
    public bool isMovingToGPS = false;

    private float LongitudeMovementTimer = 0;
    private float LatitudeMovementTimer = 0;

    private Vector3 originalRotation;
    private Vector3 newRotation;

    private float latitudeDistance;
    private float longitudeDistance;
    private float GPSMovementSpeed = 3.5f;


    [SerializeField]
    float zoomModifierSpeed = 0.0001f;

    private MapPointButton[] mapPoints;

    public GameObject mapPointContainer;

    private void Start()
    {
        //gets all the map point buttons in the scene
        mapPoints = mapPointContainer.GetComponentsInChildren<MapPointButton>();
        StartCoroutine(DelayTimer());
    }

    private IEnumerator DelayTimer()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartLocationService());

    }

     public float GetLatitude()
    {
        float lat = transform.localRotation.eulerAngles.x;
        return lat;
    }

    public float GetLongitude()
    {
        float lon = -transform.localRotation.eulerAngles.y;
        return lon;
    }
    void Update()
    {
        
        if(isMovingToGPS)
        {
            MoveToGPS();
            
        }

        if (!doubleTouched)
        {
            //executes when the phone is touched by one finger
            if (Input.touchCount == 1)
            {
                //executes when the screen is tapped
                if (Input.GetMouseButtonDown(0))
                {
                    previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                }
                //executes when the tap is still held down
                else if (Input.GetMouseButton(0))
                {
                    Vector3 newPosition = cam.ScreenToViewportPoint(Input.mousePosition);
                    Vector3 direction = previousPosition - newPosition;

                    // gets horizontal rotation value
                    float rotationAroundYAxis = -direction.x * rotationSpeed;
                    // gets verticalrotation value
                    float rotationAroundXAxis = direction.y * rotationSpeed; 

                    //makes sure the camera is at the same position as the global to focus on it
                    cam.transform.position = target.position;

                    // camera moves horizontally in local space
                    cam.transform.Rotate(new Vector3(1, 0, 0), rotationAroundXAxis);
                    // camera moves vertically in world space so the globe's position for north and south stays the same
                    cam.transform.Rotate(new Vector3(0, 1, 0), rotationAroundYAxis, Space.World); 

                    //zooms out the camera so it's not in the centre of the globe
                    cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

                    previousPosition = newPosition;
                }
            }
        }

        else
        {
            //do nothing
        }

        //checks if two fingers are touching the phone screen
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0); 
            Touch secondTouch = Input.GetTouch(1);
            doubleTouched = true;

            //measures the difference between the initial touches and where the fingers have moved
            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            //gets the diference between the touches on the previous frame
            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            //gets the distance between the two current touches
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;
            //used to determine how much to change the zoom
            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed * 0.01f;

            //checks if it is scaling the camera zoom in since the distance between touches is decreasing
            if (touchesPrevPosDifference < touchesCurPosDifference)
            {
                if (distanceToTarget > 6)
                {
                    //gets the new distance for the camera
                    distanceToTarget = distanceToTarget - zoomModifier;
                    //sets the camera to the same position as the globe but then pulls it back
                    cam.transform.position = target.position;
                    cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
                    changeRotationSpeed(distanceToTarget);
                }

                //executes when the camera reaches it's minimum distance and stops any more zooming
                else
                {
                    distanceToTarget = 6;
                    cam.transform.position = target.position;
                    cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
                    changeRotationSpeed(distanceToTarget);
                }
            }

            //checks if it is scaling the camera zoom out since the distance between touches is increasing
            if (touchesPrevPosDifference > touchesCurPosDifference)
            {
                //executes until the camera is zoomed out at it's it's maximum
                if (distanceToTarget <= 10)
                {
                    distanceToTarget = distanceToTarget + zoomModifier;
                    cam.transform.position = target.position;
                    cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
                    changeRotationSpeed(distanceToTarget);
                }

                if (distanceToTarget < 6)
                {
                    distanceToTarget = 6;
                    cam.transform.position = target.position;
                    cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));
                    changeRotationSpeed(distanceToTarget);
                }
            }

        }

        if(Input.touchCount == 0)
        {
            doubleTouched = false;
        }

       
    }



    public float getDistance()
    {
        return distanceToTarget;
    }

    //changes how fast the camera rotates around the globe depending on how zoomed in it is
    private void changeRotationSpeed(float d)
    {
        //sets the value of the rotation speed between 60 and 180, depending on the distance. 
        //At 6, it is 60 and at 10 it is 180, and values in between will calculate a value using the ratio. A distance of 8 for eample would be 120
        rotationSpeed = Mathf.Lerp(60, 180, (d - 6f) / 4f);
    }

    //tells the script to start moving the camera to the actual GPS point
    public void StartGPSMove()
    {
        //gets the current rotation
        originalRotation = transform.localRotation.eulerAngles;
        //gets where it is rotating to
        newRotation = new Vector3(latitude, -longitude, 0);
        //gets the angle distance between the two rotations
        longitudeDistance = Mathf.DeltaAngle(originalRotation.y ,newRotation.y);
        latitudeDistance = Mathf.DeltaAngle(originalRotation.x , newRotation.x);
        //starts moving
        isMovingToGPS = true;
        
    }

    //gets GPS data from the phone
    private IEnumerator StartLocationService()
    {
        //checks if the user has allowed the app to get GPS data
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        //starts gathering GPS data
        Input.location.Start();

        //waits for a few seconds while the app gets the GPS data before setting the longitude and lattitude.
        yield return new WaitForSeconds(5.0f);
        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);

            maxWait--;
        }

        //after 20 seconds the app stops trying to get the GPS data if it fails to work
        if (maxWait <= 0)
        {
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }

        //get GPS coordinates from phone
        longitude = Input.location.lastData.longitude;
        latitude = Input.location.lastData.latitude;


       
    }


    private void MoveToGPS()
    {
        //uses the distances between the rotation values to determine how fast to rotate the camera so it's not just a fixed value. Otherwise long distances would take too long or short distances would be too uick
        LongitudeMovementTimer = (longitudeDistance/360);
        LatitudeMovementTimer = (latitudeDistance / 360);

        cam.transform.position = target.position;

        //checks if the distance between the x rotations is greater than 1
        if (Mathf.DeltaAngle(newRotation.x, transform.localRotation.eulerAngles.x) > 1 || Mathf.DeltaAngle(newRotation.x, transform.localRotation.eulerAngles.x) < -1)
        {
            //checks if the distance between the x rotations is greater than 1 and then starts rotating thge camera towards the new rotation
            if (Mathf.DeltaAngle(newRotation.y, transform.localRotation.eulerAngles.y) > 1 || Mathf.DeltaAngle(newRotation.y, transform.localRotation.eulerAngles.y) < -1)
            {
                cam.transform.Rotate(new Vector3(1, 0, 0), LatitudeMovementTimer * GPSMovementSpeed);

                cam.transform.Rotate(new Vector3(0, 1, 0), LongitudeMovementTimer * GPSMovementSpeed, Space.World);

                //
                foreach (MapPointButton m in mapPoints)
                {
                    m.checkGPS(transform.localRotation.eulerAngles.x, transform.localRotation.eulerAngles.y);
                    
                }
            }

            else
            {
                isMovingToGPS = false;
            }
        }
       

        else
        {
            isMovingToGPS = false;
            
        }

        cam.transform.Translate(new Vector3(0, 0, -distanceToTarget));

    }
}
