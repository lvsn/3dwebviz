using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
    
    public int zoomSpeed = 5;
    public int minFOV = 20;
    public int maxFOV = 60;

	void Update ()
    {
	    if(Input.GetKey(KeyCode.Space))
        {
            if (Camera.main.fieldOfView >= minFOV)
            {
                Camera.main.fieldOfView -= zoomSpeed;
            }
        }
        else
        {
            if(Camera.main.fieldOfView <= maxFOV)
            {
                Camera.main.fieldOfView += zoomSpeed;
            }
        }
	}
}

