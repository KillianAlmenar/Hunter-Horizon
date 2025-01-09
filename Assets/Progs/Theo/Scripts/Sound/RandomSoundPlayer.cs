using UnityEngine;
using UnityEngine.Audio;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField] float lengthFactor = 1f;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ClipCollection clips;
    [SerializeField] AudioMixerGroup mixerGroup;
    [SerializeField] bool playOnStart;
    int previousClipIndex = -1;

    void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play(float pitch = 1f)
    {
        if (SoundManager.instance.CanPlay(clips, lengthFactor))
        {
            AudioClip clip = GetRandomClip();
            if (audioSource != null)
            {
                if (!IsSourceHearable() || audioSource.isPlaying)
                    return;

                audioSource.clip = clip;
                audioSource.pitch = pitch;
                audioSource.outputAudioMixerGroup = mixerGroup;
                audioSource.Play();
            }
            else if (mixerGroup == null)
            {
                SoundManager.instance.Play(clip, pitch);
            }
            else
            {
                SoundManager.instance.Play(clip, pitch, mixerGroup);
            }
        }
    }

    AudioClip GetRandomClip()
    {
        int clipIndex;
        do
        {
            clipIndex = Random.Range(0, clips.clips.Length);
        } while (clipIndex == previousClipIndex && clips.clips.Length > 1);

        previousClipIndex = clipIndex;
        return clips.clips[clipIndex];
    }

    bool IsSourceHearable()
    {
        Vector3 diff = audioSource.transform.position - SoundManager.instance.listener.transform.position;
        return diff.sqrMagnitude <= audioSource.maxDistance * audioSource.maxDistance;
    }
}
