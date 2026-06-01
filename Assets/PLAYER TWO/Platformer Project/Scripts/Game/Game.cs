using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private UIPause uiPause;

    private bool paused = false;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (GameInputSystem.Instance.IsPausePressedThisFrame())
        {
            if (!paused)
            {
                Pause();
            } else
            {
                UnPause();
            }
        }
    }

    private void Pause()
    {
        paused = true;
        uiPause.gameObject.SetActive(true);
        uiPause.Show();
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameInputSystem.Instance.LockCamera();
    }

    private void UnPause()
    {
        paused = false;
        uiPause.Hide();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameInputSystem.Instance.UnlockCamera();
    }
}