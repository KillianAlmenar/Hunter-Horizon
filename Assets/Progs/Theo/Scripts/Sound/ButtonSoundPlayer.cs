using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    const float VOLUME = 0.6f;

    public AudioClip hoveredClip;
    public AudioClip clickClip;

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.instance.Play(clickClip, "Noises");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.instance.Play(hoveredClip, "Noises");
    }
}
