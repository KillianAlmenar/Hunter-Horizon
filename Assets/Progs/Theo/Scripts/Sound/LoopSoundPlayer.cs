using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class LoopSoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] float volumeFadeDuration = 1f;
    [SerializeField] AudioMixerGroup mixerGroup;
    [SerializeField] AudioSource source;
    bool isInTransition = false;

    public void Play()
    {
        if (source == null)
        {
            source = SoundManager.instance.GetSource();
            source.loop = true;
            source.outputAudioMixerGroup = mixerGroup;
        }

        if (!source.isPlaying && !isInTransition)
        {
            source.clip = clip;
            StartCoroutine(StartSource());
        }
    }

    public void Stop()
    {
        if (source == null || isInTransition)
            return;

        if (source.isPlaying)
            StartCoroutine(StopSource());
    }

    IEnumerator StartSource()
    {
        isInTransition = true;

        source.Stop();
        source.time = source.clip.length * Random.Range(0f, 0.99f);
        source.volume = 0f;
        source.Play();
        while (source.volume < 1f)
        {
            source.volume = Mathf.Min(source.volume + Time.deltaTime / volumeFadeDuration, 1f);
            yield return null;
        }
        isInTransition = false;
    }

    IEnumerator StopSource()
    {
        isInTransition = true;

        source.volume = 1f;
        while (source.volume > 0f)
        {
            source.volume = Mathf.Max(source.volume - Time.deltaTime / volumeFadeDuration, 0f);
            yield return null;
        }
        source.Stop();

        isInTransition = false;
    }
}
