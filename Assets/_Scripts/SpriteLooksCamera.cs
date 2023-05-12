using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpriteLooksCamera : MonoBehaviour
{
    public Transform cameraTransform; // Référence au transform de la caméra

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        //transform.LookAt(Camera.main.transform);

        Vector3 cameraPosition = cameraTransform.position;
        Vector3 targetPosition = transform.position;


        // Calculer la direction horizontale de l'objet vers la caméra
        Vector3 targetDirection = cameraPosition - targetPosition;
        targetDirection.y = 0f; // Bloquer le mouvement sur l'axe Y
        targetDirection.Normalize();

        // Calculer la rotation en utilisant LookRotation pour faire face à la direction horizontale
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Modifier l'axe X et Y de la rotation actuelle de l'objet
        Quaternion currentRotation = transform.rotation;
        Vector3 eulerRotation = currentRotation.eulerAngles;
        eulerRotation.x = targetRotation.eulerAngles.x;
        eulerRotation.y = targetRotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(eulerRotation);
    }
}

