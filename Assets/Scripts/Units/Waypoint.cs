using UnityEngine;

public abstract class Waypoint : MonoBehaviour
{
    public Transform[] points; 

    public abstract Transform GetNextPoint(int currentIndex);
}
