using System;
using System.Collections;
using UnityEngine;

public class PlayerGliderVisual : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject glider;
    [SerializeField] private GameObject trails;
    [SerializeField] private float openDuration = 0.2f;
    [SerializeField] private float closeDuration = 0.15f;
    [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve closeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine currentAnim;

    private void Awake()
    {
        glider.transform.localScale = Vector3.zero;
        glider.SetActive(false);
        trails.SetActive(false);
    }

    private void OnEnable()
    {
        player.playerEvents.GlideStarted.AddListener(Show);
        player.playerEvents.GlideEnded.AddListener(Hide);
    }

    private void OnDisable()
    {
        player.playerEvents.GlideStarted.RemoveListener(Show);
        player.playerEvents.GlideEnded.RemoveListener(Hide);
    }

    private void Show()
    {
        if (currentAnim != null) StopCoroutine(currentAnim);
        glider.SetActive(true);
        trails.SetActive(true);
        currentAnim = StartCoroutine(ScaleTo(Vector3.one, openDuration, openCurve));
    }

    private void Hide()
    {
        if (currentAnim != null) StopCoroutine(currentAnim);
        currentAnim = StartCoroutine(ScaleTo(Vector3.zero, closeDuration, closeCurve, () =>
        {
            glider.SetActive(false);
            trails.SetActive(false);
        }));
    }

    private IEnumerator ScaleTo(Vector3 target, float duration, AnimationCurve curve, Action onComplete = null)
    {
        Vector3 start = glider.transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = curve.Evaluate(elapsed / duration);
            glider.transform.localScale = Vector3.LerpUnclamped(start, target, t);
            yield return null;
        }

        glider.transform.localScale = target;
        onComplete?.Invoke();
        currentAnim = null;
    }
}