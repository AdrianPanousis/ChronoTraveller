using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaScrollSnap : MonoBehaviour
{
    // Start is called before the first frame update
    private int childCount;
    private int currentMidPoint;
    private const float slideWidth = 950;
    private const float slideSpacing = 100;

    private float[] snapPoints;
    private float[] distances;

    private Vector2 initialPosition;
    private float snapTime;

    private bool isSnapping;
    void Start()
    {
        //childCount = transform.childCount;
        childCount = transform.GetComponentsInChildren<Button>().Length;
        //print(childCount);
        snapPoints = new float[childCount];
        distances = new float[childCount];
        snapTime = 0;

        for(int i = 0; i<childCount; i++)
        {
            snapPoints[i] = (slideWidth + slideSpacing) * -i;
           
        }

    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = new Vector3(0, 0, 0);
        if(isSnapping == true)
        {
            Snap(currentMidPoint);
        }

        

    }

    private void Snap(int midpoint)
    {
        
        snapTime += Time.deltaTime * 8;
        float xPos = Mathf.Lerp(initialPosition.x, snapPoints[midpoint], snapTime);
        transform.localPosition = new Vector3(xPos, 0, 0);
        if(snapTime >1)
        {
            isSnapping = false;
            snapTime = 0;
        }
    }

    public void MidPointCheck()
    {
        
        for (int i = 0; i < childCount; i++)
        {
            distances[i] = Vector3.Distance(transform.localPosition,new Vector3(snapPoints[i],0,0));
            //print("XPos is " + transform.localPosition.x);
            
            
        }

        for (int i = 0; i < childCount; i++)
        {
            
            if(distances[i] == Mathf.Min(distances))
            {
                currentMidPoint = i;
                initialPosition = transform.localPosition;
                isSnapping = true;
            }
        }
            
        
    }

    public void ButtonCheck()
    {
        //print("Button Clicked");
    }
}
