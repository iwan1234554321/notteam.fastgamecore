// using Notteam.GameCoreV2;
// using UnityEngine;
//
// [RequireComponent(typeof(ConfigurableJoint))]
// public class CarWheel : GameSystemComponent<CarSystem>
// {
//     [SerializeField] private float radius;
//     [Space]
//     [SerializeField] private Transform wheelRotationPivot;
//
//     private bool _groundDetected;
//     
//     private float _mass;
//     private float _archHeight;
//     private float _targetPosition;
//     private float _spring;
//     private float _springDamp;
//     private Vector3 _connectedAnchor;
//     
//     private Vector3 _velocity;
//     private Vector3 _prevPosition;
//
//     private Rigidbody _rigidbody;
//     private ConfigurableJoint _joint;
//
//     public bool GroundDetection => _groundDetected;
//     public float Radius { get => radius; set => radius = value; }
//     public float Mass { get => _mass; set => _mass = value; }
//     public float ArchHeight { get => _archHeight; set => _archHeight = value; }
//     public float TargetPosition { get => _targetPosition; set => _targetPosition = value; }
//     public float Spring { get => _spring; set => _spring = value; }
//     public float SpringDamp { get => _springDamp; set => _springDamp = value; }
//     public Vector3 ConnectedAnchor { get => _connectedAnchor; set => _connectedAnchor = value; }
//
//     private void Initialize()
//     {
//         if (_joint == null)
//             _joint = GetComponent<ConfigurableJoint>();
//
//         if (_rigidbody == null)
//             _rigidbody = GetComponent<Rigidbody>();
//     }
//     
//     protected override void OnAwakeComponent()
//     {
//         Initialize();
//         UpdatePhysicsBehaviour();
//     }
//
//     protected override void OnEnabledComponent()
//     {
//         Initialize();
//         UpdatePhysicsBehaviour();
//     }
//
//     protected override void OnUpdateFixedComponent()
//     {
//         UpdateWheelSize();
//         UpdatePhysicsBehaviour();
//         RotateWheelByVelocity();
//     }
//
//     // protected override void OnInitializedComponentInEditor()
//     // {
//     //     Initialize();
//     // }
//     //
//     // protected override void OnUpdateComponentInEditor()
//     // {
//     //     UpdateWheelSize();
//     // }
//     //
//     // protected override void OnStartComponent()
//     // {
//     //     Initialize();
//     //     UpdatePhysicsBehaviour();
//     // }
//     //
//     // protected override void OnExecuteFixedComponent()
//     // {
//     //     UpdateWheelSize();
//     //     UpdatePhysicsBehaviour();
//     //     RotateWheelByVelocity();
//     // }
//
//     private void UpdateWheelSize()
//     {
//         transform.localScale = Vector3.one * radius;
//     }
//
//     private void UpdatePhysicsBehaviour()
//     {
//         _rigidbody.mass = _mass;
//
//         _joint.yDrive = new JointDrive { positionSpring = _spring, positionDamper = _springDamp, maximumForce = 3.402823e+38f };
//         
//         _joint.linearLimit = new SoftJointLimit { limit = _archHeight };
//         _joint.targetPosition = new Vector3(0, _targetPosition, 0);
//         // _joint.connectedAnchor = new Vector3(_connectedAnchor.x, 0, _connectedAnchor.z);
//     }
//     
//     private void RotateWheelByVelocity()
//     {
//         var transformWheel = transform;
//         
//         var wheelPosition = transformWheel.position;
//         var wheelWorldAngle = transformWheel.eulerAngles.y;
//         var wheelDirection = new Vector3(Mathf.Sin((wheelWorldAngle) * Mathf.Deg2Rad), 0, Mathf.Cos((wheelWorldAngle) * Mathf.Deg2Rad));;
//         
//         _velocity = (wheelPosition - _prevPosition) / Time.fixedDeltaTime;
//         _prevPosition = wheelPosition;
//         
//         var directionVelocity = Vector3.Dot(wheelDirection, _velocity);
//         
//         wheelRotationPivot.localRotation *= Quaternion.Euler(directionVelocity * (1.0f / radius), 0, 0);
//     }
//     
//     private void OnCollisionStay(Collision _)
//     {
//         _groundDetected = true;
//     }
//
//     private void OnCollisionExit(Collision _)
//     {
//         _groundDetected = false;
//     }
// }