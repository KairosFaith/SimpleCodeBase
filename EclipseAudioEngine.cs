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
            foreach (SfxMag sfx in AudioMags)
                _SoundBank.Add(sfx.Tag, sfx);
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public SfxMag[] AudioMags;
    Dictionary<string, SfxMag> _SoundBank = new Dictionary<string, SfxMag>();
    public SfxMag FetchSfx(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out SfxMag data))
            return data;
        else
            throw new Exception(SoundID + " not found");
    }
    public AudioClip FetchSfx(string SoundID, out AudioMixerGroup channel)
    {
        SfxMag mag = FetchSfx(SoundID);
        channel = mag.Channel;
        return mag.FetchRandomClip();
    }
    AudioSource CreateAudioSource(string SoundId, AudioMixerGroup channel, Vector3 position)
    {
        GameObject go = new GameObject(SoundId);
        go.transform.position = position;
        AudioSource source = go.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = channel;
        source.spatialBlend = 1;
        return source;
    }
    public AudioSource PlayClipAtPoint(string SoundId, Vector3 position, float volume = 1f)
    {
        AudioClip clip = FetchSfx(SoundId, out AudioMixerGroup channel);
        AudioSource source = CreateAudioSource(SoundId, channel, position);
        source.PlayOneShot(clip, volume);
        Destroy(source.gameObject, clip.length);
        return source;
    }
}
[Serializable]
public class SfxMag
{
    public string Tag;
    public AudioMixerGroup Channel;
    public AudioClip[] Clips;
    public AudioClip FetchRandomClip()
    {
        int key = UnityEngine.Random.Range(0, Clips.Length);
        AudioClip c = Clips[key];
        return c;
    }
}