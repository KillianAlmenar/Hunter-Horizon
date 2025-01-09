using Cinemachine;
using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private ThirdPersonPlayer/*PlayerTest*/ player;
    [SerializeField] private GameObject/*PlayerTest*/ playerGO;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject deathCam;
    bool loading = false;
    public float speed;
    void Start()
    {
    }

    void DeathCamAnim()
    {
        Vector3 start = player.transform.position;


        Vector3 tempPos = deathCam.transform.position;
        Vector3 tempRot = deathCam.transform.localEulerAngles;
        tempPos.y += Time.deltaTime * 2;
        tempRot.y += Time.deltaTime * speed;
        deathCam.transform.position = tempPos;
        deathCam.transform.localEulerAngles = tempRot;
        if (deathCam.transform.position.y > start.y + 10)
        {
            if (loading == false)
            {
                LevelLoader.instance.LoadLevel("Lobby");
                GameManager.instance.SwitchToLobby();
                loading = true;
            }
        }



    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetHealth() <= 0)
        {
            deathCam.SetActive(true);
            MusicManager.instance.Stop();
            DeathCamAnim();
        }
        else
        {

            deathCam.SetActive(false);
        }
        //deathCam.transform.Rotate(0, speed * Time.deltaTime, 0);


    }


}
