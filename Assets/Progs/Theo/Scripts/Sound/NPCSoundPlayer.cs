using UnityEngine;

public class NPCSoundPlayer : MonoBehaviour
{
    [SerializeField] float minPlayerDistance;
    [SerializeField] RandomSoundPlayer nearSoundPlayer;
    [SerializeField] RandomSoundPlayer buySoundPlayer;
    [SerializeField] RandomSoundPlayer leaveSoundPlayer;
    [SerializeField] Transform player;
    bool nearSoundPlayed = false;

    public void PlayBuy()
    {
        buySoundPlayer.Play();
    }

    public void PlayLeave()
    {
        leaveSoundPlayer.Play();
    }

    private void Update()
    {
        Vector3 diff = player.transform.position - transform.position;
        if (diff.sqrMagnitude <= minPlayerDistance * minPlayerDistance)
        {
            if (!nearSoundPlayed)
            {
                nearSoundPlayer.Play();
                nearSoundPlayed = true;
            }
            return;
        }
        nearSoundPlayed = false;
    }
}
