using UnityEngine;
[RequireComponent(typeof(Camera))]//attach this to main camera
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance =  this;
        else
            Destroy(this);
    }
    private void OnDestroy()
    {
        if(Instance == this)
           Instance =  null;
    }
    public ICameraMount CurrentMount;
    public Transform Subject;
    public LayerMask SubjectLayer;
    private void LateUpdate()
    {
        transform.position = CurrentMount.UpdateCameraPosition(transform, Subject.position, out Quaternion rotation);
        transform.rotation = rotation;
    }
}
public abstract class ICameraMount : MonoBehaviour
{
    public abstract Vector3 UpdateCameraPosition(Transform cameraTransform, Vector3 subjectPostion, out Quaternion outRotation);
}