using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "Sound/Music Data")]
public class MusicData : ScriptableObject
{
    [Serializable]
    public struct Part
    {
        public AudioClip calmIntro;
        public AudioClip calmLoop;
        public AudioClip strongIntro;
        public AudioClip strongLoop;
        public AudioClip strongOutro;
    }

    public Part[] parts;
}
