using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>Ensure Receiver Object Follows <see cref="IAnimatorKeyframeReceiver"/> Functions</summary>
public class SpriteAnimationController : MonoBehaviour
{
    public SpriteAnimationObject[] AnimationList = new SpriteAnimationObject[1];
    Action<Sprite> _OnSpriteChange;
    Dictionary<string, SpriteAnimationObject> _AnimationBank = new Dictionary<string, SpriteAnimationObject>();
    Coroutine _CurrentRoutine;
    public bool IsLoop;
    public float FrameDuration;
    //private void Start()
    //{
    //    SpriteRenderer renderer = GetComponent<SpriteRenderer>();
    //    Initialize((s) => renderer.sprite = s);
    //}
    public void Initialize(Action<Sprite> onSpriteChange)
    {
        _OnSpriteChange = onSpriteChange;
        foreach (SpriteAnimationObject animation in AnimationList)
        {
            animation.Construct();
            _AnimationBank.Add(animation.name, animation);
        }
        PlayAnimation(AnimationList[0]);
    }
    public void PlayAnimation(string animationName)
    {
        if (_AnimationBank.TryGetValue(animationName, out SpriteAnimationObject animation))
            PlayAnimation(animation);
        else
            throw new System.Exception($"Animation '{animationName}' not found in the bank.");
    }
    public void PlayAnimation(SpriteAnimationObject animation)
    {
        StopAnimation();
        _CurrentRoutine = StartCoroutine(Animate(animation));
    }
    public void StopAnimation()
    {
        if (_CurrentRoutine != null)
            StopCoroutine(_CurrentRoutine);
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
            if (s != null)//allow for null frames
                _OnSpriteChange(s);
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
    void KeyframeFunction(KeyframeType keyframeType, string message)
    {
        SendMessage($"Receive{keyframeType}Keyframe", message, SendMessageOptions.RequireReceiver);
    }
}