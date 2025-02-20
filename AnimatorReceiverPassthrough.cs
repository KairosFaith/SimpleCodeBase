using UnityEngine;
using System;
[RequireComponent(typeof(Animator))]
public class AnimatorReceiverPassthrough : MonoBehaviour
{
    public Action<string> OnReceiveKeyframe;
    public void SendKeyframeMessage(string message)
    {
        OnReceiveKeyframe(message);
    }
}