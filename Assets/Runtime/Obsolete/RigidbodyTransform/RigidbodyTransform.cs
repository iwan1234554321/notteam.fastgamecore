using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyTransform : RigidbodyTransformBase
{
    [SerializeField] private Vector3 position = Vector3.zero;
    [SerializeField] private Quaternion rotation = Quaternion.identity;
    
    private Rigidbody _rigidbody;

    public Vector3 Position { get => position; set => position = value; }
    public Quaternion Rotation { get => rotation; set => rotation = value; }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _rigidbody.maxAngularVelocity = 1000.0f;

        position = transform.position;
        rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        var positionDelta =
            position - _rigidbody.position;
        var rotationDelta =
            rotation * Quaternion.Inverse(_rigidbody.rotation);
        var rotationDeltaVector = new Vector3(
            rotationDelta.x,
            rotationDelta.y,
            rotationDelta.z) * rotationDelta.w;

        var rigidPosition =
            (positionDelta * ForcePositionInfluence) -
            (ForceVelocityDamp ? _rigidbody.velocity * Mathf.Clamp01(ForcePositionInfluence) : Vector3.zero);
        var rigidRotation =
            (rotationDeltaVector * ForceRotationInfluence) -
            (ForceVelocityDamp ? _rigidbody.angularVelocity * Mathf.Clamp01(ForceRotationInfluence) : Vector3.zero);

        var rigidAxisLockedPosition = new Vector3(
            AxisPosition.HasFlag(Axis.X) ? rigidPosition.x : 0,
            AxisPosition.HasFlag(Axis.Y) ? rigidPosition.y : 0,
            AxisPosition.HasFlag(Axis.Z) ? rigidPosition.z : 0);
        
        var rigidAxisLockedRotation = new Vector3(
            AxisRotation.HasFlag(Axis.X) ? rigidRotation.x : 0,
            AxisRotation.HasFlag(Axis.Y) ? rigidRotation.y : 0,
            AxisRotation.HasFlag(Axis.Z) ? rigidRotation.z : 0);

        _rigidbody.AddForce(rigidAxisLockedPosition, ForceMode);
        _rigidbody.AddTorque(rigidAxisLockedRotation, ForceMode);
    }
}
