using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(InputCar))]
    [RequireComponent(typeof(CarForce))]
    [RequireComponent(typeof(CarWheelBase))]
    [RequireComponent(typeof(CarBodyInertia))]
    public class CarControlBehavior : GameUpdaterComponent<CarControlBehaviorSystem>
    {
        [SerializeField] private DataCarControl dataCarControl;

        private float _defaultDrag;
        private float _defaultDragAngular;
        
        private float _acceleration;
        private float _brake;
        private float _handBrake;
        private float _handBrakeMobility;

        private float _movementForce;
        private float _steeringAngle;
        
        private float _movementValue;
        private float _steeringValue;
        
        private float _movementMobilityValue;
        private float _steeringMobilityValue;

        private float _velocitySpeed;
        private float _velocitySpeedNormalized;

        private Vector3 _smoothSteeringDirection;
        
        private Rigidbody _rigidbody;
        private InputCar _inputCar;
        private CarForce _carForce;
        private CarWheelBase _carWheelBase;
        private CarBodyInertia _carBodyInertia;

        private void GetDefaultPhysicValues()
        {
            _defaultDrag = _rigidbody.drag;
            _defaultDragAngular = _rigidbody.angularDrag;
        }

        private void Initialize()
        {
            if (_rigidbody == null ||
                _inputCar == null ||
                _carForce == null ||
                _carWheelBase == null ||
                _carBodyInertia == null)
            {
                _rigidbody = GetComponent<Rigidbody>();
                _inputCar = GetComponent<InputCar>();
                _carForce = GetComponent<CarForce>();
                _carWheelBase = GetComponent<CarWheelBase>();
                _carBodyInertia = GetComponent<CarBodyInertia>();
            }

            GetDefaultPhysicValues();
        }

        private void GetVelocity()
        {
            var velocity = _rigidbody.velocity;
            
            _velocitySpeed = velocity.magnitude;
            _velocitySpeedNormalized = Mathf.Clamp01(_velocitySpeed / dataCarControl.Data.MovementMaxSpeed);
        }
        
        private void CalculateMovementForce()
        {
            var coefficient = Mathf.Pow(3.6f, 2);
            
            _movementForce = (dataCarControl.Data.MovementMaxSpeed * _rigidbody.mass) * coefficient;
        }
        
        private void ControlSteeringValue()
        {
            var steeringSpeed = Time.fixedDeltaTime * (dataCarControl.Data.SteeringInputSpeed * dataCarControl.Data.SteeringSpeedMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized));
            
            _steeringValue = Mathf.MoveTowards(_steeringValue, _inputCar.Steering, steeringSpeed);

            _steeringAngle = dataCarControl.Data.SteeringMaxAngle * (dataCarControl.Data.MaxAngleMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized) * _steeringValue);
        }
        
        private void ControlMovementValue()
        {
            var brakeSpeed = Time.fixedDeltaTime * (dataCarControl.Data.BrakeInputSpeed * dataCarControl.Data.BrakeInputSpeedMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized));
            var handBrakeSpeed = Time.fixedDeltaTime * (dataCarControl.Data.HandBrakeInputSpeed * dataCarControl.Data.HandBrakeInputSpeedMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized));
            var accelerationSpeed = Time.fixedDeltaTime * (dataCarControl.Data.AccelerationInputSpeed * dataCarControl.Data.AccelerationInputSpeedMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized));
            
            _handBrake = _inputCar.HandBrake > 0 ? Mathf.MoveTowards(_handBrake, _inputCar.HandBrake, handBrakeSpeed) : 0.0f;
            
            _brake        = Mathf.MoveTowards(_brake, _inputCar.Brake, brakeSpeed) * (1.0f - _handBrake);
            _acceleration = Mathf.MoveTowards(_acceleration, _inputCar.Acceleration, accelerationSpeed) * (1.0f - _handBrake);
            
            _movementValue = (_acceleration - _brake) * (1.0f - _handBrake);
            
            _movementValue = Mathf.Clamp(_movementValue, -1, 1);
        }
        
        private void SetForceControl()
        {
            _rigidbody.drag = _carWheelBase.WheelsCollided ? _defaultDrag : 0.0f;
            _rigidbody.angularDrag = _carWheelBase.WheelsCollided ? _defaultDragAngular : 0.05f;
            
            _carForce.CarForceControl = new CarForceControlData
            {
                movementValue = _movementForce * _movementValue,
            };
        }
        
        private void SetWheelsControl()
        {
            _carWheelBase.WheelControl = new WheelControlData
            {
                angle = _steeringAngle,
                friction = Mathf.Abs(_movementValue) > 0.0f ? 0.0f : 1.0f,
            };
        }

        private void SetSideOffsets()
        {
            _carWheelBase.SteeringInput = new SteeringInputData
            {
                steeringValue = _steeringValue * dataCarControl.Data.MaxAngleMultiplyByMaxSpeed.Data.Curve.Evaluate(_velocitySpeedNormalized),
            };
        }

        private void SetMobility()
        {
            var movementMobilitySpeed = Mathf.Lerp(dataCarControl.Data.MovementMinMobilitySpeed, dataCarControl.Data.MovementMaxMobilitySpeed, 1.0f - _inputCar.HandBrake);
            var steeringMobilitySpeed = Mathf.Lerp(dataCarControl.Data.SteeringMinMobilitySpeed, dataCarControl.Data.SteeringMaxMobilitySpeed, 1.0f - _inputCar.HandBrake);
            
            _movementMobilityValue = Mathf.MoveTowards(_movementMobilityValue, movementMobilitySpeed, Time.fixedDeltaTime * dataCarControl.Data.MobilitySpeed);
            _steeringMobilityValue = Mathf.MoveTowards(_steeringMobilityValue, steeringMobilitySpeed, Time.fixedDeltaTime * dataCarControl.Data.MobilitySpeed);
            
            _carWheelBase.MobilityControl = new MobilityControlData
            {
                movementSpeed = _movementMobilityValue,
                steeringSpeed = _steeringMobilityValue,
            };
        }

        private void SetInertiaControl()
        {
            _carBodyInertia.CarBodyInertiaControl = new CarBodyInertiaControlData
            {
                movementValue = -_velocitySpeedNormalized,
                steeringValue = -_steeringValue,
            };
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
            if (dataCarControl)
            {
                GetVelocity();
                CalculateMovementForce();
                
                ControlMovementValue();
                ControlSteeringValue();
                
                SetForceControl();
            }
            
            SetWheelsControl();
            SetSideOffsets();
            SetMobility();
            SetInertiaControl();
        }

        private void OnGUI()
        {
            GUILayout.Label($"VelocitySpeed : {_velocitySpeed}" +
                            $"\nVelocitySpeedNormalized : {_velocitySpeedNormalized}");
        }

        private void OnDrawGizmos()
        {
            if (_carWheelBase)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawRay(_carWheelBase.WheelBaseCenter, _smoothSteeringDirection);
            }
        }
    }
}