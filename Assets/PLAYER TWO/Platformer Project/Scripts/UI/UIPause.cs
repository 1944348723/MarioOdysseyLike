using System;
using UnityEngine;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private String showTrigger = "Show";
    [SerializeField] private String hideTrigger = "Hide";

    public void Show()
    {
        animator.SetTrigger(showTrigger);
    }

    public void Hide()
    {
        animator.SetTrigger(hideTrigger);
    }
}