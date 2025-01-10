using UnityEngine;
public class PlayerThirdPersonMount : ICameraMount
{
    public Transform MountPoint;
    public Transform Subject;
    public float rotationSpeed = 5;
    private void Start()
    {
        CameraManager.Instance.ActivateMount(this);
    }
    private void Update()
    {
        transform.position = Subject.position;
    }
    public override Vector3 CalculateCameraPosition(Transform cameraTransform, Vector3 subjectPostion, out Quaternion outRotation)
    {
        outRotation = MountPoint.rotation;
        return MountPoint.position;
    }
    public void Rotate(Vector2 input)
    {
        Vector3 look = new Vector3(-input.y, input.x, 0);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        Vector3 targetRotation = currentRotation + look * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(targetRotation);
        //transform.Rotate(look * rotationSpeed * Time.deltaTime);
    }
}