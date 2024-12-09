using UnityEngine;
public abstract class ICameraMount : MonoBehaviour
{
    public abstract Vector3 CalculateCameraPosition(Vector3 subjectPostion);
}