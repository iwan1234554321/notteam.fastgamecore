using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    public class PhysicsSystem : GameUpdaterSystem
    {
        [SerializeField] private float timeStep = 1.0f / 90.0f;

        protected override void OnUpdateFixedSystem()
        {
            Physics.Simulate(timeStep);
        }
    }
}