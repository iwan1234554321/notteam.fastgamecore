using System;
using Notteam.FastGameCore;
using UnityEngine;

[Serializable]
public struct SteeringInputData
{
    public float steeringValue;
}

[Serializable]
public struct MobilityControlData
{
    public float movementSpeed;
    public float steeringSpeed;
}

public class CarWheelBase : GameUpdaterComponent<CarWheelBaseSystem>
{
    [Serializable]
    protected struct RelativeCarWheelsPosition
    {
        public CarWheelType type;
        public Vector3 position;
    }
    
    [SerializeField] private DataCarWheelBase dataWheelBase;

    private CarWheel[] _wheels;

    private RelativeCarWheelsPosition[] _wheelsPositionsRelativeCar = Array.Empty<RelativeCarWheelsPosition>();

    private bool _wheelsCollided;
    
    private float _wheelOffsetSideForwardCenter;
    private float _wheelOffsetSideBackwardCenter;
    
    private Vector3 _wheelBaseCenter;
    private Vector3 _wheelBaseNormal;

    private Vector3 _wheelsForwardCenter;
    private Vector3 _wheelsBackwardCenter;
    
    private Vector3 _wheelsForwardDirection;
    private Vector3 _wheelsBackwardDirection;

    public bool WheelsCollided => _wheelsCollided;
    public Vector3 WheelsForwardCenter => _wheelsForwardCenter;
    public Vector3 WheelsBackwardCenter => _wheelsBackwardCenter;
    
    public Vector3 WheelsForwardDirection => _wheelsForwardDirection;
    public Vector3 WheelsBackwardDirection => _wheelsBackwardDirection;
    
    public Vector3 WheelBaseCenter => _wheelBaseCenter;
    public Vector3 WheelBaseNormal => _wheelBaseNormal;
    
    public WheelControlData WheelControl { get; set; }
    public SteeringInputData SteeringInput { get; set; }
    public MobilityControlData MobilityControl { get; set; }

    private void CollectWheels()
    {
        var childWheels = GetComponentsInChildren<CarWheel>();

        if (childWheels.Length >= 4)
        {
            _wheels = new CarWheel[4];

            for (int i = 0; i < 4; i++)
                _wheels[i] = childWheels[i];
        }
    }
    
    private void SetupWheels()
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            var wheel = _wheels[i];
            var wheelRadius = dataWheelBase.Data.WheelBackwardRadius;

            if (_wheels[i].WheelType == CarWheelType.Forward)
                wheelRadius = dataWheelBase.Data.WheelForwardRadius;
            
