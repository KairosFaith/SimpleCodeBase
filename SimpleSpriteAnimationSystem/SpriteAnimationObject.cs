using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpriteAnimationObject", menuName = "SpriteAnimationObject")]
public class SpriteAnimationObject : ScriptableObject
{
    public float FrameDuration = .1f;
    public bool Loop;
    /// <summary>
    ///<see cref="SpriteAnimationController"> allows for null frames
    /// </summary>
    public Sprite[] Frames = new Sprite[2];
    public List<SpriteAnimationKeyframe> Keyframes = new List<SpriteAnimationKeyframe>();
    Dictionary<int, (KeyframeType, string)> _KeyframeLookup;
    public void Construct()
    {
        if (_KeyframeLookup == null)
        {
            _KeyframeLookup = new Dictionary<int, (KeyframeType, string)>();
            foreach (SpriteAnimationKeyframe keyframe in Keyframes)
                _KeyframeLookup[keyframe.FrameIndex] = (keyframe.Type, keyframe.Message);
        }
        else
            throw new Exception("SpriteAnimationData already constructed. Do not construct twice");
    }
    public Sprite FetchFrame(int index, Action<KeyframeType, string> keyframeFunction)
    {
        if (_KeyframeLookup.TryGetValue(index, out (KeyframeType, string) value))
            keyframeFunction(value.Item1, value.Item2);
        return Frames[index];
    }
}
[Serializable]
public class SpriteAnimationKeyframe
{
    public int FrameIndex;
    public KeyframeType Type;
    public string Message;
}
public enum KeyframeType
{
    Control,
    Audio,
}