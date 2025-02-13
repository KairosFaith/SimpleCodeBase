using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class EclipseAudioEngine : MonoBehaviour
{
    public static EclipseAudioEngine Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSoundBank();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public SfxMag[] AudioMags;
    Dictionary<string, SfxMag> _SoundBank = new Dictionary<string, SfxMag>();
    void LoadSoundBank()
    {
        foreach (var sfx in AudioMags)
        {
            var tag = sfx.Tag;
            if (!_SoundBank.ContainsKey(tag))
                _SoundBank.Add(tag, sfx);
        }
    }
    public SfxMag FetchSfx(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out SfxMag data))
            return data;
        else
            throw new Exception(SoundID + " not found");
    }
    public AudioClip FetchSfx(string SoundID, out float gain, out AudioMixerGroup channel)
    {
        SfxMag mag = FetchSfx(SoundID);
        channel = mag.Channel;
        return mag.Randomise(out gain);
    }
    public AudioSource PlayClipAtPoint(string SoundId, Vector3 position)
    {
        GameObject go = new GameObject(SoundId);
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.spatialBlend = 1;

        AudioClip clip = FetchSfx(SoundId, out float gain, out AudioMixerGroup channel);
        source.outputAudioMixerGroup = channel;
        source.PlayOneShot(clip, gain);
        Destroy(go, clip.length);
        return source;
    }
}
[Serializable]
public class SfxMag
{
    public string Tag;
    public AudioMixerGroup Channel;
    [Range(0, 1)]
    public float MinRandomVolume = 1;
    public AudioClip[] Clips;
    public AudioClip Randomise(out float gain)
    {
        int key = UnityEngine.Random.Range(0, Clips.Length);
        AudioClip c = Clips[key];
        gain = UnityEngine.Random.Range(MinRandomVolume, 1);
        return c;
    }
}