//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////To call a sound
////SoundManager.PlaySound(SoundManager.Sound.SOUND,GetPosition);
//public static class OldSoundManager
//{
//    public enum Sound
//    {
//        SOUND1,
//        SOUND2,
//        SOUND3
//    }

//    private static Dictionary<Sound, float> soundTimerDictionary;
//    private static GameObject oneShotGameObject;
//    private static AudioSource oneShotAudioSource;
//    public static void Initialize() //CALL THIS AT THE START AT THE GAME MANAGER
//    {
//        soundTimerDictionary = new Dictionary<Sound, float>();
//        soundTimerDictionary[Sound.SOUND1] = 0f;
//    }
   
//    public static void PlaySound(Sound sound)
//    {
//        if(CanPlaySound(sound))
//        {
//            if (oneShotGameObject == null)
//            {
//                oneShotGameObject = new GameObject("One Shot Sound");
//                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
//            }
           
//            oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
//        }     
//    }

//    public static void PlaySound(Sound sound, Vector3 position)
//    {
//        if (CanPlaySound(sound))
//        {
//            GameObject soundObject = new GameObject("Sound");
//            soundObject.transform.position = position;
//            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
//            audioSource.clip = GetAudioClip(sound);

//            audioSource.maxDistance = 100f;
//            audioSource.spatialBlend = 1f;
//            audioSource.rolloffMode = AudioRolloffMode.Linear;
//            audioSource.dopplerLevel = 0f;

//            audioSource.Play();

//            Object.Destroy(soundObject, audioSource.clip.length);
//        }
//    }

//    private static bool CanPlaySound(Sound sound)
//    {
//        switch (sound)
//        {
//            default:
//                return true;
//            //Put here all sounds that needs to end before being played again
//            case Sound.SOUND1:
//                if (soundTimerDictionary.ContainsKey(sound))
//                {
//                    float lastTimePlayed = soundTimerDictionary[sound];
//                    float sound1ActionTimerMax = 1f;
//                    if (lastTimePlayed + sound1ActionTimerMax < Time.time)
//                    {
//                        soundTimerDictionary[sound] = Time.time;
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                else
//                {
//                    return true;
//                }
//        }
//    }

//    private static AudioClip GetAudioClip(Sound sound)
//    {
//        foreach (TempGameManager.SoundAudioClip soundAudioClip in TempGameManager.i.soundAudioClipArray)
//        {
//            if (soundAudioClip.sound == sound)
//            {
//                return soundAudioClip.audioClip;
//            }
//        }
//        Debug.LogError("Sound " + sound + " not found");
//        return null;
//    }
//}
