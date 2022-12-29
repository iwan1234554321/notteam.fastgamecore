using System;
using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    [Serializable]
    public struct CarBodyInertiaControlData
    {
        public float movementValue;
        public float steeringValue;
    }

    [RequireComponent(typeof(CarWheelBase))]
    public class CarBodyInertia : GameUpdaterComponent<CarBodyInertiaSystem>
    {
        [Serializable]
        public struct CarInertiaValue
        {
            [SerializeField] private float currentValue;
            [SerializeField] private float minLimit;
            [SerializeField] private float maxLimit;

            public float MinLimit => minLimit;
            public float MaxLimit => maxLimit;
            
            public float Value { set => currentValue = value; }
            public float ConstraintValue => Mathf.Clamp(currentValue, minLimit, maxLimit);
        }

        [SerializeField] private DataCarInertia dataCarInertia;
        [SerializeField] private Transform bodyTransform;
        [Space]
        [SerializeField] private CarInertiaValue rollValue;
        [SerializeField] private CarInertiaValue pitchValue;
        [SerializeField] private CarInertiaValue heightValue;
        [Space]
        [SerializeField] private float velocityIncrease;
        [SerializeField] private float speed;

        private float _prevRoll;
        private float _prevPitch;
        private float _prevHeight;
        
        private float _inertiaRoll;
        private float _inertiaPitch;
        private float _inertiaHeight;
        
        private Vector3 _normalBody;
        private Vector3 _forwardBody;
        
        private CarWheelBase _carWheelBase;

        public CarBodyInertiaControlData CarBodyInertiaControl { get; set; }

        private void Initialize()
        {
            if (_carWheelBase == null)
            {
                _carWheelBase = GetComponent<CarWheelBase>();
            }

            _prevRoll = rollValue.ConstraintValue;
            _prevPitch = pitchValue.ConstraintValue;
            _prevHeight = heightValue.ConstraintValue;
            
            _inertiaRoll = rollValue.ConstraintValue;
            _inertiaPitch = pitchValue.ConstraintValue;
            _inertiaHeight = heightValue.ConstraintValue;
        }

        private float GetVelocityValue(float value, ref float prevValue)
        {
            var velocity = (value - prevValue) / Time.fixedDeltaTime;
            prevValue = value;

            return velocity;
        }
        
        private void CalculateInertia()
        {
            var velocityRoll = GetVelocityValue(_inertiaRoll, ref _prevRoll);
            var velocityPitch = GetVelocityValue(_inertiaPitch, ref _prevPitch);
            var velocityHeight = GetVelocityValue(_inertiaHeight, ref _prevHeight);

            _inertiaRoll += ((rollValue.ConstraintValue - _inertiaRoll) * speed) + (velocityRoll * velocityIncrease);
            _inertiaPitch += ((pitchValue.ConstraintValue - _inertiaPitch) * speed) + (velocityPitch * velocityIncrease);
            _inertiaHeight += ((heightValue.ConstraintValue - _inertiaHeight) * speed) + (velocityHeight * velocityIncrease);
        }
        
        private void CalculateDirections(float roll, float pitch)
        {
            var carTransform = transform;

            var carTransformRight = carTransform.right;
            var carTransformForward = carTransform.forward;
            
            _normalBody = _carWheelBase.WheelBaseNormal * Mathf.Cos(roll * Mathf.Deg2Rad) + carTransformRight * Mathf.Sin(roll * Mathf.Deg2Rad);
            _forwardBody = carTransformForward * Mathf.Cos(pitch * Mathf.Deg2Rad) + _normalBody * Mathf.Sin(pitch * Mathf.Deg2Rad);
        }
        
        private void SetCalculationsToBody()
        {
            if (bodyTransform)
            {
                var body = bodyTransform;
                var carTransform = transform;

                var rotationBody = Quaternion.LookRotation(_forwardBody, _normalBody);

                var centerRelativePosition = carTransform.InverseTransformPoint(_carWheelBase.WheelBaseCenter);
                
                body.position = carTransform.TransformPoint(centerRelativePosition + Vector3.up * _inertiaHeight);
                body.rotation = rotationBody;
            }
        }

        private void SetControlData()
        {
            pitchValue.Value = dataCarInertia.Data.MaxPitch * CarBodyInertiaControl.movementValue;
            rollValue.Value = dataCarInertia.Data.MaxRoll * (CarBodyInertiaControl.steeringValue * Mathf.Abs(pitchValue.ConstraintValue));
        }
        
        protected override void OnInitializedEditor()
        {
            Initialize();
        }

        protected override void OnAwakeComponent()
        {
            Initialize();
        }

        protected override void OnUpdateFixedComponent()
        {
            CalculateInertia();
            CalculateDirections(_inertiaRoll, _inertiaPitch);
            SetCalculationsToBody();

            if (dataCarInertia)
            {
                SetControlData();
            }
        }
    }
}