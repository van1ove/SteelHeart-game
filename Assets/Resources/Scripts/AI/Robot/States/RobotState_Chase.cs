using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace AI
{
    public class RobotState_Chase : MonoBehaviour, IState
    {
        [SerializeField, Required] private AIMoveAgent _aiMoveAgent;
        [SerializeField] private RobotVision _robotVision;
        [SerializeField] private float _speed;
        [SerializeField] private float _minDistance;
        [SerializeField] private float _maxDistance;

        [Inject] private Player _player;

        private float _baseStoppingDistance;

        public void OnEnter()
        {
            _aiMoveAgent.Speed = _speed;
            _aiMoveAgent.UpdateRotation = true;

            _baseStoppingDistance = _aiMoveAgent.StoppingDistance;
            _aiMoveAgent.StoppingDistance = _minDistance;
        }

        public void OnExit()
        {
            _aiMoveAgent.StoppingDistance = _baseStoppingDistance;
            _player = null;
        }

        public void Tick()
        {
            if (_robotVision.IsVisible(out _player))
                _aiMoveAgent.SetDestination(_player.transform.position);
        }

        public bool IsLostPlayer()
        {
            return _player == null || Vector3.Distance(_player.transform.position, _aiMoveAgent.transform.position) > _maxDistance;
        }

        public bool IsDone()
        {
            return _aiMoveAgent.IsDone() && _player != null;
        }
    }
}
