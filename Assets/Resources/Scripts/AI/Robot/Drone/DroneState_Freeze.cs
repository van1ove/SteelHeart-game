using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class DroneState_Freeze : IState
    {
        private NavMeshAgent _navMeshAgent;
        private DroneState_Attack _attackState;

        private float _minHeight;
        private float _duration;

        private float _maxHeight;
        private float _endTime;

        public DroneState_Freeze
            (NavMeshAgent navMeshAgent, DroneState_Attack attackState, float minHeight, float duration)
        {
            _navMeshAgent = navMeshAgent;
            _attackState = attackState;

            _minHeight = minHeight;
            _duration = duration;
        }

        public void OnEnter()
        {
            _maxHeight = _navMeshAgent.baseOffset;

            _endTime = Time.time + _duration;
            
            //_navMeshAgent.baseOffset = Mathf.Lerp(_navMeshAgent.baseOffset, _minHeight, 0.1f);
            _navMeshAgent.isStopped = true;
        }

        public void OnExit()
        {
            _endTime = 0;

            _attackState.Reload();

            _navMeshAgent.baseOffset = _maxHeight;
            _navMeshAgent.isStopped = false;
        }

        public void Tick()
        {
            // smooth getting up
            if(Time.time >= _endTime - _duration / 3)
                _navMeshAgent.baseOffset = ChangeHeight(_navMeshAgent.baseOffset, _maxHeight, 0.05f);

            // smooth getting down
            else if(Time.time <= _endTime - 2 * _duration / 3)
                _navMeshAgent.baseOffset = ChangeHeight(_navMeshAgent.baseOffset, _minHeight, 0.05f);
        }

        public bool IsDone() => _endTime <= Time.time;

        private float ChangeHeight(float a, float b, float t)
        {
            return Mathf.Lerp(a, b, t);
        }
    }
}