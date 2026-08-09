using System.Collections;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private UIPause uiPause;
    [SerializeField] private Transform defaultRespawnPoint;
    [SerializeField] private Fader fader;

    private bool paused = false;
    private Transform currentRespawnPoint;
    private int coins = 0;
    private static WaitForSeconds blackScreenWaitTime = new(0.5f);
    
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentRespawnPoint = defaultRespawnPoint;
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
                Resume();
            }
        }
    }

    public void SetRespawnPoint(Transform point)
    {
        currentRespawnPoint = point;
    }

    public void AddCoin(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Coin adding amount is negative.");
        }
        coins += amount;
    }

    public void Pause()
    {
        paused = true;
        uiPause.gameObject.SetActive(true);
        uiPause.Show();
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameInputSystem.Instance.LockCamera();
    }

    public void Resume()
    {
        paused = false;
        uiPause.Hide();
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameInputSystem.Instance.UnlockCamera();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return fader.FadeOut();

        player.Respawn(currentRespawnPoint.position, currentRespawnPoint.rotation);
        yield return blackScreenWaitTime;

        yield return fader.FadeIn();
    }
}