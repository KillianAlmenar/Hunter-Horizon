using UnityEngine;

public class BossSoundPlayer : EnemySoundPlayer
{
    [SerializeField] SoundPlayer breathingPlayer;
    [SerializeField] SoundPlayer orbPlayer;

    public void PlayBreathingSound()
    {
        breathingPlayer.Play();
    }

    public void PlayOrbSound()
    {
        orbPlayer.Play();
    }
}
