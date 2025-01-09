using UnityEngine;

[CreateAssetMenu(fileName = "ClipCollection", menuName = "Sound/Clip collection")]
public class ClipCollection : ScriptableObject
{
    public AudioClip[] clips;
    public float length;

    private void Awake()
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.length > length)
                length = clip.length;
        }
    }
}