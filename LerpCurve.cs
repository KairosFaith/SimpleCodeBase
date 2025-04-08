using System;
using UnityEngine;
[Serializable]
public class LerpCurve
{
    public AnimationCurve Curve;
    public float MinInputValue, MaxInputValue, OutputValueA, OutputValueB;
    public float Evaluate(float input)
    {
        float inputT = Mathf.InverseLerp(MinInputValue, MaxInputValue, input);
        float outputT = Curve.Evaluate(inputT);
        return Mathf.LerpUnclamped(OutputValueA, OutputValueB, outputT);
    }
}