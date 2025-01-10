using UnityEngine;
[RequireComponent(typeof(Camera))]//attach this to main camera
public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        if(Instance==this)
           Instance = null;
    }
    ICameraMount CurrentMount;
    Transform _Subject;
    public LayerMask SubjectLayer;
    public void SetSubject(Transform subject)
    {
        _Subject = subject;
    }
    public void ActivateMount(ICameraMount mount)
    {
        CurrentMount = mount;
        //onActivate?
    }
    private void LateUpdate()
    {
        if (CurrentMount == null || _Subject == null)
            return;
        transform.position = CurrentMount.CalculateCameraPosition(transform, _Subject.position, out Quaternion rotation);
        transform.rotation = rotation;
    }
}
public abstract class ICameraMount : MonoBehaviour
{
    public abstract Vector3 CalculateCameraPosition(Transform cameraTransform, Vector3 subjectPostion, out Quaternion outRotation);
}