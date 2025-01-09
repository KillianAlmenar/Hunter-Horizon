using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [SerializeField] PlayerSoundPlayer soundPlayer;

    private void Step()
    {
        soundPlayer.PlayStep();
    }
}
