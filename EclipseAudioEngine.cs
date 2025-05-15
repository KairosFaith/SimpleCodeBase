using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
///<summary>Using Pooling for Audio Sources, Do not destroy, Only Release</summary>
public class EclipseAudioEngine : MonoBehaviour
{
    public static EclipseAudioEngine Instance { get; private set; }
    LinkedPool<AudioSource> _Pool;
    const int MaxRealVoices = 32;//change in Project Settings > Audio > MaxRealVoices before changing here
    public SfxMag[] AudioMags = new SfxMag[1];
    Dictionary<string, SfxMag> _SoundBank = new Dictionary<string, SfxMag>();
    //public int ActiveCount, InactiveCount;//for fine tuning MaxRealVoices
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            foreach (SfxMag sfx in AudioMags)
                _SoundBank.Add(sfx.Tag, sfx);
            DontDestroyOnLoad(gameObject);
            _Pool = new LinkedPool<AudioSource>(CreateAudioSource, OnGet, OnRelease, null, true, MaxRealVoices);
        }
        else
            Destroy(gameObject);
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    AudioSource CreateAudioSource()
    {
        GameObject go = new GameObject();
        return go.AddComponent<AudioSource>();
    }
    void OnGet(AudioSource source)
    {
        source.gameObject.SetActive(true);
        source.transform.SetParent(null);
        //InactiveCount = transform.childCount;
        //ActiveCount++;
    }
    void OnRelease(AudioSource source)
    {
        source.Stop();
        source.name = "InactiveAudioSource";
        source.transform.SetParent(transform);
        source.gameObject.SetActive(false);
        //InactiveCount = transform.childCount;
        //ActiveCount--;
    }
    ///<summary>Release all AudioSources when changing scene</summary>
    public void ReleaseAll() => _Pool.Clear();
    public void ReleaseSource(AudioSource source) => _Pool.Release(source);
    IEnumerator _ReleaseAfterDuration(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReleaseSource(source);
    }
    public void ReleaseSource(AudioSource source, float duration) => StartCoroutine(_ReleaseAfterDuration(source, duration));
    public SfxMag FetchSfx(string SoundID)
    {
        if (_SoundBank.TryGetValue(SoundID, out SfxMag data))
            return data;
        else
            throw new Exception(SoundID + " not found");
    }
    public AudioSource GetSource(string SoundId, Vector3 position, float spatialBlend = 1, bool loop = false)
    {
        SfxMag mag = FetchSfx(SoundId);
        AudioSource source = _Pool.Get();
        source.outputAudioMixerGroup = mag.Channel;
        source.clip = mag.FetchRandomClip();
        source.name = SoundId;
        source.transform.position = position;
        source.spatialBlend = spatialBlend;
        source.loop = loop;
        return source;
    }
    public AudioSource PlayClipAtPoint(string SoundId, Vector3 position, float volume = 1, float spatialBlend = 1)
    {
        AudioSource source = GetSource(SoundId, position, spatialBlend);
        AudioClip clip = source.clip;
        source.PlayOneShot(clip, volume);
        ReleaseSource(source, clip.length);
        return source;
    }
}
[Serializable]
public class SfxMag
{
    public string Tag;
    public AudioMixerGroup Channel;
    public AudioClip[] Clips = new AudioClip[1];
    public AudioClip FetchRandomClip()
    {
        int key = UnityEngine.Random.Range(0, Clips.Length);
        AudioClip c = Clips[key];
        return c;
    }
}