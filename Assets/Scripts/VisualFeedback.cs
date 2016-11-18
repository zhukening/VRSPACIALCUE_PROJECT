﻿using UnityEngine;
using System.Collections;

// Written By David Tree - University of Hertfordshire 11/2016

public class VisualFeedback : MonoBehaviour {

    public GameObject VFDown;
    public GameObject VFUp;
    public GameObject VFLeft;
    public GameObject VFRight;

    public FeedbackMain.Directions newDirection;
    private FeedbackMain.Directions currentDirection;
    
    // Use this for initialization
	void Start () {
        VFDown.SetActive(false);
        VFUp.SetActive(false);
        VFLeft.SetActive(false);
        VFRight.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        // update the direction indicator only if  its changed
        if (newDirection != currentDirection)
        {
            UpdateIndicator();
        }

	}

    void UpdateIndicator()
    {
    // turn off old indicator
        switch (currentDirection)
        {
            case FeedbackMain.Directions.Up:
                VFUp.SetActive(false);
                break;
            case FeedbackMain.Directions.Down:
                VFDown.SetActive(false);
                break;
            case FeedbackMain.Directions.Right:
                VFRight.SetActive(false);
                break;
            case FeedbackMain.Directions.Left:
                VFLeft.SetActive(false);
                break;
        }
        // turn on new indicator
        switch (newDirection)
        {
            case FeedbackMain.Directions.Up:
                VFUp.SetActive(true);
                break;
            case FeedbackMain.Directions.Down:
                VFDown.SetActive(true);
                break;
            case FeedbackMain.Directions.Right:
                VFRight.SetActive(true);
                break;
            case FeedbackMain.Directions.Left:
                VFLeft.SetActive(true);
                break;
            case FeedbackMain.Directions.Null:
                // Do nothing the target is visible
                break;
        }
        currentDirection = newDirection;
    }

}