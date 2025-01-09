using UnityEngine;

public class PlayerSoundPlayer : MonoBehaviour
{
    const float PITCH_MIN = 0.8f;
    const float PITCH_MAX = 1.2f;

    [SerializeField] SoundPlayer dashSoundPlayer;
    [SerializeField] SoundPlayer swordSoundPlayer;
    [SerializeField] SoundPlayer unsheatheSoundPlayer;
    [SerializeField] SoundPlayer sheatheSoundPlayer;
    [SerializeField] RandomSoundPlayer stepsSoundPlayer;
    [SerializeField] RandomSoundPlayer attackSoundPlayer;
    [SerializeField] RandomSoundPlayer heavyAttackSoundPlayer;
    [SerializeField] RandomSoundPlayer gunShotPlayer;
    public float speed = 0f;

    public void PlayDash()
    {
        dashSoundPlayer.Play(Random.Range(PITCH_MIN, PITCH_MAX));
    }

    public void PlaySword()
    {
        swordSoundPlayer.Play(Random.Range(PITCH_MIN, PITCH_MAX));
    }

    public void PlayUnsheathe()
    {
        unsheatheSoundPlayer.Play();
    }

    public void PlaySheathe()
    {
        sheatheSoundPlayer.Play();
    }

    public void PlayAttack()
    {
        attackSoundPlayer.Play(Random.Range(PITCH_MIN, PITCH_MAX));
    }

    public void PlayHeavyAttack()
    {
        heavyAttackSoundPlayer.Play();
    }

    public void PlayStep()
    {
        stepsSoundPlayer.Play(speed + 0.5f);
    }

    public void PlayGunShot()
    {
        gunShotPlayer.Play(Random.Range(PITCH_MIN, PITCH_MAX));
    }
}
