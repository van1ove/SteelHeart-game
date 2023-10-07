using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class DroneAnimator : MonoBehaviour
    {
        public Animator Animator => _animator;

        [SerializeField, Required] private DroneBehavior _robotBrain;
        [SerializeField, Required] private Health _health;
        [SerializeField, Required] private NavMeshAgent _navMeshAgent;
        [SerializeField, Required] private Animator _animator;

        [Header("Animator Parameters")]
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Float)] private string _animatorRobotSpeed;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorAttack;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorTakeDamage;
        [SerializeField, AnimatorParam(nameof(_animator), AnimatorControllerParameterType.Trigger)] private string _animatorDeath;

        private void Start()
        {
            _health.OnTakeDamage.AddListener(() => _animator.SetTrigger(_animatorTakeDamage));
            _health.OnDeath.AddListener(() => _animator.SetTrigger(_animatorDeath));

            _robotBrain.OnAttack.AddListener(OnAttack);
        }

        private void Update()
        {
            _animator.SetFloat(_animatorRobotSpeed, _navMeshAgent.velocity.magnitude);
        }
        
        private void OnAttack()
        {
            _animator.SetTrigger(_animatorTakeDamage);
        }

    }
}