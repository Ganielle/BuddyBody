using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class swipercamrot : MonoBehaviour
{

    private Touch initTouch = new Touch();

    public TouchField field;
    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f; 


    void FixedUpdate () {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }


    float HandleAxisInputDelegate(string axisName)
    {
        switch(axisName)
        {
            case "Mouse X":
                if (Input.touchCount > 0)
                {
                    return field.TouchDist.x * TouchSensitivity_x * Time.deltaTime;
                }
                else
                {
                    return Input.GetAxis(axisName);
                }
                
            case "Mouse Y":
                if (Input.touchCount > 0)
                {
                    return field.TouchDist.y * TouchSensitivity_y * Time.deltaTime;
                }
                else
                {
                    return Input.GetAxis(axisName);
                }

            default:
                Debug.LogError("Input <"+axisName+"> not recognyzed.",this);
                break;
        }

        return 0f;
    }
}
