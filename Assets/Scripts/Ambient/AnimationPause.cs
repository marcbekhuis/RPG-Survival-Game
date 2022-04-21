using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationPause : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.instance.onTogglePause += ToggleAnimator;
    }

    private void ToggleAnimator(bool active)
    {
        animator.enabled = !active;
    }
}
