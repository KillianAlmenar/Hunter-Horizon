using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] GameObject sword;
    [SerializeField] GameObject gun;
    [SerializeField] ThirdPersonPlayer player;
    private bool swordActive = false;

    void Start()
    {
        sword.SetActive(false);
        gun.SetActive(false);
    }

    private void Update()
    {
        if (!player.isAlive)
        {
        }
    }

    public void Sword(InputAction.CallbackContext context)
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.LOBBY)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            sword.SetActive(swordActive);
        }
    }

    public void Weapon(InputAction.CallbackContext context)
    {
        if (GameManager.instance.currentGameState != GameManager.GameState.LOBBY)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            if (sword.activeSelf)
            {
                gun.SetActive(true);
                sword.SetActive(false);
            }
            else
            {
                sword.SetActive(true);
                gun.SetActive(false);
            }
        }
    }

    public void Unsheath()
    {
        swordActive = !swordActive;
        sword.SetActive(swordActive);
        gun.SetActive(swordActive);
    }
}
