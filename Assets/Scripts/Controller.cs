using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Keyboard")]

public class Controller : MonoBehaviour {

    // Keyboard axes buttons in the same order as Unity
    public enum KeyboardAxis { Horizontal = 0, Vertical = 1, None = 3 }


    [System.Serializable]
    // Handles common parameters for translations and rotations
    public class KeyboardControlConfiguration
    {

        public bool activate;
        public KeyboardAxis keyboardAxis;
        public float sensitivity;

        public bool isActivated()
        {
            return activate && keyboardAxis != KeyboardAxis.None;
        }
    }

    // Horizontal translation default configuration
    public KeyboardControlConfiguration horizontalTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Horizontal, sensitivity = 0.5F };

    // Depth (forward/backward) translation default configuration
    public KeyboardControlConfiguration depthTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, sensitivity = 0.5F };

    public KeyboardControlConfiguration mouseRotation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, sensitivity = 0.5F };
    public KeyboardControlConfiguration mouseTranslation = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, sensitivity = 0.5F };
    public KeyboardControlConfiguration mouseScroll = new KeyboardControlConfiguration { keyboardAxis = KeyboardAxis.Vertical, sensitivity = 0.5F };

    // Default unity names for keyboard axes
    private string keyboardHorizontalAxisName = "Horizontal";
    private string keyboardVerticalAxisName = "Vertical";


    private string[] keyboardAxesNames;

    void Start()
    {
        keyboardAxesNames = new string[] { keyboardHorizontalAxisName, keyboardVerticalAxisName };
    }


    // LateUpdate  is called once per frame after all Update are done
    void LateUpdate()
    {
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
        if(mouseRotation.isActivated())
        {
            if (Input.GetMouseButton(0))
            {
                float displacementX = Input.GetAxis("Mouse X") * mouseRotation.sensitivity;
                float displacementY = Input.GetAxis("Mouse Y") * mouseRotation.sensitivity;
                displacementX = ClampAngle(displacementX, -360F, 360F);
                displacementY = ClampAngle(displacementY, -60F, 60F);
                Quaternion xQuaternion = Quaternion.AngleAxis(displacementX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis(displacementY, -Vector3.right);
                transform.localRotation = transform.rotation * xQuaternion * yQuaternion;
            }
        }
        if(mouseTranslation.isActivated())
        {
            if (Input.GetMouseButton(1))
            {
                float displacementX = Input.GetAxis("Mouse X") * mouseTranslation.sensitivity;
                float displacementY = Input.GetAxis("Mouse Y") * mouseTranslation.sensitivity;
                transform.Translate(displacementX, displacementY, 0);
            }
        }
        if(mouseScroll.isActivated())
        {
            float displacementZ = Input.GetAxis("Mouse ScrollWheel") * mouseScroll.sensitivity;
            transform.Translate(0, 0, displacementZ);
        }

        ApplyRotationRestriction();
        ApplyTranslationRestriction();

    }

    private void ApplyRotationRestriction()
    {
        //Todo : it will depend on models!
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x < 180 ?
                Mathf.Clamp(transform.rotation.eulerAngles.x, 0F, 60F) :
                Mathf.Clamp(transform.rotation.eulerAngles.x, 300F, 360F),
            Mathf.Clamp(transform.rotation.eulerAngles.y, 100F, 260F),
            Mathf.Clamp(transform.rotation.eulerAngles.z, 0F, 0F)
        );
    }

    private void ApplyTranslationRestriction()
    {
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
