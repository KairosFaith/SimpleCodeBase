using UnityEngine;
public static class CameraAlignedMovement
{
    public static Vector3 CalculatePlayerMovement(this Camera camera, Vector2 inputAxis)
    {
        return camera.transform.CalculatePlayerMovement(inputAxis);
    }
    static Vector3 CalculatePlayerMovement(this Transform cameraTransform, Vector2 inputAxis)
    {
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 targetDirection = forward * inputAxis.y + right * inputAxis.x;
        targetDirection.Normalize();
        return targetDirection;
    }
}