using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float Y_ANGLE_MIN = 70.0f;
    public float Y_ANGLE_MAX = -50.0f;

    public Transform lookAt;
    public Transform camTransform;
    public float distance = 10.0f;
    public float minDistance = 2.0f;
    public float maxDistance = 20.0f; 
    public float zoomSpeed = 5.0f; 
    public float blockingRadius = 0.5f;

    private float currentX = 0.0f;
    private float currentY = 0.0f;

    public LayerMask groundLayerMask;

    [Header("Sensitivity Values")]
    public float sensitivityX = 20.0f;
    public float sensitivityY = 20.0f;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            currentX += Input.GetAxis("Mouse X");
            currentY += Input.GetAxis("Mouse Y");

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MAX, Y_ANGLE_MIN);
        }
            
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        float newDistance = distance - scrollWheel * zoomSpeed;
        distance = Mathf.Clamp(newDistance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(-currentY, currentX, 0);
        Vector3 desiredPosition = lookAt.position + rotation * dir;
            
        RaycastHit hit;
        if (Physics.SphereCast(lookAt.position, blockingRadius, desiredPosition - lookAt.position, out hit, distance, groundLayerMask))
        {
            desiredPosition = hit.point - (desiredPosition - lookAt.position).normalized * blockingRadius;
        }

        camTransform.position = desiredPosition;
        camTransform.LookAt(lookAt.position);
    }
}