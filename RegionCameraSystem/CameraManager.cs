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
    public Transform Subject;
    public float LerpSpeed;
    public LayerMask SubjectLayer;
    public void SetSubject(Transform subject)
    {
        Subject = subject;
    }
    public void ActivateMount(ICameraMount mount)
    {
        CurrentMount = mount;
        //onActivate?
    }
    private void Update()
    {
        if (CurrentMount == null || Subject == null)
            return;
        Vector3 targetPos = CurrentMount.CalculateCameraPosition(Subject.position);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * LerpSpeed);
    }
}