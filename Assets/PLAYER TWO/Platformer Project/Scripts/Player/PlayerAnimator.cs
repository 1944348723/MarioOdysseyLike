using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 目前通过每帧在LateUpdate中更新Animator Paramaters来驱动动画
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    [System.Serializable]
    public class ForcedTransition
    {
        // 对应的是EntityStateManager中的状态列表的索引
        public int fromStateId;
        public int toAnimationLayer;
        public string toAnimationStateName;
    }

    [Header("Parameter Names")]
    [SerializeField] private string stateParameterName = "State";
    [SerializeField] private string lastStateParameterName = "Last State";
    [SerializeField] private string planarSpeedParameterName = "Planar Speed";
    [SerializeField] private string verticalSpeedParameterName = "Vertical Speed";
    [SerializeField] private string planarAnimationSpeedParameterName = "Planar Animation Speed";
    [SerializeField] private string healthParameterName = "Health";
    [SerializeField] private string jumpCounterParameterName = "Jump Counter";
    [SerializeField] private string isGroundedParameterName = "Is Grounded";
    [SerializeField] private string isHoldingParameterName = "Is Holding";
    [SerializeField] private string onStateChangedParameterName = "On State Changed";

    [Header("Settings")]
    [SerializeField] private float minPlanarAnimationSpeed = 0.5f;
    [SerializeField] private List<ForcedTransition> forcedTransitions;

    // Animator参数的Hash值，避免每次调用Animator.SetXXX时都要计算Hash
    private int stateHash;
    private int lastStateHash;
    private int planarSpeedHash;
    private int verticalSpeedHash;
    private int planarAnimationSpeedHash;
    private int healthHash;
    private int jumpCounterHash;
    private int isGroundedHash;
    private int isHoldingHash;
    private int onStateChangedHash;

    private Player player;
    [SerializeField] private Animator animator;
    private Dictionary<int, ForcedTransition> forcedTransitionsMap;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        InitializeForcedTransitionsMap();
        InitializeAnimatorParameterHashes();

        // 这俩目前不知道有什么用，目前通过LateUpdate更新参数就实现驱动动画了
        player.StateMachine.events.Changed.AddListener(HandleForcedTransitions);
        player.StateMachine.events.Changed.AddListener(() => animator.SetTrigger(onStateChangedHash));
    }

    private void LateUpdate()
    {
        // 通过每帧更新参数来驱动动画
        UpdateAnimatorParameters();
    }

    private void HandleForcedTransitions()
    {
        int lastStateIndex = player.StateMachine.LastStateIndex;

        if (forcedTransitionsMap.ContainsKey(lastStateIndex))
        {
            int layer = forcedTransitionsMap[lastStateIndex].toAnimationLayer;
            animator.Play(forcedTransitionsMap[lastStateIndex].toAnimationStateName, layer);
        }
    }

    // 将forcedTransitions列表转换成map，key是状态id，这样能过滤掉重复key，并且方便后续直接通过状态id查询状态转换
    private void InitializeForcedTransitionsMap()
    {
        forcedTransitionsMap = new Dictionary<int, ForcedTransition>();
        foreach (var transition in forcedTransitions)
        {
            if (!forcedTransitionsMap.ContainsKey(transition.fromStateId))
            {
                forcedTransitionsMap.Add(transition.fromStateId, transition);
            }
        }
    }

    // 计算动画参数的Hash值，避免每次调用Animator.SetXXX时都要计算Hash
    private void InitializeAnimatorParameterHashes()
    {
        stateHash = Animator.StringToHash(stateParameterName);
        lastStateHash = Animator.StringToHash(lastStateParameterName);
        planarSpeedHash = Animator.StringToHash(planarSpeedParameterName);
        verticalSpeedHash = Animator.StringToHash(verticalSpeedParameterName);
        planarAnimationSpeedHash = Animator.StringToHash(planarAnimationSpeedParameterName);
        healthHash = Animator.StringToHash(healthParameterName);
        jumpCounterHash = Animator.StringToHash(jumpCounterParameterName);
        isGroundedHash = Animator.StringToHash(isGroundedParameterName);
        isHoldingHash = Animator.StringToHash(isHoldingParameterName);
        onStateChangedHash = Animator.StringToHash(onStateChangedParameterName);
    }

    private void UpdateAnimatorParameters()
    {
        float planarSpeed = player.PlanarVelocity.magnitude;
        float verticalSpeed = player.VerticalVelocity.y;
        // 范围为[minPlanarAnimationSpeed, 1]
        float planarAnimationSpeed = Mathf.Max(minPlanarAnimationSpeed, planarSpeed / player.Stats.Current.maxSpeed);

        animator.SetInteger(stateHash, player.StateMachine.CurrentStateIndex);
        animator.SetInteger(lastStateHash, player.StateMachine.LastStateIndex);
        animator.SetFloat(planarSpeedHash, planarSpeed);
        animator.SetFloat(verticalSpeedHash, verticalSpeed);
        animator.SetFloat(planarAnimationSpeedHash, planarAnimationSpeed);
        animator.SetBool(isGroundedHash, player.IsGrounded);
    }
}