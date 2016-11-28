using UnityEngine;
using System.IO.Ports;				//Serial Port in C#
using System.Collections;

public class VibroHapticFeedback : MonoBehaviour {

    public FeedbackMain.Directions newDirection;
    private FeedbackMain.Directions currentDirection;

	//need to change the name of the serial port everytime the Arduino is connected
	public static SerialPort sp = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
    
	// Use this for initialization
    void Start () {
		OpenConnection ();
	}

	// Need to close sp when the level is finished. What is the callback?

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
				//sp.WriteLine("0");
                break;
            case FeedbackMain.Directions.Down:
                // Stop Vibration motor for Down Direction
				//sp.WriteLine("0");
                break;
            case FeedbackMain.Directions.Right:
                // Stop Vibration motor for Right Direction
				//sp.WriteLine("0");
                break;
            case FeedbackMain.Directions.Left:
                // Stop Vibration motor for Left Direction
                break;
			case FeedbackMain.Directions.UpLeft:
				// Stop Vibration motor for Up-Left Direction
				//sp.WriteLine("0");
				break;
			case FeedbackMain.Directions.UpRight:
				// Stop Vibration motor for Up-Right Direction
				//sp.WriteLine("0");
				break;
			case FeedbackMain.Directions.DownLeft:
				// Stop Vibration motor for Down-Left Direction
				//sp.WriteLine("0");
				break;
			case FeedbackMain.Directions.DownRight:
				// Stop Vibration motor for Down-Right Direction
				//sp.WriteLine("0");
				break;
        }
        // turn on new indicator
        switch (newDirection)
        {
            case FeedbackMain.Directions.Up:
                // Trigger Vibration motor for Up Direction
				sp.WriteLine("8");
                break;
            case FeedbackMain.Directions.Down:
                // Trigger Vibration motor for Down Direction
				sp.WriteLine("4");
                break;
            case FeedbackMain.Directions.Right:
                // Trigger Vibration motor for Right Direction
				sp.WriteLine("6");
                break;
            case FeedbackMain.Directions.Left:
                // Trigger Vibration motor for Left Direction
				sp.WriteLine("2");
                break;
			case FeedbackMain.Directions.UpLeft:
				// Trigger Vibration motor for Up-left Direction
				sp.WriteLine("1");
				break;
			case FeedbackMain.Directions.UpRight:
				// Trigger Vibration motor for UpRight Direction
				sp.WriteLine("7");
				break;
			case FeedbackMain.Directions.DownLeft:
				// Trigger Vibration motor for DownLeft Direction
				sp.WriteLine("3");
				break;
			case FeedbackMain.Directions.DownRight:
				// Trigger Vibration motor for DownRight Direction
				sp.WriteLine("5");
				break;
            case FeedbackMain.Directions.Null:
                // Do nothing the target is visible
				sp.WriteLine("0");
                break;
        }
        currentDirection = newDirection;
    }

	//Open Arduino Serial Port
	//Function connecting to Arduino
	public void OpenConnection()
	{
        // needs protection if arduino not connected.

		if (sp != null) {
			if (sp.IsOpen) {
				sp.Close ();
				print("Closing port, because it was already open!");
			} else {
				sp.Open ();  // opens the connection
				sp.ReadTimeout = 50;  // sets the timeout value before reporting error
				print("Port Opened!");
			}
		} else {
			if (sp.IsOpen) {
				print ("Port is already open");
			} else {
				print ("Port == null");
			}
		}
	}

    public void closeConnection()
    {
        sp.Close();
    }

}
