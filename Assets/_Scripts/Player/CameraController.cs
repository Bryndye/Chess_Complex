using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform anchorCamera;
    [SerializeField] private Transform mainCamera;

    [Header("Mvt Zoom")]
    [SerializeField] private float verticalSpeed = 50f;
    [SerializeField] private float minDistanceVertical = -5f;
    [SerializeField] private float maxDistanceVertical = 5;

    [Header("Mvt Horizontal")]
    [SerializeField] private float rotationSpeed = 70f;

    [Header("Mvt Zoom")]
    [SerializeField] private float forwardSpeed = 50f;
    [SerializeField] private float minDistance = -5f;
    [SerializeField] private float maxDistance = -20f;

    void Update()
    {
        MoveObjectVertically();
        cameraMouvementHorizontal();
        cameraMovementForward();
    }

    private void cameraMouvementHorizontal()
    {
        float _axeX = Input.GetAxis("Horizontal");

        if (_axeX == 0)
        {
            return;
        }
        float rotationAmount = -_axeX * rotationSpeed * Time.deltaTime;
        Vector3 currentRotation = transform.eulerAngles;
        Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y + rotationAmount, currentRotation.z);
        transform.eulerAngles = newRotation;
    }

    private void cameraMovementForward()
    {
        float _axisZ = Input.GetAxis("Mouse ScrollWheel");
        if (_axisZ == 0)
        {
            return;
        }

        float movementAmount = _axisZ * forwardSpeed * Time.deltaTime;

        // Déplace la caméra sur son axe Z
        Vector3 cameraPosition = mainCamera.localPosition;
        float newZPosition = Mathf.Clamp(cameraPosition.z + movementAmount, maxDistance, minDistance);
        Vector3 newPosition = new Vector3(cameraPosition.x, cameraPosition.y, newZPosition);
        mainCamera.localPosition = newPosition;
    }


    public void MoveObjectVertically()
    {
        float input = Input.GetAxis("Vertical");
        float newY = transform.position.y + input * verticalSpeed * Time.deltaTime;
        newY = Mathf.Clamp(newY, minDistanceVertical, maxDistanceVertical);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
