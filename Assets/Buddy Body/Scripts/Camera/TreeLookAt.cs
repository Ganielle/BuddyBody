using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeLookAt : MonoBehaviour
{
    private Camera cameraTarget;
    Quaternion originalRotation;

    private void Start()
    {
        cameraTarget = GameManager.Instance.mainCamera;
    }

    private void Update(){
        transform.LookAt(transform.position + cameraTarget.transform.rotation * Vector3.left);
    }
}
