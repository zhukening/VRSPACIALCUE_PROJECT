using UnityEngine;
using System.Collections;

public class VibroHapticFeedback : MonoBehaviour {

    public FeedbackMain.Directions newDirection;
    private FeedbackMain.Directions currentDirection;

    // Use this for initialization
    void Start () {
	
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
                // Stop Vibration motor for Up Direction
                break;
            case FeedbackMain.Directions.Down:
                // Stop Vibration motor for Down Direction
                break;
            case FeedbackMain.Directions.Right:
                // Stop Vibration motor for Right Direction
                break;
            case FeedbackMain.Directions.Left:
                // Stop Vibration motor for Left Direction
                break;
        }
        // turn on new indicator
        switch (newDirection)
        {
            case FeedbackMain.Directions.Up:
                // Trigger Vibration motor for Up Direction
                break;
            case FeedbackMain.Directions.Down:
                // Trigger Vibration motor for Down Direction
                break;
            case FeedbackMain.Directions.Right:
                // Trigger Vibration motor for Right Direction
                break;
            case FeedbackMain.Directions.Left:
                // Trigger Vibration motor for Left Direction
                break;
            case FeedbackMain.Directions.Null:
                // Do nothing the target is visible
                break;
        }
        currentDirection = newDirection;
    }
}
