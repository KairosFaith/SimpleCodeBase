using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AnimatorKeyframePassthrough : MonoBehaviour
{
    public IAnimatorKeyframeReceiver Receiver;
    public void ControlKeyframe(string message)
    {
        Receiver.ReceiveControlKeyframe(message);
    }
    public void AudioKeyframe(string message)
    {
        Receiver.ReceiveAudioKeyframe(message);
    }
}
public interface IAnimatorKeyframeReceiver
{
    void ReceiveControlKeyframe(string message);
    void ReceiveAudioKeyframe(string message);
}