            _wheels[i].WheelSetup = new WheelSetupData
            {
                radius = wheelRadius,
                camber = this.GetCamber(wheel, dataWheelBase),
            };
        }
    }
    private void SetupWheelConnectPositions()
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            _wheels[i].Joint.connectedAnchor = new Vector3(_wheelsPositionsRelativeCar[i].position.x + this.OffsetTrackPosition(_wheels[i], dataWheelBase), _wheelsPositionsRelativeCar[i].position.y, _wheelsPositionsRelativeCar[i].position.z);
        }
    }
    private void SetupArchHeights()
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            var wheelArchLimit = dataWheelBase.Data.WheelBackwardArchHeight;
            
            if (_wheels[i].WheelType == CarWheelType.Forward)
                wheelArchLimit = dataWheelBase.Data.WheelForwardArchHeight;
            
            _wheels[i].Joint.linearLimit = new SoftJointLimit { limit = wheelArchLimit, };
        }
    }
    private void SetupWheelsPositions()
    {
        _wheelsPositionsRelativeCar = new RelativeCarWheelsPosition[_wheels.Length];

        for (int i = 0; i < _wheels.Length; i++)
        {
            var wheel = _wheels[i];

            _wheelsPositionsRelativeCar[i] = new RelativeCarWheelsPosition
            {
                type = wheel.WheelType,
                position = transform.InverseTransformPoint(wheel.transform.position),
            };
        }
    }
    
    private void Initialize()
    {
        CollectWheels();
        
        SetupWheelsPositions();
    }

    private void CalculateWheelBaseCenterAndWheelsCenter(out Vector3 flWheelPosition, out Vector3 frWheelPosition, out Vector3 blWheelPosition, out Vector3 brWheelPosition)
    {
        var centerOfMass = Vector3.zero;
        
        flWheelPosition = Vector3.zero;
        frWheelPosition = Vector3.zero;
        blWheelPosition = Vector3.zero;
        brWheelPosition = Vector3.zero;

        foreach (var wheelPosition in _wheelsPositionsRelativeCar)
        {
            centerOfMass += wheelPosition.position;

            var side = this.GetTrackSideWheelRelativeCarPosition(wheelPosition.position);
            
            switch (wheelPosition.type)
            {
                case CarWheelType.Forward:
                    if (side < 0)
                        flWheelPosition = wheelPosition.position;
                    else
                        frWheelPosition = wheelPosition.position;
                    break;
                case CarWheelType.Backward:
                    if (side < 0)
                        blWheelPosition = wheelPosition.position;
                    else
                        brWheelPosition = wheelPosition.position;
                    break;
            }
        }

        centerOfMass /= _wheelsPositionsRelativeCar.Length;

        _wheelBaseCenter = transform.TransformPoint(centerOfMass);
    }
    private void CalculateWheelsCenter(Vector3 flWheelPosition, Vector3 frWheelPosition, Vector3 blWheelPosition, Vector3 brWheelPosition)
    {
        var carTransform = transform;
        var carTransformRight = carTransform.right;
        
        _wheelsForwardCenter = carTransform.TransformPoint((flWheelPosition + frWheelPosition) / 2) + carTransformRight * _wheelOffsetSideForwardCenter;
        _wheelsBackwardCenter = carTransform.TransformPoint((blWheelPosition + brWheelPosition) / 2) + carTransformRight * _wheelOffsetSideBackwardCenter;
    }
    private void CalculateWheelBaseNormal(Vector3 flWheelPosition, Vector3 frWheelPosition, Vector3 blWheelPosition, Vector3 brWheelPosition)
    {
        var carTransform = transform;

        var flWheelPositionWorld = carTransform.TransformPoint(flWheelPosition);
        var frWheelPositionWorld = carTransform.TransformPoint(frWheelPosition);
        var blWheelPositionWorld = carTransform.TransformPoint(blWheelPosition);
        var brWheelPositionWorld = carTransform.TransformPoint(brWheelPosition);
        
        var diagonalOne = flWheelPositionWorld - brWheelPositionWorld;
        var diagonalTwo = frWheelPositionWorld - blWheelPositionWorld;

        _wheelBaseNormal = Vector3.Cross(diagonalOne, diagonalTwo).normalized;
    }
    
    
    private void SetWheelControlData()
    {
        for (int i = 0; i < _wheels.Length; i++)
        {
            var wheel = _wheels[i];

            var angleWheel = WheelControl.angle * this.GetYawMagnitude(wheel, dataWheelBase);
            
            _wheels[i].WheelControl = new WheelControlData
            {
                angle = angleWheel,
                friction = WheelControl.friction,
            };
        }
    }
    private void SetWheelsDirections()
    {
        var carTransform = transform;
        
        for (int i = 0; i < _wheels.Length; i++)
        {
            var wheel = _wheels[i];

            var direction = CarUtils.DirectionRelativeTransform(carTransform, wheel.WheelControl.angle);
            
            if (wheel.WheelType == CarWheelType.Forward)
                _wheelsForwardDirection = Vector3.MoveTowards(_wheelsForwardDirection, direction, Time.fixedDeltaTime * MobilityControl.steeringSpeed);
            if (wheel.WheelType == CarWheelType.Backward)
                _wheelsBackwardDirection = Vector3.MoveTowards(_wheelsBackwardDirection, direction, Time.fixedDeltaTime * MobilityControl.movementSpeed);
        }
    }
    private void SetOffsetSideWheelsCenters()
    {
        _wheelOffsetSideForwardCenter = dataWheelBase.Data.WheelOffsetForwardCenter * SteeringInput.steeringValue;
        _wheelOffsetSideBackwardCenter = dataWheelBase.Data.WheelOffsetBackwardCenter * SteeringInput.steeringValue;
    }
    

    private void GetCollideInfoFromWheels()
    {
        _wheelsCollided = false;
        
        for (var i = 0; i < _wheels.Length; i++)
        {
            if (_wheels[i].CollisionDetector.Collided)
                _wheelsCollided = true;
        }
    }
    
    protected override void OnInitializedEditor()
    {
        Initialize();
    }

    protected override void OnUpdatedEditor()
    {
        if (dataWheelBase)
        {
            SetupWheels();
            
            CalculateWheelBaseCenterAndWheelsCenter(out var flWheelPosition, out var frWheelPosition, out var blWheelPosition, out var brWheelPosition);
            CalculateWheelsCenter(flWheelPosition, frWheelPosition, blWheelPosition, brWheelPosition);
            CalculateWheelBaseNormal(flWheelPosition, frWheelPosition, blWheelPosition, brWheelPosition);
            
            SetWheelsDirections();
        }
    }

    protected override void OnAwakeComponent()
    {
        Initialize();

        SetupWheelConnectPositions();
    }

    protected override void OnUpdateFixedComponent()
    {
        if (dataWheelBase)
        {
            SetupWheels();
            SetupWheelConnectPositions();
            SetupArchHeights();

            SetOffsetSideWheelsCenters();
            
            CalculateWheelBaseCenterAndWheelsCenter(out var flWheelPosition, out var frWheelPosition, out var blWheelPosition, out var brWheelPosition);
            CalculateWheelsCenter(flWheelPosition, frWheelPosition, blWheelPosition, brWheelPosition);
            CalculateWheelBaseNormal(flWheelPosition, frWheelPosition, blWheelPosition, brWheelPosition);
            
            SetWheelsDirections();
            
            SetWheelControlData();
        }

        GetCollideInfoFromWheels();
    }

    private void OnDrawGizmos()
    {
        if (dataWheelBase)
        {
            var carTransform = transform;
        
            for (var i = 0; i < _wheelsPositionsRelativeCar.Length; i++)
            {
                var wheelPosition = transform.TransformPoint(_wheelsPositionsRelativeCar[i].position);
                
                var wheel = _wheels[i];

                var wheelTransform = wheel.transform;
                
                var wheelRadius = dataWheelBase.Data.WheelBackwardRadius;
                var wheelArchHeight = dataWheelBase.Data.WheelBackwardArchHeight;

                if (wheel.WheelType == CarWheelType.Forward)
                {
                    wheelRadius = dataWheelBase.Data.WheelForwardRadius;
                    wheelArchHeight = dataWheelBase.Data.WheelForwardArchHeight;
                }

                GizmosUtils.DrawCircle(carTransform.forward, carTransform.up, wheelPosition + transform.up * wheelArchHeight, 180.0f, wheelRadius, Color.red);
                GizmosUtils.DrawCircle(wheelTransform.up, wheelTransform.forward, wheelPosition, 360.0f, wheelRadius, Color.cyan);
                
                Gizmos.DrawSphere(wheelPosition, 0.1f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(_wheelBaseCenter, 0.1f);
            Gizmos.DrawRay(_wheelBaseCenter, _wheelBaseNormal);
            
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_wheelsForwardCenter, 0.1f);
            Gizmos.DrawSphere(_wheelsBackwardCenter, 0.1f);
            
            Gizmos.DrawRay(_wheelsForwardCenter, _wheelsForwardDirection);
            Gizmos.DrawRay(_wheelsBackwardCenter, _wheelsBackwardDirection);
        }
    }
}
