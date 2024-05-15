using UnityEngine;

[CreateAssetMenu(fileName = "New Values", menuName = "PlayerValues")]

public class PlayerScriptableObject : ScriptableObject
{

    [Header("Speed")]
    [Tooltip("This is the speed of the player on start")][Range(0f, 10f)] public float speed;
    [Tooltip("This is the maximum the speed the player can reach")][Range(1f, 20f)] public float maxSpeedInGround;
    [Tooltip("This is minimal speed the player can have")][Range(1f, 10f)] public float minSpeedInGround;
    [Tooltip("This is minimal speed the player can have")][Range(1f, 10f)] public float minSpeedInAir;
    [Tooltip("This is the rotation speed of the player when he's in drill mode")][Range(0.5f, 1.5f)] public float rotationSpeed;

    [Header("Update Speed")]
    [Tooltip("This is how much the player accelerate while he's in ground")][Range(1f, 1.1f)] public float acceleration;
    [Tooltip("This is how much player speed player loose every frame while he's in the air")][Range(0.9f, 1f)] public float AirDecelleration;
    [Tooltip("This is how much player speed player gain while he's falling")][Range(1f, 1.1f)] public float AirAcceleration;
    [Tooltip("This is how much the player accelerate while he's in ground")][Range(1f, 1.1f)] public float boostAcceleration;

    [Header("CoolDown")]
    [Tooltip("This is the minimum time between switching the two modes")][Range(0f, 3f)] public float changeTimeLimit;
    [Tooltip("This is the duration of the player boost when he'll make the biggest circle")][Range(0f, 3f)] public float boostTime;
    [Tooltip("This is the maximum duration of a player turn to create a loop")][Range(0f, 3f)] public float MaxLoopTime;

    [Header("Else")]
    [Tooltip("gravity")][Range(0f, 10f)] public float gravity;
}
