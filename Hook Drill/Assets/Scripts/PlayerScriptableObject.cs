using UnityEngine;

[CreateAssetMenu(fileName = "New Values", menuName = "PlayerValues")]

public class PlayerScriptableObject : ScriptableObject
{

    [Header("Speed")]
    [Tooltip("This is the speed of the player")][Range(0f, 10f)] public float speed;
    [Tooltip("This is the maximum the speed the player can reach")][Range(1f, 20f)] public float maxSpeed;
    [Tooltip("This is the rotation speed of the player when he's in drill mode")][Range(0.5f, 1.5f)] public float rotationSpeed;
    [Tooltip("This is the acceleration of the player when he's in ground")][Range(1f, 1.1f)] public float acceleration;

    [Header("CoolDown")]
    [Tooltip("This is the time the player can stay in the air in drill mode before getting swtch back to *basic*")][Range(0f, 3f)] public float airTimeLimit;
    [Tooltip("This is the minimum time between switching the two modes")][Range(0f, 3f)] public float changeTimeLimit;

    [Header("Else")]
    [Tooltip("gravity")][Range(0f, 10f)] public float gravity;
}
