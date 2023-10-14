using UnityEngine;

namespace Notteam.CarPrototype
{
    public static class CarUtils
    {
        public static int GetTrackSideWheel(this CarWheelBase wheelBase, CarWheel wheel)
        {
            var directionWheelFromCenter = (wheel.transform.position - wheelBase.WheelBaseCenter).normalized;
            
            return (int)Mathf.Sign(Vector3.Dot(directionWheelFromCenter, wheelBase.transform.right));
        }
        
        public static int GetTrackSideWheelRelativeCarPosition(this CarWheelBase wheelBase, Vector3 position)
        {
            var carTransform = wheelBase.transform;
            
            var positionWorldRelativeCar = carTransform.TransformPoint(position);
            
            var directionWheelFromCenter = (positionWorldRelativeCar - wheelBase.WheelBaseCenter).normalized;
            
            return (int)Mathf.Sign(Vector3.Dot(directionWheelFromCenter, carTransform.right));
        }
        
        public static float OffsetTrackPosition(this CarWheelBase wheelBase, CarWheel wheel, DataCarWheelBase data)
        {
            var trackPosition = 0.0f;
            
            var side = wheelBase.GetTrackSideWheel(wheel);

            switch (wheel.WheelType)
            {
                case CarWheelType.Forward:
                    if (side < 0)
                        trackPosition = -data.Data.WheelForwardTrackOffset;
                    else
                        trackPosition = data.Data.WheelForwardTrackOffset;
                    break;
                case CarWheelType.Backward:
                    if (side < 0)
                        trackPosition = -data.Data.WheelBackwardTrackOffset;
                    else
                        trackPosition = data.Data.WheelBackwardTrackOffset;
                    break;
            }

            return trackPosition;
        }

        public static float GetCamber(this CarWheelBase wheelBase, CarWheel wheel, DataCarWheelBase data)
        {
            var angle = 0.0f;

            var side = wheelBase.GetTrackSideWheel(wheel);
            
            switch (wheel.WheelType)
            {
                case CarWheelType.Forward:
                    if (side < 0)
                        angle = -data.Data.WheelForwardCamber;
                    else
                        angle = data.Data.WheelForwardCamber;
                    break;
                case CarWheelType.Backward:
                    if (side < 0)
                        angle = -data.Data.WheelBackwardCamber;
                    else
                        angle = data.Data.WheelBackwardCamber;
                    break;
            }
            
            return angle;
        }

        public static float GetYawMagnitude(this CarWheelBase wheelBase, CarWheel wheel, DataCarWheelBase data)
        {
            var magnitude = 0.0f;

            switch (wheel.WheelType)
            {
                case CarWheelType.Forward:
                    magnitude = data.Data.WheelForwardYawMagnitude;
                    break;
                case CarWheelType.Backward:
                    magnitude = data.Data.WheelBackwardYawMagnitude;
                    break;
            }
            
            return magnitude;
        }
        
        public static Vector3 DirectionRelativeTransform(Transform transform, float angle)
        {
            var currentTransform = transform;

            return currentTransform.forward * Mathf.Cos(angle * Mathf.Deg2Rad) + currentTransform.right * Mathf.Sin(angle * Mathf.Deg2Rad);
        }
    }
}