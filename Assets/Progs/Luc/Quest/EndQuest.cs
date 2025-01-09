using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndQuest : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textEndQuest;
    [SerializeField] AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetEndQuestCanva(string _questName)
    {
        textEndQuest.text = _questName + " Completed !";
        SoundManager.instance.Play(clip);
    }

    public void DestroyEndQuest()
    {
        Destroy(gameObject);
    }
}
