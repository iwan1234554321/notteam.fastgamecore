using System;
using Notteam.FastGameCore;
using UnityEngine;

[UpdateAfter(typeof(PhysicsCollisionDetector))]
public class PhysicsCollisionDetectorGroup : GameUpdaterComponent<PhysicsSystem>
{
    [SerializeField] private PhysicsCollisionDetector[] group = Array.Empty<PhysicsCollisionDetector>();

    private bool _collided;

    public bool Collided => _collided;

    protected override void OnUpdateComponent()
    {
        _collided = false;
        
        foreach (var groupElement in group)
        {
            if (groupElement.Collided)
                _collided = true;
        }
    }
}
