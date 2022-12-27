// using System;
// using Notteam.GameCoreV2;
// using UnityEngine;
//
// public class Car : GameSystemComponent<CarSystem>
// {
//     [Serializable]
//     public struct WheelData
//     {
//         public Vector3 Position;
//         public Vector3 ForcePosition;
//         public Vector3 PositionLimit;
//         
//         public Transform Transform;
//     }
//     
//     [SerializeField] private CarCharacteristicData characteristicData;
//     [SerializeField] private InputCar input;
//     [SerializeField] private CarWheel[] wheels;
//
//     private bool _idle;
//     private bool _move;
//     private bool _rotate;
//
//     private float _steeringAngle;
//     private float _movementForce;
//
//     private float _carWorldAngle;
//
//     private bool _groundDetected;
//     
//     private WheelData _wheelFLData;
//     private WheelData _wheelFRData;
//     private WheelData _wheelBLData;
//     private WheelData _wheelBRData;
//
//     private Vector3 _carForwardDirection;
//
//     private Rigidbody _rigidbody;
//
//     // protected override void OnInitializedComponentInEditor()
//     // {
//     //     Initialize();
//     // }
//     //
//     // protected override void OnUpdateComponentInEditor()
//     // {
//     //     ConstraintWheelsArray();
//     //     UpdateCarProperties();
//     //     UpdateWheelsSize();
//     //     UpdateWheelsData();
//     // }
//
//     private void Initialize()
//     {
//         _rigidbody = GetComponent<Rigidbody>();
//
//         UpdateWheelPhysicsProperties();
//     }
//     
//     protected override void OnAwakeComponent()
//     {
//         Initialize();
//     }
//
//     protected override void OnEnabledComponent()
//     {
//         Initialize();
//     }
//
//     protected override void OnUpdateFixedComponent()
//     {
//         _groundDetected = false;
//         
//         foreach (var wheel in wheels)
//         {
//             if (wheel.GroundDetection)
//                 _groundDetected = true;
//         }
//         
//         UpdateCarProperties();
//         UpdateWheelsSize();
//         UpdateWheelsData();
//         // UpdateWheelPhysicsProperties();
//         UpdateControl();
//         UpdateForce();
//     }
//
//     // protected override void OnStartComponent()
//     // {
//     //     Initialize();
//     // }
//
//     // protected override void OnExecuteFixedComponent()
//     // {
//     //     _groundDetected = false;
//     //     
//     //     foreach (var wheel in wheels)
//     //     {
//     //         if (wheel.GroundDetection)
//     //             _groundDetected = true;
//     //     }
//     //     
//     //     UpdateCarProperties();
//     //     UpdateWheelsSize();
//     //     UpdateWheelsData();
//     //     UpdateWheelPhysicsProperties();
//     //     UpdateControl();
//     //     UpdateForce();
//     // }
//
//     private void UpdateCarProperties()
//     {
//         if (characteristicData)
//              _rigidbody.mass = characteristicData.CarMass;
//         
//         _carWorldAngle = transform.eulerAngles.y;
//     }
//     
//     private void UpdateWheelsData()
//     {
//         _carForwardDirection = transform.forward;
//         
//         if (characteristicData)
//         {
//             _wheelFLData.Position = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelBaseHeight, characteristicData.WheelBase / 2);
//             _wheelFRData.Position = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelBaseHeight, characteristicData.WheelBase / 2);
//             _wheelBLData.Position = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelBaseHeight, -characteristicData.WheelBase / 2);
//             _wheelBRData.Position = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelBaseHeight, -characteristicData.WheelBase / 2);
//             
//             _wheelFLData.ForcePosition = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelBaseForceHeight, characteristicData.WheelBase / 2);
//             _wheelFRData.ForcePosition = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelBaseForceHeight, characteristicData.WheelBase / 2);
//             _wheelBLData.ForcePosition = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelBaseForceHeight, -characteristicData.WheelBase / 2);
//             _wheelBRData.ForcePosition = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelBaseForceHeight, -characteristicData.WheelBase / 2);
//             
//             _wheelFLData.PositionLimit = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelsArchHeight, characteristicData.WheelBase / 2);
//             _wheelFRData.PositionLimit = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelsArchHeight, characteristicData.WheelBase / 2);
//             _wheelBLData.PositionLimit = transform.TransformPoint(-characteristicData.WheelTrack / 2, characteristicData.WheelsArchHeight, -characteristicData.WheelBase / 2);
//             _wheelBRData.PositionLimit = transform.TransformPoint(+characteristicData.WheelTrack / 2, characteristicData.WheelsArchHeight, -characteristicData.WheelBase / 2);
//
//             if (wheels.Length == 4)
//             {
//                 _wheelFLData.Transform = wheels[0] ? wheels[0].transform : null;
//                 _wheelFRData.Transform = wheels[1] ? wheels[1].transform : null;
//                 _wheelBLData.Transform = wheels[2] ? wheels[2].transform : null;
//                 _wheelBRData.Transform = wheels[3] ? wheels[3].transform : null;
//             }
//         }
//     }
//
//     private void UpdateWheelsSize()
//     {
//         if (characteristicData)
//         {
//             if (wheels[0])
//                 wheels[0].Radius = characteristicData.WheelsForwardRadius;
//             
//             if (wheels[1])
//                 wheels[1].Radius = characteristicData.WheelsForwardRadius;
//             
//             if (wheels[2])
//                 wheels[2].Radius = characteristicData.WheelsBackwardRadius;
//             
//             if (wheels[3])
//                 wheels[3].Radius = characteristicData.WheelsBackwardRadius;
//         }
//     }
//     
//     private void UpdateWheelPhysicsProperties()
//     {
//         if (characteristicData)
//         {
//             var wheelFLOffset = transform.InverseTransformPoint(_wheelFLData.Position);
//             var wheelFROffset = transform.InverseTransformPoint(_wheelFRData.Position);
//             var wheelBLOffset = transform.InverseTransformPoint(_wheelBLData.Position);
//             var wheelBROffset = transform.InverseTransformPoint(_wheelBRData.Position);
//
//             foreach (var wheel in wheels)
//             {
//                 wheel.Mass = characteristicData.WheelMass;
//                 wheel.ArchHeight = characteristicData.WheelsArchHeight;
//                 wheel.TargetPosition = -characteristicData.WheelBaseHeight;
//                 wheel.Spring = characteristicData.WheelsSpring;
//                 wheel.SpringDamp = characteristicData.WheelsSpringDamp;
//             }
//             
//             // wheels[0].ConnectedAnchor = new Vector3(wheelFLOffset.x, 0, wheelFLOffset.z);
//             // wheels[1].ConnectedAnchor = new Vector3(wheelFROffset.x, 0, wheelFROffset.z);
//             // wheels[2].ConnectedAnchor = new Vector3(wheelBLOffset.x, 0, wheelBLOffset.z);
//             // wheels[3].ConnectedAnchor = new Vector3(wheelBROffset.x, 0, wheelBROffset.z);
//         }
//     }
//
//     private void UpdateControl()
//     {
//         var acceleration = input.Acceleration - input.Brake;
//         
//         _move = Mathf.Abs(acceleration) > 0;
//         _idle = _move == false;
//         _rotate = Mathf.Abs(input.Steering) > 0;
//
//         if (_move & _groundDetected)
//             _movementForce += ((characteristicData.MaxMovementSpeed * acceleration) - _movementForce) * characteristicData.MovementForceIncreaseSpeed * Time.fixedDeltaTime;
//         else
//             _movementForce += (0 - _movementForce) * characteristicData.MovementForceDecreaseSpeed * Time.fixedDeltaTime;
//
//         _rigidbody.drag = _groundDetected ? characteristicData.DragMovement : 0.0f;
//         _rigidbody.angularDrag = _groundDetected ? characteristicData.DragAngular : 0.05f;
//         
//         _movementForce = Mathf.Clamp(_movementForce, -characteristicData.MaxMovementSpeed, characteristicData.MaxMovementSpeed);
//
//         _steeringAngle = characteristicData.MaxSteeringAngle * input.Steering;
//     }
//
//     private void UpdateForce()
//     {
//         var wheelMovementForce = new Vector3(Mathf.Sin((_carWorldAngle) * Mathf.Deg2Rad), _carForwardDirection.y, Mathf.Cos((_carWorldAngle) * Mathf.Deg2Rad)) * (_movementForce * characteristicData.MovementForceMultiply);
//         var wheelSteeringForce = new Vector3(Mathf.Sin((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad), _carForwardDirection.y, Mathf.Cos((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad)) * (_movementForce * characteristicData.SteeringForceMultiply);
//
//         wheels[0].transform.localRotation = Quaternion.Euler(0, _steeringAngle, 0);
//         wheels[1].transform.localRotation = Quaternion.Euler(0, _steeringAngle, 0);
//
//         if (_groundDetected)
//         {
//             _rigidbody.centerOfMass = new Vector3(0, characteristicData.WheelBaseForceHeight, 0);
//             
//             _rigidbody.AddForceAtPosition(wheelSteeringForce, _wheelFLData.ForcePosition);
//             _rigidbody.AddForceAtPosition(wheelSteeringForce, _wheelFRData.ForcePosition);
//             _rigidbody.AddForceAtPosition(wheelMovementForce, _wheelBLData.ForcePosition);
//             _rigidbody.AddForceAtPosition(wheelMovementForce, _wheelBRData.ForcePosition);
//         }
//     }
//     
//     private void ConstraintWheelsArray()
//     {
//         if (wheels.Length > 4)
//         {
//             var prevArray = wheels;
//
//             wheels = new[]
//             {
//                 prevArray[0],
//                 prevArray[1],
//                 prevArray[2],
//                 prevArray[3],
//             };
//         }
//     }
//     
//     // TODO : It will be necessary to implement a separate visualization system, such a footcloth is not convenient for understanding
//     private void OnDrawGizmos()
//     {
//         { // Wheels base visualization
//             Gizmos.color = Color.cyan * 0.5f;
//             Gizmos.DrawLine(_wheelFLData.Position, _wheelFRData.Position);
//             Gizmos.DrawLine(_wheelBLData.Position, _wheelBRData.Position);
//
//             var centerForwardWheelsTrack = (_wheelFLData.Position + _wheelFRData.Position) / 2;
//             var centerBackwardWheelsTrack = (_wheelBLData.Position + _wheelBRData.Position) / 2;
//             
//             Gizmos.DrawLine(centerForwardWheelsTrack, centerBackwardWheelsTrack);
//             
//             Gizmos.DrawSphere(_wheelFLData.Position, 0.2f);
//             Gizmos.DrawSphere(_wheelFRData.Position, 0.2f);
//             Gizmos.DrawSphere(_wheelBLData.Position, 0.2f);
//             Gizmos.DrawSphere(_wheelBRData.Position, 0.2f);
//             
//             Gizmos.color = Color.green * 0.5f;
//             Gizmos.DrawSphere(_wheelFLData.ForcePosition, 0.2f);
//             Gizmos.DrawSphere(_wheelFRData.ForcePosition, 0.2f);
//             Gizmos.DrawSphere(_wheelBLData.ForcePosition, 0.2f);
//             Gizmos.DrawSphere(_wheelBRData.ForcePosition, 0.2f);
//
//             { // Wheels visualization
//
//                 if (characteristicData)
//                 {
//                     var detalization = 45;
//
//                     for (int i = 0; i < 4; i++)
//                     {
//                         var wheelData = default(WheelData);
//                         var wheelSteering = false;
//
//                         switch (i)
//                         {
//                             case 0:
//                                 wheelData = _wheelFLData;
//
//                                 wheelSteering = true;
//                                 break;
//                             case 1:
//                                 wheelData = _wheelFRData;
//                                 
//                                 wheelSteering = true;
//                                 break;
//                             case 2:
//                                 wheelData = _wheelBLData;
//                                 break;
//                             case 3:
//                                 wheelData = _wheelBRData;
//                                 break;
//                         }
//
//                         if (wheelData.Transform)
//                         {
//                             var wheelUp = wheelData.Transform.up;
//                             var wheelForward = wheelData.Transform.forward;
//                             var carUp = transform.up;
//                             var carForward = transform.forward;
//                         
//                             DrawCircle(wheelUp, wheelForward, wheelData.Transform.position, 360.0f, wheelSteering ? characteristicData.WheelsForwardRadius : characteristicData.WheelsBackwardRadius, Color.white);
//                             DrawCircle(carForward, carUp, wheelData.PositionLimit, 180.0f, wheelSteering ? characteristicData.WheelsForwardRadius : characteristicData.WheelsBackwardRadius, Color.red);
//                         }
//                     }
//                 }
//             }
//         }
//
//         { // Steering visualization
//             Gizmos.color = Color.white * 0.5f;
//             Gizmos.DrawRay(_wheelFLData.Position, new Vector3(Mathf.Sin((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad), 0, Mathf.Cos((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad)));
//             Gizmos.DrawRay(_wheelFRData.Position, new Vector3(Mathf.Sin((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad), 0, Mathf.Cos((_carWorldAngle + _steeringAngle) * Mathf.Deg2Rad)));
//         }
//         
//         if (_rigidbody)
//         {
//             Gizmos.DrawSphere(_rigidbody.worldCenterOfMass, 0.1f);
//         }
//     }
//
//     private void DrawCircle(Vector3 up, Vector3 forward, Vector3 position, float limit, float radius, Color color)
//     {
//         var detalization = 45;
//         
//         var circlePoints = new Vector3[detalization];
//                     
//         for (int i = 0; i < circlePoints.Length; i++)
//         {
//             var stepRadius = (360.0f / detalization) * i;
//
//             if (stepRadius <= limit)
//             {
//                 var radiusPoint = 
//                     (up * Mathf.Cos(stepRadius * Mathf.Deg2Rad)) +
//                     (forward * Mathf.Sin(stepRadius * Mathf.Deg2Rad));
//                     
//                 circlePoints[i] = position + radiusPoint * radius / 2;
//                     
//                 Gizmos.color = color;
//                 Gizmos.DrawSphere(circlePoints[i], 0.025f);
//             }
//         }
//     }
// }
