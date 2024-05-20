using UnityEngine;

[CreateAssetMenu(fileName = "New Values", menuName = "CameraValues")]
public class CameraObject : ScriptableObject
{
    [Range(1f, 5f)]public float minimumZoom;
    [Range(2f, 6f)] public float maximumZoom;
    [Range(0f, 1f)] public float smoothTime;
}
