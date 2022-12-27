using System;
using Notteam.FastGameCore;
using UnityEngine;

public enum CarWheelType
{
    Forward,
    Backward,
}

[Serializable]
public struct WheelSetupData
{
    public float radius;
    public float camber;
}

[Serializable]
public struct WheelControlData
{
    public float angle;
    public float friction;
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PhysicsCollisionDetector))]
public class CarWheel : GameUpdaterComponent<CarWheelSystem>
{
    [SerializeField] private CarWheelType wheelType;
    [SerializeField] private Transform origin;

    private Vector3 _originRotation;

    private PhysicMaterial _wheelPhysicMaterial;
    
    private Collider _collider;
    private Rigidbody _rigidbody;
    private ConfigurableJoint _joint;
    private PhysicsCollisionDetector _collisionDetector;

    public CarWheelType WheelType => wheelType;
    public ConfigurableJoint Joint => _joint;
    public PhysicsCollisionDetector CollisionDetector => _collisionDetector;

    public WheelSetupData WheelSetup { get; set; }
    public WheelControlData WheelControl { get; set; }

    private void Initialize()
    {
        if (_collider == null ||
            _rigidbody == null ||
            _joint == null ||
            _collisionDetector == null)
        {
            _collider = GetComponentInChildren<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _joint = GetComponent<ConfigurableJoint>();
            _collisionDetector = GetComponent<PhysicsCollisionDetector>();
        }
    }

    private void SetupCollisionFriction()
    {
        _wheelPhysicMaterial = _collider.material = new PhysicMaterial
        {
            bounciness = 0,
            dynamicFriction = 0,
            staticFriction = 0,
            frictionCombine = PhysicMaterialCombine.Multiply,
        };
    }
    
    private void ControlWheelCamber()
    {
        _originRotation.z = WheelSetup.camber;
    }

    private void ControlWheelScale()
    {
        transform.localScale = Vector3.one * WheelSetup.radius;
    }
    
    private void ControlOriginRotation()
    {
        origin.localRotation = Quaternion.Euler(_originRotation);
    }
    
    private void ControlYaw()
    {
        _originRotation.y = WheelControl.angle;
    }

    private void ControlFriction()
    {
        _wheelPhysicMaterial.dynamicFriction = WheelControl.friction;
    }

    protected override void OnInitializedEditor()
    {
        Initialize();
        
        SetupCollisionFriction();
    }

    protected override void OnUpdatedEditor()
    {
        ControlWheelScale();

        if (origin)
        {
            ControlWheelCamber();

            ControlOriginRotation();
        }
    }

    protected override void OnAwakeComponent()
    {
        Initialize();

        SetupCollisionFriction();
    }

    protected override void OnUpdateFixedComponent()
    {
        ControlWheelScale();
        
        if (origin)
        {
            ControlWheelCamber();
            ControlYaw();
            
            ControlOriginRotation();
        }

        ControlFriction();
    }
}
