using System;
using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    [Serializable]
    public struct CarInertiaData
    {
        [SerializeField] private float maxPitch;
        [SerializeField] private float maxRoll;

        public float MaxPitch => maxPitch;
        public float MaxRoll => maxRoll;
    }

    [CreateAssetMenu(menuName = "Game/CarCharacteristic/Create Car Inertia Data", fileName = "CarInertiaData", order = 0)]
    public class DataCarInertia : GameData<CarInertiaData>
    {
        [SerializeField] private CarInertiaData data;

        public override CarInertiaData Data => data;
    }
}