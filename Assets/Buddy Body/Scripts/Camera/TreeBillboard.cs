using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBillboard : MonoBehaviour
{
    [SerializeField] private float distanceToCamera = 2;
    private Camera cameraTarget;
    Quaternion originalRotation;

    private void Start()
    {
        cameraTarget = GameManager.Instance.mainCamera;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, cameraTarget.transform.position) > distanceToCamera)
        {
            transform.LookAt(transform.position + cameraTarget.transform.rotation * Vector3.left);
        }
    }
}
