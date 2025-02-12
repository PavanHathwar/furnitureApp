﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
public class TapToPlace : MonoBehaviour
{
    
    public GameObject placementIndicator;

    [SerializeField]private ARRaycastManager arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private int count = 0;
    [SerializeField] private GameObject ColourcontrolPanel;
    

    void Start()
    {
        
    }

    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && count==0)
        {
            Controller.instance.PlaceObject(placementPose);
            count = 1;
            ColourcontrolPanel.SetActive(true);
        }
    }
    

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid && count==0)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
        
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCenter, hits);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid && count==0)
        {
            placementPose = hits[0].pose;

            
        }
        
        var cameraForward = Camera.current.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        placementPose.rotation = Quaternion.LookRotation(cameraBearing);
    }
}

