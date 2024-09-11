using UnityEngine;

public class LinearWaypoint : Waypoint
{
    public override Transform GetNextPoint(int currentIndex)
    {
        if (currentIndex < points.Length - 1)
        {
            return points[currentIndex + 1];
        }
        else
        {
            return points[currentIndex]; 
        }
    }
}
