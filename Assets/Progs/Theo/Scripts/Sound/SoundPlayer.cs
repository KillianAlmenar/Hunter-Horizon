using System.Collections;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public float minDelay = 0f;
    public AudioSource audioSource;
    public AudioClip clip;
    bool canPlay = true;

    public void Play(float pitch = 1f)
    {
        if (canPlay)
        {
            if (audioSource != null)
            {
                if (!SoundManager.instance.IsHearable(audioSource))
                    return;
                audioSource.clip = clip;
                audioSource.pitch = pitch;
                audioSource.Play();
            }
            else
            {
                SoundManager.instance.Play(clip);
            }
            canPlay = false;
            StartCoroutine(WaitBeforePlaying(Mathf.Max(clip.length, minDelay)));
        }
    }

    IEnumerator WaitBeforePlaying(float duration)
    {
        yield return new WaitForSeconds(duration);
        canPlay = true;
    }
}
