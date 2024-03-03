using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;

public class AnalyticsUtils : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            AnalyticsService.Instance.StartDataCollection();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
