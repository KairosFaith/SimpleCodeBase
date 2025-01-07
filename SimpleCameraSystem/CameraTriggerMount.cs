using UnityEngine;
public class CameraTriggerMount : ICameraMount
{
    public Transform MountPoint;
    public float LerpSpeed = 5;
    Vector3 _Offset;
    private void Start()
    {
        _Offset = MountPoint.position - transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        CameraManager instance = CameraManager.Instance;
        if (instance.SubjectLayer.ContainsLayer(other.gameObject))
            instance.ActivateMount(this);
    }
    public override Vector3 CalculateCameraPosition(Transform cameraTransform, Vector3 subjectPostion, out Quaternion outRotation)
    {
        //move camera based on delta position, from mount point to subject
        Vector3 targetPos = subjectPostion + _Offset;
        float lerpValue = Time.deltaTime * LerpSpeed;
        outRotation = Quaternion.Lerp(cameraTransform.rotation, MountPoint.rotation, lerpValue);
        return Vector3.Lerp(cameraTransform.position, targetPos, lerpValue);
    }
}