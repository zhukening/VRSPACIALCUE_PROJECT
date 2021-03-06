﻿using UnityEngine;
using System.Collections;

// Written By David Tree - University of Hertfordshire 11/2016

public class FeedbackMain : MonoBehaviour {

    public GameObject target;

    // store the current Experiment
    private ExperimentData.Experiment CurrentExperiment;
    
    // standard directions enum
    public enum Directions { Up, Down, Left, Right, UpLeft, UpRight, DownLeft, DownRight, Null }

    // private variables
    private Directions TargetDirection;
    private VisualFeedback VisualFeedbackScript;
    private VibroHapticFeedback VibroHapticFeebackScript;
    private float FOV = 0.7f; 

    private Component MouseControls;

    // Use this for initialization
    void Start () {

        // get experiment Type from persistantGO
        GameObject PersistantGameObject = GameObject.Find("persistantGO");
        CurrentExperiment = PersistantGameObject.GetComponent<ExperimentData>().GetCurrentExperiment();

        if (!UnityEngine.VR.VRDevice.isPresent)
        {
            // if a headset is not plugged in add the mouse look script
            MouseLook MouseControls = Camera.main.gameObject.AddComponent<MouseLook>();
            // set maximum and minimum Y axis to full range
            MouseControls.maximumY = 90F;
            MouseControls.minimumY = -90F;
        }

        // disable the audio listener as default
        AudioListener.pause = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (target != null)
        {
            TargetDirection = getDirection();
        }
        updateFeedbackSystem();
    }

    // Get the direction of target based on camera forward vector
    private Directions getDirection ()
    {

        // Get position transform as V3
        Vector3 relativePosition = target.transform.position - Camera.main.transform.position;
        Vector3 heading = Vector3.Normalize(relativePosition);

        // get rotational transform 
        Vector3 directionVector = Vector3.Cross(Camera.main.transform.forward, heading);

        float distanceToTurn = Vector3.Dot(Camera.main.transform.forward, heading); // whether to display the UI

        if (distanceToTurn < FOV)
        {
            if (directionVector.x > FOV + 0.01 || directionVector.x < -FOV - 0.01) // if user is looking up too high
            {
                if (directionVector.x > FOV)
                {
                    return (Directions.Down);
                }
                else
                {
                    return (Directions.Up);
                }

            }
            else
            {
                // set direction of target
                // need to add additional triggers for enumeration
                if (directionVector.y > 0)
                {
                    if (directionVector.y < 0.12) // 180-159 degrees = 21 degrees
                    {
                        return (Directions.Down); 
                    }
                    else if (directionVector.y < 0.375) 
                    {
                        return (Directions.DownRight);
                    }
                    if (directionVector.y < 0.625)
                    {
                        return (Directions.Right);
                    }
                    else
                    {
                        return (Directions.UpRight); // cleanup
                    }
                }
                else
                {
                    if (directionVector.y > -0.12) 
                    {
                        return (Directions.Down);
                    }
                    else if (directionVector.y > -0.375)
                    {
                        return (Directions.DownLeft);
                    }
                    else if (directionVector.y > -0.625)
                    {
                        return (Directions.Left);
                    }
                    else
                    {
                        return (Directions.UpLeft); // cleanup
                    }
                }
            }
        }
        else
        {
            return (Directions.Null);
        }
    }

    // Function to update all feedback systems selected
    // n.b. all feedback systems should accept targetDirection of type FeedbackMain.Directions
    void updateFeedbackSystem()
    {
        switch (CurrentExperiment)
        {
            case ExperimentData.Experiment.Visual:
                {
                    // trigger update function in visual feedback system
                    VisualFeedbackScript = GetComponentInChildren<VisualFeedback>();  // get the feedback Script
                    VisualFeedbackScript.newDirection = TargetDirection; // update its target direction
                    break;
                }
            case ExperimentData.Experiment.Audio:
                {
                    AudioListener.pause = false;
                    break;
                }
            case ExperimentData.Experiment.VibroHaptic:
                {
                    // Trigger update function in vibration feedback system
                    VibroHapticFeebackScript = GetComponentInChildren<VibroHapticFeedback>();  // get the feedback Script
                    VibroHapticFeebackScript.newDirection = TargetDirection;
                    break;
                }
            case ExperimentData.Experiment.NULL:
                {
                    break;
                }

        }
    }
}
