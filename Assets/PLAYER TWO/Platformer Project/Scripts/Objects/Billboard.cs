using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour
{
    [SerializeField] private string content = "Hello World";
    [SerializeField] private Canvas canvas;
    [SerializeField] private Text uiText;
    [SerializeField] private float scaleDuration = 0.25f;

    private Vector3 initScale;
    private bool showing = false;
    private Camera playerCamera;

    private void Awake()
    {
        playerCamera = Camera.main;
    }

    private void Start()
    {
        initScale = canvas.transform.localScale;
        canvas.gameObject.SetActive(true);
        canvas.transform.localScale = Vector3.zero;
        uiText.text = content;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        Vector3 cameraPlanarDirection = playerCamera.transform.forward;
        cameraPlanarDirection.y = 0;
        Vector3 billboardPlanarDirection = transform.forward;
        billboardPlanarDirection.y = 0;
        // cos < 0 => 大于90度
        if (Vector3.Dot(cameraPlanarDirection, billboardPlanarDirection) < 0)
        {
            Show();
        } else
        {
            Hide();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.GetComponent<Player>()) return;

        Hide();
    }

    private void Show()
    {
        if (!showing)
        {
            showing = true;
            DoTween.To<Vector3>(
                () => canvas.transform.localScale,
                scale => canvas.transform.localScale = scale,
                initScale,
                scaleDuration
            );
        }
    }

    private void Hide()
    {
        if (showing)
        {
            showing = false;
            DoTween.To<Vector3>(
                () => canvas.transform.localScale,
                scale => canvas.transform.localScale = scale,
                Vector3.zero,
                scaleDuration
            );
        }
    }
}