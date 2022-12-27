using UnityEngine;

public class TransformVelocity : MonoBehaviour
{
    private Vector3 _velocity;
    private Vector3 _prevPosition;

    public Vector3 Velocity => _velocity;
    
    private void FixedUpdate()
    {
        _velocity = (transform.position - _prevPosition) / Time.fixedDeltaTime;
        _prevPosition = transform.position;
    }
}
