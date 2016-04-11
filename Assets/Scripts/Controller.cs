using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Keyboard")]

public class Controller : MonoBehaviour {

    public float mouseSensitivityX = 0.5F;
    public float mouseSensitivityY = 0.5F;

    // Keyboard axes buttons in the same order as Unity
    public enum KeyboardAxis { Horizontal = 0, Vertical = 1, None = 3 }

    [System.Serializable]
    // Handles left modifiers keys (Alt, Ctrl, Shift)
    public class Modifiers
    {
        public bool leftAlt;
        public bool leftControl;
        public bool leftShift;

        public bool checkModifiers()
        {
            return (!leftAlt ^ Input.GetKey(KeyCode.LeftAlt)) &&
                (!leftControl ^ Input.GetKey(KeyCode.LeftControl)) &&
                (!leftShift ^ Input.GetKey(KeyCode.LeftShift));
        }
    }

    [System.Serializable]
    // Handles common parameters for translations and rotations
    public class KeyboardControlConfiguration
    {

        public bool activate;
        public KeyboardAxis keyboardAxis;
        public Modifiers modifiers;
        public float sensitivity;

        public bool isActivated()
        {
            return activate && keyboardAxis != KeyboardAxis.None && modifiers.checkModifiers();
        }
    }

    // Yaw default configuration
    public KeyboardControlConfiguration yaw = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Horizontal, modifiers = new Modifiers { leftAlt = true }, sensitivity = 1F };

    // Pitch default configuration
    public KeyboardControlConfiguration pitch = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, modifiers = new Modifiers { leftAlt = true }, sensitivity = 1F };

    // Roll default configuration
    public KeyboardControlConfiguration roll = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Horizontal, modifiers = new Modifiers { leftAlt = true, leftControl = true }, sensitivity = 1F };

    // Vertical translation default configuration
    public KeyboardControlConfiguration verticalTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, modifiers = new Modifiers { leftControl = true }, sensitivity = 0.5F };

    // Horizontal translation default configuration
    public KeyboardControlConfiguration horizontalTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Horizontal, sensitivity = 0.5F };

    // Depth (forward/backward) translation default configuration
    public KeyboardControlConfiguration depthTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, sensitivity = 0.5F };

    // Default unity names for keyboard axes
    public string keyboardHorizontalAxisName = "Horizontal";
    public string keyboardVerticalAxisName = "Vertical";


    private string[] keyboardAxesNames;

    void Start()
    {
        keyboardAxesNames = new string[] { keyboardHorizontalAxisName, keyboardVerticalAxisName };
    }


    // LateUpdate  is called once per frame after all Update are done
    void LateUpdate()
    {
        if (yaw.isActivated())
        {
            float rotationX = Input.GetAxis(keyboardAxesNames[(int)yaw.keyboardAxis]) * yaw.sensitivity;
            transform.Rotate(0, rotationX, 0);
        }
        if (pitch.isActivated())
        {
            float rotationY = Input.GetAxis(keyboardAxesNames[(int)pitch.keyboardAxis]) * pitch.sensitivity;
            transform.Rotate(-rotationY, 0, 0);
        }
        if (roll.isActivated())
        {
            float rotationZ = Input.GetAxis(keyboardAxesNames[(int)roll.keyboardAxis]) * roll.sensitivity;
            transform.Rotate(0, 0, rotationZ);
        }
        if (verticalTranslation.isActivated())
        {
            float translateY = Input.GetAxis(keyboardAxesNames[(int)verticalTranslation.keyboardAxis]) * verticalTranslation.sensitivity;
            transform.Translate(0, translateY, 0);
        }
        if (horizontalTranslation.isActivated())
        {
            float translateX = Input.GetAxis(keyboardAxesNames[(int)horizontalTranslation.keyboardAxis]) * horizontalTranslation.sensitivity;
            transform.Translate(translateX, 0, 0);
        }
        if (depthTranslation.isActivated())
        {
            float translateZ = Input.GetAxis(keyboardAxesNames[(int)depthTranslation.keyboardAxis]) * depthTranslation.sensitivity;
            transform.Translate(0, 0, translateZ);
        }

        // Mouselook
        Quaternion originalRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        float mouseRotationX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseRotationY = Input.GetAxis("Mouse Y") * mouseSensitivityY;
        mouseRotationX = ClampAngle(mouseRotationX, -360F, 360F);
        mouseRotationY = ClampAngle(mouseRotationY, -60F, 60F);
        Quaternion xQuaternion = Quaternion.AngleAxis(mouseRotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(mouseRotationY, -Vector3.right);
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;

        // Restrict view toward the wall
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x < 180 ?
                Mathf.Clamp(transform.rotation.eulerAngles.x, 0F, 60F) :
                Mathf.Clamp(transform.rotation.eulerAngles.x, 300F, 360F),
            Mathf.Clamp(transform.rotation.eulerAngles.y, 100F, 260F),
            Mathf.Clamp(transform.rotation.eulerAngles.z, 0F, 0F)
        );

        // Restrict movement around the wall front
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -2F, 10F),
            Mathf.Clamp(transform.position.y, 0F, 5F),
            Mathf.Clamp(transform.position.z, -1F, 6F)
        );

    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
         angle += 360F;
        if (angle > 360F)
         angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
