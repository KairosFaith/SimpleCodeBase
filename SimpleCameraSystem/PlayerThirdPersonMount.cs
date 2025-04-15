using UnityEngine;
public class PlayerThirdPersonMount : ICameraMount
{
    public Transform CameraPoint;
    public Transform Subject;
    public float rotationSpeed = 5, LerpSpeed, SlerpSpeed;
    private void Update()
    {
        transform.position = Subject.position;
    }
    public override Vector3 UpdateCameraPosition(Transform cameraTransform, Vector3 subjectPostion, out Quaternion outRotation)
    {
        outRotation = Quaternion.Slerp(cameraTransform.rotation, CameraPoint.rotation, SlerpSpeed*Time.deltaTime);
        return Vector3.Lerp(cameraTransform.position,CameraPoint.position,LerpSpeed*Time.deltaTime);
    }
    public void Rotate(Vector2 input)
    {
        Vector3 look = new Vector3(-input.y, input.x, 0);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = currentRotation + rotationSpeed * Time.deltaTime * look;
        transform.rotation = Quaternion.Euler(targetRotation);
    }
}