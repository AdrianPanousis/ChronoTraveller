using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveLoadingBars : MonoBehaviour
{
    [SerializeField] private LevelDownloader[] LoadingBars;
    
    
    public void BarStatus(bool isActive)
    {
        //checks all the loading bars
        for(int i = 0; i<LoadingBars.Length;i++)
        {
            //checks if the level is not currently downloading
            if (!LoadingBars[i].showDownloadProgress)
            {
                LoadingBars[i].SliderImagesFadeOut();
            }

            //if the level is downloading
            else
            {
                //if the downloading bar is to be displayed
                if (isActive)
                {
                    
                    LoadingBars[i].SliderImagesFadeIn();
                }
                //if the download bar is to be removed
                else
                {
                   
                    LoadingBars[i].SliderImagesFadeOut();
                }
            }
        }

       
        
    
    }

   







    }
