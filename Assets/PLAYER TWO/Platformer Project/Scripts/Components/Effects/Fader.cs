using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private float defaultDuration = 0.3f;

    private Coroutine running;

    private void OnValidate()
    {
        if (defaultDuration < 0) defaultDuration = 0.1f;
    }

    public Coroutine FadeOut()
    {
        return FadeTo(1f, defaultDuration);
    }

    public Coroutine FadeIn()
    {
        return FadeTo(0f, defaultDuration);
    }

    public Coroutine FadeTo(float targetAlpha, float duration)
    {
        if (running != null)
        {
            StopCoroutine(running);
        }
        running = StartCoroutine(FadeRoutine(targetAlpha, duration));
        return running;
    }

    private IEnumerator FadeRoutine(float targetAlpha, float duration)
    {
        float startAlpha = image.color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / duration);
            Color color = image.color;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            image.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, targetAlpha);
        running = null;
    }
}