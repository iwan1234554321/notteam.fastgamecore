using System;
using Notteam.FastGameCore;
using UnityEngine;

[Serializable]
public struct WheelBaseData
{
    [SerializeField] private float wheelForwardTrackOffset;
    [SerializeField] private float wheelBackwardTrackOffset;
    [Space]
    [SerializeField] private float wheelForwardRadius;
    [SerializeField] private float wheelBackwardRadius;
    [Space]
    [SerializeField] private float wheelForwardCamber;
    [SerializeField] private float wheelBackwardCamber;
    [Space]
    [SerializeField] private float wheelForwardYawMagnitude;
    [SerializeField] private float wheelBackwardYawMagnitude;
    [Space]
    [SerializeField] private float wheelOffsetForwardCenter;
    [SerializeField] private float wheelOffsetBackwardCenter;
    [Space]
    [SerializeField] private float wheelForwardArchHeight;
    [SerializeField] private float wheelBackwardArchHeight;

    public float WheelForwardTrackOffset => wheelForwardTrackOffset / 1000.0f;
    public float WheelBackwardTrackOffset => wheelBackwardTrackOffset / 1000.0f;
    
    public float WheelForwardRadius => wheelForwardRadius;
    public float WheelBackwardRadius => wheelBackwardRadius;
    public float WheelForwardCamber => wheelForwardCamber;
    public float WheelBackwardCamber => wheelBackwardCamber;
    
    public float WheelForwardYawMagnitude => wheelForwardYawMagnitude;
    public float WheelBackwardYawMagnitude => wheelBackwardYawMagnitude;
    
    public float WheelOffsetForwardCenter => wheelOffsetForwardCenter;
    public float WheelOffsetBackwardCenter => wheelOffsetBackwardCenter;
    
    public float WheelForwardArchHeight => wheelForwardArchHeight;
    public float WheelBackwardArchHeight => wheelBackwardArchHeight;
}

[CreateAssetMenu(menuName = "Game/CarCharacteristic/Create Car Wheel Base Data", fileName = "CarWheelBaseData", order = 0)]
public class DataCarWheelBase : GameData<WheelBaseData>
{
    [SerializeField] private WheelBaseData data;
    public override WheelBaseData Data => data;
}