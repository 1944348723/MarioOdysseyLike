using System;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    public bool IsInvincible => remainingTime > 0f;
    public event Action Started;
    public event Action Ended;

    private float remainingTime = 0f;

    private void Update()
    {
        if (!IsInvincible) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime < 0f)
        {
            remainingTime = 0f;
            Ended?.Invoke();
        }
    }

    // 已经处于无敌状态时调用无效
    public bool StartInvincibility(float duration)
    {
        if (duration <= 0f)
        {
            Debug.Log("Invicinbility time must be above 0.", this);
            return false;
        }
        if (IsInvincible) return false;

        remainingTime += duration;
        Started.Invoke();

        return true;
    }
}