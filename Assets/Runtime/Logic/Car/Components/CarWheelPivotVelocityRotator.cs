using Notteam.FastGameCore;
using UnityEngine;

[UpdateAfter(typeof(CarWheel))]
[RequireComponent(typeof(CarWheel))]
public class CarWheelPivotVelocityRotator : GameUpdaterComponent<CarWheelSystem>
{
    [SerializeField] private Transform pivot;

    private Vector3 _velocity;
    private Vector3 _prevPosition;
    
    private CarWheel _carWheel;

    private void Initialize()
    {
        if (_carWheel == null)
            _carWheel = GetComponent<CarWheel>();
    }

    private void CalculateVelocity()
    {
        var wheelPosition = transform.position;

        _velocity = (wheelPosition - _prevPosition) / Time.fixedDeltaTime;
        _prevPosition = wheelPosition;
    }

    private void ControlPivotRotation()
    {
        var velocityDirection = Vector3.Dot(transform.forward, _velocity);
        var rotationValue = _carWheel.WheelSetup.radius > 0.0f ? velocityDirection / _carWheel.WheelSetup.radius : 0.0f;
        
        pivot.localRotation *= Quaternion.Euler(rotationValue, 0, 0);
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
        CalculateVelocity();

        if (pivot)
        {
            ControlPivotRotation();
        }
    }
}
