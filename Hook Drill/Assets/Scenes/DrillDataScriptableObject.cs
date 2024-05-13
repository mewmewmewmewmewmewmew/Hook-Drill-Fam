using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillDataScriptableObject : ScriptableObject
{
    [Range(0f, 100f)]
    [SerializeField] float AirDecceleration;
    [Range(0f, 100f)]
    [SerializeField] float AirTurningMaxSpeed;
    [Range(0f, 100)]
    [SerializeField] float GroundAcceleration;
    [Range(0f, 100f)]
    [SerializeField] float GroundTurningMaxSpeed;
    [Range(-10f, 100f)]
    [SerializeField] float Gravity;
    [Range(0f, 100f)]
    [SerializeField] float RopeLength;
    [Range(0, 10)]
    [SerializeField] int RopeNumberOfJoints;
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostDuration;
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostForce;
    [Range(0f, 100f)]
    [SerializeField] float SpeedBoostCoolDown;



}
