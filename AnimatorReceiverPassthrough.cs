using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimatorReceiverPassthrough : MonoBehaviour
{
    IKeyframeReceiver _Host;
    public void SetHost(IKeyframeReceiver host)
    {
        _Host = host;
    }
    public void SendKeyframeMessage(string message)
    {
        _Host.GetKeyframeMessage(message);
    }
}
public interface IKeyframeReceiver
{
    public void GetKeyframeMessage(string message);
}