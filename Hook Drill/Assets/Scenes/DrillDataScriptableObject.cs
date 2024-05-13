using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class DrillDataScriptableObject : ScriptableObject
{
    [Range(0f, 100f)]
    [Tooltip("This is the variable for the speed of turning while in the air.")]
    [SerializeField] float AirDecceleration;

    [Tooltip("This is the variable for the decceleration while in the air.")]
    [Range(0f, 100f)]
    [SerializeField] float AirTurningMaxSpeed;

    [Tooltip("This is the variable for the acceleration while underground.")]
    [Range(0f, 100)]
    [SerializeField] float GroundAcceleration;

    [Tooltip("This is the variable for the speed of turning underground.")]
    [Range(0f, 100f)]
    [SerializeField] float GroundTurningMaxSpeed;
    
    [Tooltip("This is the variable for the speed of falling in air. Standard is 9.8")]
    [Range(-10f, 100f)]
    [SerializeField] float Gravity;

    [Tooltip("This is the variable for the total length of all rope segments combined.")]
    [Range(0f, 100f)]
    [SerializeField] float RopeLength;

    [Tooltip("This is the variable for the total number of rope segments.")]
    [Range(0, 10)]
    [SerializeField] int RopeNumberOfJoints;

    [Tooltip("This is the variable for amount of time the drill is boosted during a boost.")]
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostDuration;

    [Tooltip("This is the variable for amount of force used on the drill during a boost.")]
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostForce;

    [Tooltip("This is the variable for the amount of time the player must wait before being able to boost again.")]
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostCoolDown;
}
