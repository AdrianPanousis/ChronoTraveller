using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using NatSuite.Devices;

public class GetPermissions : MonoBehaviour
{
    bool granted;
    async void Start()
    {
        // Request camera permissions
        granted = await MediaDeviceQuery.RequestPermissions<ICameraDevice>();

        //Request location permission for Android devices
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif


    }

   
}
