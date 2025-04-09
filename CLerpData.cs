using System;
using UnityEngine;
///<summary>
///Ian Ngoh - Curve Linear Interpolation, Yes I made this up
///(Or does Cerp sound better hmmmmm)
/// </summary>
[Serializable]
public class CLerpData
{
    public AnimationCurve Curve;
    public float MinInputValue, MaxInputValue, MinOutputValue, MaxOutputValue;
    public float Evaluate(float input)
    {
        float inputT = Mathf.InverseLerp(MinInputValue, MaxInputValue, input);
        float outputT = Curve.Evaluate(inputT);
        return Mathf.LerpUnclamped(MinOutputValue, MaxOutputValue, outputT);
    }
}