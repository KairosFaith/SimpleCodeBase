using System;
using System.Collections.Generic;
using UnityEngine;
public static class flexEngine
{
    static Dictionary<string, anClipMag> _SoundBank = new Dictionary<string, anClipMag>();
    static flexEngine()
    {
        string FilePath = "AudioMags";
        anClipMag[] m = Resources.LoadAll<anClipMag>(FilePath);
        foreach (anClipMag b in m)
            _SoundBank.Add(b.name, b);
    }
    public static anClipMag FetchMag(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out anClipMag mag))
            return mag;
        else
            throw new System.Exception(SoundID + " not found");
    }
    public static AudioClip FetchClip(string SoundID, out float gain)
    {
        if (_SoundBank.TryGetValue(SoundID, out anClipMag mag))
            return mag.Randomise(out gain);
        else
            throw new System.Exception(SoundID + " not found");
    }
    public static AudioClip PlayClipFromSource(string SoundID, AudioSource source)
    {
        if (_SoundBank.TryGetValue(SoundID, out anClipMag mag))
        {
            AudioClip c = mag.Randomise(out float gain);
            source.PlayOneShot(c, gain);
            return c;
        }
        else
            throw new System.Exception(SoundID + " not found");
    }
    public static AudioSource PlayClipAtPoint(string SoundID, Vector3 position, Action<anSourcerer> beforePlay = null)
    {
        if (_SoundBank.TryGetValue(SoundID, out anClipMag mag))
        {
            anClipObjectMag m = (anClipObjectMag)mag;
            return m.PlayClipAtPoint(position, beforePlay);
        }
        else
            throw new System.Exception(SoundID + " not found");
    }
}
