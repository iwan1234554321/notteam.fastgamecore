using System;
using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    public class PhysicsCollisionDetector : GameUpdaterComponent<PhysicsSystem>
    {
        private bool _collided;

        private Action<Collision> _stayCollision;
        private Action<Collision> _exitCollision;
    
        public bool Collided => _collided;

        public Action<Collision> StayCollision
        {
            get => _stayCollision;
            set => _stayCollision = value;
        }
        public Action<Collision> ExitCollision
        {
            get => _exitCollision;
            set => _exitCollision = value;
        }

        private void OnCollisionStay(Collision collision)
        {
            _stayCollision?.Invoke(collision);
        
            _collided = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            _exitCollision?.Invoke(collision);
        
            _collided = false;
        }
    }
}