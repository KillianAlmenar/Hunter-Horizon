using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    const float DEFAULT_MIN_DISTANCE = 5f;
    const float DEFAULT_MAX_DISTANCE = 100f;
    const float MIN_SAME_SOUND_PLAY_DELAY = 0.05f;

    public static SoundManager instance;

    AudioSource oneShotSource;
    List<AudioSource> sources = new List<AudioSource>();
    Dictionary<AudioClip, float> clipsTimes = new Dictionary<AudioClip, float>();
    Dictionary<ClipCollection, float> collectionsTimes = new Dictionary<ClipCollection, float>();
    [SerializeField] AudioMixer mixer;

    private AudioListener _listener = null;
    public AudioListener listener
    {
        get
        {
            if (_listener == null)
            {
                _listener = FindObjectOfType<AudioListener>();
            }
            return _listener;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            oneShotSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
        _listener = null;
    }

    public void Play(AudioClip clip)
    {
        if (CanPlay(clip))
            oneShotSource.PlayOneShot(clip);
    }

    public void Play(AudioClip clip, Vector3 position)
    {
        if (CanPlay(clip))
            AudioSource.PlayClipAtPoint(clip, position);
    }

    public void Play(AudioClip clip, float pitch)
    {
        if (!CanPlay(clip))
            return;

        AudioSource source = GetSource();
        source.pitch = pitch;
        source.clip = clip;
        source.Play();
    }

    public void Play(AudioClip clip, string mixerGroupName)
    {
        if (!CanPlay(clip))
            return;

        AudioSource source = GetSource();
        
        source.outputAudioMixerGroup = mixer.FindMatchingGroups(mixerGroupName)[0];
        source.clip = clip;
        source.Play();
    }

    public void Play(AudioClip clip, float pitch, AudioMixerGroup mixerGroup)
    {
        if (!CanPlay(clip))
            return;

        AudioSource source = GetSource();
        source.pitch = pitch;
        source.clip = clip;
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();
    }

    public bool CanPlay(AudioClip clip)
    {
        if (Time.time - clipsTimes.GetValueOrDefault(clip, 0f) < clip.length + MIN_SAME_SOUND_PLAY_DELAY)
            return false;

        clipsTimes[clip] = Time.time;
        return true;
    }

    public bool CanPlay(ClipCollection collection, float lengthFactor = 1f)
    {
        if (Time.time - collectionsTimes.GetValueOrDefault(collection, 0f) < lengthFactor * collection.length + MIN_SAME_SOUND_PLAY_DELAY)
            return false;

        collectionsTimes[collection] = Time.time;
        return true;
    }

    public AudioSource GetSource()
    {
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
            {
                source.pitch = 1f;
                return source;
            }
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.dopplerLevel = 0f;
        newSource.spatialBlend = 0f;
        newSource.minDistance = DEFAULT_MIN_DISTANCE;
        newSource.maxDistance = DEFAULT_MAX_DISTANCE;
        sources.Add(newSource);
        return newSource;
    }

    public bool IsHearable(AudioSource source)
    {
        Vector3 diff = source.transform.position - listener.transform.position;
        return diff.sqrMagnitude <= source.maxDistance * source.maxDistance;
    }

    const float REAL_MIN_VOLUME = -80f;
    const float MIN_VOLUME = -30f;
    const float MAX_VOLUME = 0f;

    float soundsVolume = 1f;
    float musicVolume = 1f;

    private float ToDb(float volume)
    {
        if (volume <= float.Epsilon)
            return REAL_MIN_VOLUME;
        return volume * (MAX_VOLUME - MIN_VOLUME) + MIN_VOLUME;
    }

    public float SoundsVolume
    {
        get
        {
            return soundsVolume;
        }
        set
        {
            soundsVolume = value;
            mixer.SetFloat("SoundsVolume", ToDb(value));
        }
    }

    public float MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            musicVolume = value;
            mixer.SetFloat("MusicVolume", ToDb(value));
        }
    }
}
