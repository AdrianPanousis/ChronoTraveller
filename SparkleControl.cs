using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

public class SparkleControl : MonoBehaviour
{
    //stores the black mask hiding the particle system
    public Image sparkleMask;
    //stores the particle system
    public ParticleSystem particles;
    //time used slow down the speed and remove the black mask
    private float timer;
    //time used to keep the black mask up
    private float startTimer = 0;
    //controls how fast the black mask fades out
    public float  fadeInSpeed = 1;

    
    
    void Start()
    {
        //gets the particle value and multiplies it's speed by 100 so the sparkles are completely loaded in the background rather than waiting for them to slowly fall
        var main = particles.main;
        main.simulationSpeed = 100;
        
    }

   
    void Update()
    {
        
        //increases the start timer value unitl 5 seconds have passed to hide the sparkles during the opening screen
        startTimer += Time.deltaTime;
        if (startTimer > 5)
        {
            //immediately slows down the particle system to normal speed as it will be visible
            if (timer > 0.05f)
            {
                SlowDownSpeed();
            }

            //starts to show the sparkles by fading out the black mask
            FadeInSparkles();
        }
        
    }

    //sets the particle speed to normal
    private void SlowDownSpeed()
    {
        var main = particles.main;
        main.simulationSpeed = 1;
    }

    //fades out the black mask so the sparkles are no longer hidden
    private void FadeInSparkles()
    {
  
            timer += Time.deltaTime * (1 / fadeInSpeed);
            if (timer < 1.1f)
            {
                sparkleMask.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), timer);
            }
        
    }

  
}
