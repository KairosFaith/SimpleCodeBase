using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>Ensure Receiver Object Follows <see cref="IAnimatorKeyframeReceiver"/> Functions</summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimationController : MonoBehaviour
{
    public SpriteAnimationObject[] AnimationList = new SpriteAnimationObject[0];
    SpriteRenderer Renderer;
    Dictionary<string, SpriteAnimationObject> _AnimationBank = new Dictionary<string, SpriteAnimationObject>();
    public bool IsLoop;
    public float FrameDuration;
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        foreach (SpriteAnimationObject animation in AnimationList)
        {
            animation.Construct();
            _AnimationBank.Add(animation.name, animation);
        }
    }
    public void PlayAnimation(string animationName)
    {
        if (_AnimationBank.TryGetValue(animationName, out SpriteAnimationObject animation))
            StartCoroutine(Animate(animation));
        else
            throw new System.Exception($"Animation '{animationName}' not found in the bank.");
    }
    void KeyframeFunction(KeyframeType keyframeType, string message)
    {
        SendMessage($"Receive{keyframeType}Keyframe", message, SendMessageOptions.RequireReceiver);
    }
    IEnumerator Animate(SpriteAnimationObject animation)
    {
        IsLoop = animation.Loop;
        FrameDuration = animation.FrameDuration;
        int frameCount = animation.Frames.Length;
        int currentFrame = 0;
        while (IsLoop)
        {
            Sprite s = animation.FetchFrame(currentFrame, KeyframeFunction);
            if(s!=null)//allow for null frames
                Renderer.sprite = s;
            yield return new WaitForSeconds(FrameDuration);
            currentFrame++;
            if (currentFrame >= frameCount)
            {
                if (IsLoop)
                    currentFrame = 0;
                else
                    break;
            }
        }
    }
}