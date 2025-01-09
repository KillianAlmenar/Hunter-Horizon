using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBoss : MonoBehaviour
{
    private MeshRenderer m_Renderer;
    [SerializeField] ThirdPersonPlayer m_Player;
    public bool active;
    public float damage;

    private void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            m_Renderer.enabled = true;
        }
        else
        {
            m_Renderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Player.TakeDamage(damage);
        }
    }
}
