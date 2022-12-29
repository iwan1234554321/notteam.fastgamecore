using UnityEngine;

public static class GizmosUtils
{
    public static void DrawCircle(Vector3 up, Vector3 forward, Vector3 position, float limit, float radius, Color color)
    {
        var detalization = 45;
         
        var circlePoints = new Vector3[detalization];
                     
        for (int i = 0; i < circlePoints.Length; i++)
        {
            var stepRadius = (360.0f / detalization) * i;

            if (stepRadius <= limit)
            {
                var radiusPoint = 
                    (up * Mathf.Cos(stepRadius * Mathf.Deg2Rad)) +
                    (forward * Mathf.Sin(stepRadius * Mathf.Deg2Rad));
                     
                circlePoints[i] = position + radiusPoint * radius / 2;
                     
                Gizmos.color = color;

                if (i > 0)
                {
                    Gizmos.DrawLine(circlePoints[i - 1], circlePoints[i]);
                     
                    if (i == circlePoints.Length - 1)
                        Gizmos.DrawLine(circlePoints[i], circlePoints[0]);
                }
            }
        }
    }
}
