using UnityEngine;
using System.Collections;

public class TimeControl : MonoBehaviour
{

    public float speed = 0.1f;
    private enum lightState { Auto = 0, Man = -1 };
    private lightState state = lightState.Auto;
	
	void Update ()
    {
        float rotation = 0;
        if( state == lightState.Auto)
        {
            rotation = speed;
        }
        else if(state == lightState.Man)
        {
            if(Input.GetKey(KeyCode.Q))
            {
                rotation = -speed * 10;
            }
            if(Input.GetKey(KeyCode.E))
            {
                rotation = speed * 10;
            }
        }
        transform.Rotate(new Vector3(0, rotation, 0));
    }

    void OnGUI()
    {
        uint buttonWidth = 80;
        uint buttonHeight = 30;
        uint spacing = 20;
        Rect ButtonRect = new Rect(Screen.width - buttonWidth - spacing, Screen.height - buttonHeight - spacing, buttonWidth, buttonHeight);
        if (GUI.Button(ButtonRect, "Light:" + state.ToString()))
        {
            state = ~state;
        }
    }
}
