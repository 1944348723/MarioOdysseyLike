using UnityEngine;
using UnityEngine.UI;

public class UIPause : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string showTrigger = "Show";
    [SerializeField] private string hideTrigger = "Hide";
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button respawnButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;


    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClicked);
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        restartButton.onClick.AddListener(OnRestartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
        respawnButton.onClick.RemoveListener(OnRespawnButtonClicked);
        restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        exitButton.onClick.RemoveListener(OnExitButtonClicked);
    }

    public void Show()
    {
        animator.SetTrigger(showTrigger);
    }

    public void Hide()
    {
        animator.SetTrigger(hideTrigger);
    }

    private void OnResumeButtonClicked()
    {
        LevelManager.Instance.Resume();
    }

    private void OnRespawnButtonClicked()
    {
        LevelManager.Instance.Resume();
        LevelManager.Instance.Respawn();
    }
    
    private void OnRestartButtonClicked()
    {

    }

    private void OnExitButtonClicked()
    {

    }
}