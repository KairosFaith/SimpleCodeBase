using UnityEngine;
public class CameraRegionMount : ICameraMount
{
    public Transform MountPoint;
    Vector3 Offset;
    private void Start()
    {
        Offset = MountPoint.position - transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        CameraManager instance = CameraManager.Instance;
        if (instance.SubjectLayer.ContainsLayer(other.gameObject))
            instance.ActivateMount(this);
    }
    public override Vector3 CalculateCameraPosition(Vector3 subjectPostion)
    {
        //move camera based on delta position, from mount point to subject
        return subjectPostion + Offset;
    }
}