using System;
using Notteam.FastGameCore;
using UnityEngine;

[Serializable]
public struct CarControlData
{
    [SerializeField] private float movementMaxSpeed;
    [SerializeField] private float steeringMaxAngle;
    [Space]
    [Header("Mobility Speed")]
    [SerializeField] private float mobilitySpeed;
    [SerializeField] private float movementMaxMobilitySpeed;
    [SerializeField] private float steeringMaxMobilitySpeed;
    [SerializeField] private float movementMinMobilitySpeed;
    [SerializeField] private float steeringMinMobilitySpeed;
    [Header("Input Speed")]
    [SerializeField] private float accelerationInputSpeed;
    [SerializeField] private float steeringInputSpeed;
    [SerializeField] private float handBrakeInputSpeed;
    [SerializeField] private float brakeInputSpeed;
    [Space]
    [Header("Animation Curves")]
    [SerializeField] private DataAnimationCurve maxAngleMultiplyByMaxSpeed;
    [Space]
    [SerializeField] private DataAnimationCurve steeringSpeedMultiplyByMaxSpeed;
    [Space]
    [SerializeField] private DataAnimationCurve accelerationInputSpeedMultiplyByMaxSpeed;
    [SerializeField] private DataAnimationCurve handBrakeInputSpeedMultiplyByMaxSpeed;
    [SerializeField] private DataAnimationCurve brakeInputSpeedMultiplyByMaxSpeed;

    public float MovementMaxSpeed => movementMaxSpeed;
    public float SteeringMaxAngle => steeringMaxAngle;
    
    
    public float MobilitySpeed => mobilitySpeed;
    public float MovementMaxMobilitySpeed => movementMaxMobilitySpeed;
    public float SteeringMaxMobilitySpeed => steeringMaxMobilitySpeed;
    public float MovementMinMobilitySpeed => movementMinMobilitySpeed;
    public float SteeringMinMobilitySpeed => steeringMinMobilitySpeed;
    
    public float AccelerationInputSpeed => accelerationInputSpeed;
    public float SteeringInputSpeed => steeringInputSpeed;
    public float HandBrakeInputSpeed => handBrakeInputSpeed;
    public float BrakeInputSpeed => brakeInputSpeed;
    
    public DataAnimationCurve MaxAngleMultiplyByMaxSpeed => maxAngleMultiplyByMaxSpeed;
    
    public DataAnimationCurve SteeringSpeedMultiplyByMaxSpeed => steeringSpeedMultiplyByMaxSpeed;
    public DataAnimationCurve AccelerationInputSpeedMultiplyByMaxSpeed => accelerationInputSpeedMultiplyByMaxSpeed;
    public DataAnimationCurve HandBrakeInputSpeedMultiplyByMaxSpeed => handBrakeInputSpeedMultiplyByMaxSpeed;
    public DataAnimationCurve BrakeInputSpeedMultiplyByMaxSpeed => brakeInputSpeedMultiplyByMaxSpeed;
}

[CreateAssetMenu(menuName = "Game/CarCharacteristic/Create Car Control Data", fileName = "CarControlData", order = 0)]
public class DataCarControl : GameData<CarControlData>
{
    [SerializeField] private CarControlData data;

    public override CarControlData Data => data;
}
