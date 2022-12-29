using System;
using Notteam.FastGameCore;
using UnityEngine;

namespace Notteam.CarPrototype
{
    [Serializable]
    public struct AnimationCurveData
    {
        [SerializeField] private AnimationCurve curve;

        public AnimationCurve Curve => curve;
    }

    [CreateAssetMenu(menuName = "Game/AnimationCurve/Create Animation Curve Data", fileName = "AnimationCurveData", order = 0)]
    public class DataAnimationCurve : GameData<AnimationCurveData>
    {
        [SerializeField] private AnimationCurveData data;
    
        public override AnimationCurveData Data => data;
    }
}