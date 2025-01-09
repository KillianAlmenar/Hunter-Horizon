using UnityEngine;

public class EnemySoundPlayer : MonoBehaviour
{
    const float MIN_PITCH = 0.9f;
    const float MAX_PITCH = 1.2f;

    public bool playIdleSounds;
    float idleSoundDelay = 0f;
    [SerializeField] float idleSoundMinDelay;
    [SerializeField] float idleSoundMaxDelay;
    [SerializeField] float minStepsPitch = 0.5f;
    [SerializeField] float maxStepsPitch = 1.5f;
    [SerializeField] RandomSoundPlayer stepsSoundPlayer;
    [SerializeField] RandomSoundPlayer idleSoundPlayer;
    [SerializeField] RandomSoundPlayer attackSoundPlayer;
    [SerializeField] RandomSoundPlayer hitSoundPlayer;
    [SerializeField] RandomSoundPlayer hurtSoundPlayer;
    [SerializeField] LoopSoundPlayer diggingSoundPlayer;
    [SerializeField] SoundPlayer whooshSoundPlayer;
    public float speed = 0f;

    public void Awake()
    {
        idleSoundDelay = Random.Range(idleSoundMinDelay, idleSoundMaxDelay);
    }

    public void PlayAttackSound()
    {
        attackSoundPlayer.Play(Random.Range(MIN_PITCH, MAX_PITCH));
    }

    public void PlayHurtSound()
    {
        hurtSoundPlayer.Play(Random.Range(MIN_PITCH, MAX_PITCH));
    }

    public void PlayWhooshSound()
    {
        whooshSoundPlayer.Play(Random.Range(MIN_PITCH, MAX_PITCH));
    }

    public void PlayHitSound()
    {
        hitSoundPlayer.Play(Random.Range(MIN_PITCH, MAX_PITCH));
    }

    public void StartDigging()
    {
        //diggingSoundPlayer.Play();
    }

    public void StopDigging()
    {
        //diggingSoundPlayer.Stop();
    }

    public void PlayStep()
    {
        stepsSoundPlayer.Play(minStepsPitch + (maxStepsPitch - minStepsPitch) * Mathf.Clamp01(speed));
    }

    protected void Update()
    {
        if (!playIdleSounds)
            return;

        idleSoundDelay -= Time.deltaTime;
        if (idleSoundDelay <= 0f)
        {
            idleSoundDelay = Random.Range(idleSoundMinDelay, idleSoundMaxDelay);
            idleSoundPlayer.Play();
        }
    }
}
