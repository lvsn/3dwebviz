using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{

    public float speed = 0.1f;
    private enum lightState { Auto = 0, Man = -1 };
    private lightState state = lightState.Auto;

    void Start()
    {
    }
	
	void Update ()
    {
        float rotation = 0;
        if( state == lightState.Auto)
        {
            if (transform.rotation.eulerAngles.y > 270 || transform.rotation.eulerAngles.y < 120)
            {
                rotation = 10.0f * speed;
            }
            else
            {
                rotation = speed;
            }
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
        Debug.Log(transform.rotation.eulerAngles.y);
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
        if (state == lightState.Man)
        {
            Rect ExplRect = new Rect(Screen.width - buttonWidth - spacing + 7, Screen.height - buttonHeight - 13 - buttonHeight, buttonWidth, buttonHeight);
            GUI.Label(ExplRect, "Use Q & E");
        }
        else
        {

        }
    }
}
