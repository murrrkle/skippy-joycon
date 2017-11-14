using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkIt;
using System;
using System.IO;

public class JoyConTracker : MonoBehaviour {
	
    private Joycon j;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public Quaternion orientation;
	public float factor;
	// NetworkIt
	public NetworkItClient networkInterface;
	public bool deliverToSelf = false;

	// Get Accel
	private float maxAccel;

    void Start ()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        // get the public Joycon object attached to the JoyconManager in scene
        j = JoyconManager.Instance.j;
		maxAccel = 0;

	}

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonDown("Jump")) {
			SendThrow (factor);
		}
		// make sure the Joycon only gets checked if attached
        if (j != null && j.state > Joycon.state_.ATTACHED)
        {
			// GetButtonDown checks if a button has been pressed (not held)
            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
				maxAccel = 0;
//				Debug.Log ("Shoulder button 2 pressed");
//				GetStick returns a 2-element vector with x/y joystick components
//				Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}",j.GetStick()[0],j.GetStick()[1]));

			}
			// GetButtonDown checks if a button has been released
			if (j.GetButtonUp (Joycon.Button.SHOULDER_2))
			{
//				Debug.Log ("Shoulder button 2 released");
				SendThrow (maxAccel);
			}
			// GetButtonDown checks if a button is currently down (pressed or held)
			if (j.GetButton (Joycon.Button.SHOULDER_2))
			{
				accel = j.GetAccel();
				float tmp = Mathf.Max(new float[]{Math.Abs(accel.x), Math.Abs(accel.y), Math.Abs(accel.z)});
				if (tmp > maxAccel) {
					maxAccel = tmp;
				}
//				Debug.Log ("Shoulder button 2 held");
			}

			if (j.GetButtonDown (Joycon.Button.DPAD_DOWN)) {
				// Joycon has no magnetometer, so it cannot accurately determine its yaw value. Joycon.Recenter allows the user to reset the yaw value.
				j.Recenter ();

				//Debug.Log ("Rumble");

				// Rumble for 200 milliseconds, with low frequency rumble at 160 Hz and high frequency rumble at 320 Hz. For more information check:
				// https://github.com/dekuNukem/Nintendo_Switch_Reverse_Engineering/blob/master/rumble_data_table.md

//				j.SetRumble (160, 320, 0.6f, 200);

				// The last argument (time) in SetRumble is optional. Call it with three arguments to turn it on without telling it when to turn off.
                // (Useful for dynamically changing rumble values.)
				// Then call SetRumble(0,0,0) when you want to turn it off.
			}

            stick = j.GetStick();

            // Gyro values: x, y, z axis values (in radians per second)
            gyro = j.GetGyro();

            // Accel values:  x, y, z axis values (in Gs)
            accel = j.GetAccel();

            orientation = j.GetVector();
            gameObject.transform.rotation = orientation;
        }
    }
	public void SendThrow(float maxAccel)
	{
		//TODO your code here
		Message m = new Message("Throw");
		m.DeliverToSelf = deliverToSelf;
		m.AddField("accel", "" + maxAccel);
		networkInterface.SendMessage(m);
		Debug.Log ("Message Sent (Throw): accel=" + maxAccel);
	}

	public void NetworkIt_Message(object m) {
		Message message = (Message)m;
		if (message.Subject.Equals ("Vibrate")) {
			int bounce = 0;
			int.TryParse(message.GetField("bounce"), out bounce);
			j.SetRumble (20*bounce, 40*bounce, 0.6f, 100);
		}
	}
}