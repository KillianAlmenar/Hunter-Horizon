using UnityEngine;

public class Intro : MonoBehaviour
{

    [SerializeField] float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        LevelLoader.instance.LoadLevelIntro(waitTime);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
