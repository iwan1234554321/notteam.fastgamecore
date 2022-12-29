using System;
using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    [Serializable]
    public struct CarForceControlData
    {
        public float movementValue;
    }
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CarWheelBase))]
    public class CarForce : GameUpdaterComponent<CarForceSystem>
    {
        [SerializeField] private ForceMode force;
        
        private Rigidbody _rigidbody;
        private CarWheelBase _carWheelBase;
    
        public CarForceControlData CarForceControl { get; set; }
    
        private void Initialize()
        {
            if (_rigidbody == null ||
                _carWheelBase == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                _carWheelBase = GetComponent<CarWheelBase>();
            }
        }
    
        private void SetCenterMass()
        {
            _rigidbody.centerOfMass = transform.InverseTransformPoint(_carWheelBase.WheelBaseCenter);
        }
        
        private void ControlForce()
        {
            var steeringForce = _carWheelBase.WheelsForwardDirection * CarForceControl.movementValue;
            var movementForce = _carWheelBase.WheelsBackwardDirection * CarForceControl.movementValue;
    
            if (_carWheelBase.WheelsCollided)
            {
                _rigidbody.AddForceAtPosition(steeringForce, _carWheelBase.WheelsForwardCenter, force);
                _rigidbody.AddForceAtPosition(movementForce, _carWheelBase.WheelsBackwardCenter, force);
            }
        }
    
        protected override void OnInitializedEditor()
        {
            Initialize();
    
            SetCenterMass();
        }
    
        protected override void OnAwakeComponent()
        {
            Initialize();
        }
    
        protected override void OnUpdateFixedComponent()
        {
            ControlForce();
            
            SetCenterMass();
        }
    
        private void OnDrawGizmos()
        {
            if (_rigidbody)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
            }
        }
    }
}