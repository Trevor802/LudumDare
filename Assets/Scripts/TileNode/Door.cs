using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : TileNode
{
    public AudioClip doorOpen;
    public AudioClip doorClose;
    private bool isOpen;
    private Animator m_animator;
    private Collider2D m_collider;

    protected override void Awake()
    {
        base.Awake();
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        isOpen = true;
        m_collider.enabled = false;
        AudioManager.Instance.PlaySingle(doorOpen);
        m_animator.SetBool("isOpen", true);
    }

    public void CloseDoor()
    {
        isOpen = false;
        m_collider.enabled = true;
        AudioManager.Instance.PlaySingle(doorClose);
        m_animator.SetBool("isOpen", false);
    }

    // Getter
    public bool GetOpenState() { return isOpen; }
}
