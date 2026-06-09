using UnityEngine;

/// <summary>
/// 目前通过每帧在LateUpdate中更新Animator Paramaters来驱动动画
/// </summary>
public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [Header("Parameter Names")]
    [SerializeField] private string stateParameterName = "State";
    [SerializeField] private string lastStateParameterName = "Last State";
    [SerializeField] private string planarSpeedParameterName = "Planar Speed";
    [SerializeField] private string verticalSpeedParameterName = "Vertical Speed";
    [SerializeField] private string healthParameterName = "Health";
    [SerializeField] private string isGroundedParameterName = "Is Grounded";
    [SerializeField] private string onStateChangedParameterName = "On State Changed";

    [Header("Settings")]
    [SerializeField] private float minPlanarAnimationSpeed = 0.5f;

    // Animator参数的Hash值，避免每次调用Animator.SetXXX时都要计算Hash
    private int stateHash;
    private int lastStateHash;
    private int planarSpeedHash;
    private int verticalSpeedHash;
    private int healthHash;
    private int isGroundedHash;
    private int onStateChangedHash;

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        InitializeAnimatorParameterHashes();
    }

    private void LateUpdate()
    {
        // 通过每帧更新参数来驱动动画
        UpdateAnimatorParameters();
    }

    // 计算动画参数的Hash值，避免每次调用Animator.SetXXX时都要计算Hash
    private void InitializeAnimatorParameterHashes()
    {
        stateHash = Animator.StringToHash(stateParameterName);
        lastStateHash = Animator.StringToHash(lastStateParameterName);
        planarSpeedHash = Animator.StringToHash(planarSpeedParameterName);
        verticalSpeedHash = Animator.StringToHash(verticalSpeedParameterName);
        healthHash = Animator.StringToHash(healthParameterName);
        isGroundedHash = Animator.StringToHash(isGroundedParameterName);
        onStateChangedHash = Animator.StringToHash(onStateChangedParameterName);
    }

    private void UpdateAnimatorParameters()
    {
        float planarSpeed = enemy.PlanarVelocity.magnitude;
        float verticalSpeed = enemy.VerticalVelocity.y;

        animator.SetInteger(stateHash, enemy.StateMachine.CurrentStateIndex);
        animator.SetInteger(lastStateHash, enemy.StateMachine.LastStateIndex);
        animator.SetFloat(planarSpeedHash, planarSpeed);
        animator.SetFloat(verticalSpeedHash, verticalSpeed);
        animator.SetBool(isGroundedHash, enemy.IsGrounded);
    }
}