using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapSizeData : MonoBehaviour
{
  
    //stores the size of the map
    private float mapWidth;
    private float mapHeight;

    //stores the initial size of the map
    private float initialMapWidth;
    private float initialMapHeight;

    void Start()
    {
       
        StartCoroutine(LoadData());
        
    }

    private IEnumerator LoadData()
    {
        //gets the data from the map
        yield return new WaitForSeconds(0.2f);
        mapWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        mapHeight = transform.GetComponent<RectTransform>().sizeDelta.y;
        initialMapWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
        initialMapHeight = transform.GetComponent<RectTransform>().sizeDelta.y;
    }

    
    //gets the current map size
    private void getMapSize()
    {
        mapWidth = transform.GetComponent<RectTransform>().sizeDelta.x * transform.localScale.x;
        mapHeight = transform.GetComponent<RectTransform>().sizeDelta.y * transform.localScale.x;

    }

    //gets the current map width
    public float GetMapWidth()
    {
        getMapSize();
        return mapWidth;
    }

    //gets the current map height
    public float GetMapHeight()
    {
        getMapSize();
        return mapHeight;
    }

    //gets the original map width
    public float GetInitialMapWidth()
    {
        return initialMapWidth;
    }

    //gets the original map height
    public float GetInitialMapHeight()
    {
        return initialMapHeight;
    }
}