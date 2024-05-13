using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillDataScriptableObject : ScriptableObject
{
    [Range(0f, 1f)]
    [SerializeField] float AirDecceleration;
    [Range(0f, 1f)]
    [SerializeField] float AirTurningMaxSpeed;
    [Range(0f, 1f)]
    [SerializeField] float GroundAcceleration;
    [Range(0f, 1f)]
    [SerializeField] float GroundTurningMaxSpeed;
    [Range(0f, 1f)]
    [SerializeField] float Gravity;


}
