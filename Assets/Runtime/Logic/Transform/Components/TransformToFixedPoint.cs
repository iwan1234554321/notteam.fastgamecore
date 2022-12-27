using Notteam.FastGameCore;
using UnityEngine;

public class TransformToFixedPoint : GameUpdaterComponent<TransformToFixedPointSystem>
{
    [SerializeField] private Transform transformPoint;
    [SerializeField] private bool stabilizationOnDirectionPoint;
    [SerializeField] private float stabilizationSpeed = 1.0f;
    [SerializeField] private Vector3 stabilizationOffset;

    protected override void OnUpdateFixedComponent()
    {
        if (transformPoint)
        {
            transform.position = transformPoint.position;

            if (stabilizationOnDirectionPoint)
            {
                transform.rotation =
                    Quaternion.Lerp(
                        transform.rotation,
                        transformPoint.rotation * Quaternion.Euler(stabilizationOffset),
                        Time.fixedDeltaTime * stabilizationSpeed);
            }
        }
    }
}
