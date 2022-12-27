using System;
using UnityEngine;

public class RigidbodyTransformBase : MonoBehaviour
{
    [Flags]
    public enum Axis { None = 0, X = 1, Y = 2, Z = 4 }
    
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private float forcePositionInfluence = 1.0f;
    [SerializeField] private float forceRotationInfluence = 1.0f;
    [SerializeField] private bool forceVelocityDamp;
    [SerializeField] private Axis axisPosition = Axis.X | Axis.Y | Axis.Z;
    [SerializeField] private Axis axisRotation = Axis.X | Axis.Y | Axis.Z;

    public ForceMode ForceMode => forceMode;
    public float ForcePositionInfluence => forcePositionInfluence;
    public float ForceRotationInfluence => forceRotationInfluence;
    public bool ForceVelocityDamp => forceVelocityDamp;
    public Axis AxisPosition => axisPosition;
    public Axis AxisRotation => axisRotation;
}