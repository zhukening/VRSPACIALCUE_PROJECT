﻿using UnityEngine;
using System.Collections;

// Written By David Tree - University of Hertfordshire 11/2016

public class FeedbackMain : MonoBehaviour {

    public GameObject target;

    // configure experiment
    public bool VFeedbackEnabled;
    public bool AFeedbackEnabled;
    public bool VbFedbackEnabled;


    // standard directions enum
    public enum Directions { Up, Down, Left, Right, Null }

    // private variables
    private Directions TargetDirection;
    private VisualFeedback VisualFeedbackScript;
    private VibroHapticFeedback VibroHapticFeebackScript;
    private float FOV = 0.7f; // 

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        TargetDirection = getDirection();
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
        
        // calculate the magnitude of the turn - For measuring how far they had to move
        float AngleMag = Vector3.Angle(Camera.main.transform.forward, heading);

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
                if (directionVector.y > 0)
                {
                    //turn right
                    return (Directions.Right);
                }
                else
                {
                    // turn Left
                    return (Directions.Left);
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
        if (VFeedbackEnabled)
        {
            // trigger update function in visual feedback system
            VisualFeedbackScript = GetComponentInChildren<VisualFeedback>();  // get the feedback Script
            VisualFeedbackScript.newDirection = TargetDirection; // update its target direction
        }

        if (AFeedbackEnabled)
        {

            // trigger update function in audio feedback system
        }

        if (VbFedbackEnabled)
        {
            // Trigger update function in vibration feedback system
            VibroHapticFeebackScript= GetComponentInChildren<VibroHapticFeedback>();  // get the feedback Script

        }
    }
}
