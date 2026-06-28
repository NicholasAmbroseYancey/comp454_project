using UnityEngine;

public class VRBodyFollow : MonoBehaviour
{
    [SerializeField] private Transform vrCamera;
    [SerializeField] private float positionOffset_Y = 0f;
    [SerializeField] private float positionOffset_Z = -0.1f;

    void LateUpdate()
    {
        if (vrCamera == null) return;
        Vector3 cameraRotation = vrCamera.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(0, cameraRotation.y, 0);
        transform.rotation = targetRotation;

        Vector3 targetPosition = vrCamera.position;
        Vector3 bodyOffset = targetRotation * new Vector3(0, 0, positionOffset_Z);
        targetPosition += bodyOffset;
        targetPosition.y = transform.parent.position.y + positionOffset_Y; 
        transform.position = targetPosition;
    }
}