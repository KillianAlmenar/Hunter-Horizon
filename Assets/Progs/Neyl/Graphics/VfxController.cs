using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VfxController : MonoBehaviour
{
    VisualEffect vfx;
    // Start is called before the first frame update
    void Start()
    {
        vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.LOBBY)
        {
            vfx.enabled = false;
        }
        else
        {
            vfx.enabled=true;
        }
    }
}
