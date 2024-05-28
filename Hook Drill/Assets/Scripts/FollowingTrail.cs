using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowingTrail : MonoBehaviour
{
    public Transform wiggleDir;
    public Transform[] bodyParts;
    public LineRenderer lineRend;
    public EdgeCollider2D myCollider;

    public UnityEvent ExtendRope;
    public UnityEvent RetractRope;

    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;

    public int length;

    private float tempDist;
    public float targetDist;
    public float smoothSpeed;
    public float wiggleSpeed;
    public float wiggleMagnitude;

    static public bool isHooked;

    [SerializeField] private float ExtendingDistance;
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
        PlayerUpdate.maxdistance = this.length * this.targetDist;
    }
    private void InputHandler()
    {
        float LeftInput = Input.GetAxis("TriggerLeft");
        float RightInput = Input.GetAxis("TriggerRight");
       
        if (LeftInput != 0)
        { 
            this.targetDist += ExtendingDistance * LeftInput; Debug.Log("Extend");
            PlayerUpdate.maxdistance = this.length * this.targetDist;
            this.ExtendRope.Invoke();
        }
        if (RightInput != 0) 
        { 
            this.targetDist -= ExtendingDistance * RightInput; Debug.Log("No extend");
            PlayerUpdate.maxdistance = this.length * this.targetDist;
            this.RetractRope.Invoke();
        }
    }
    void LateUpdate()
    {
        this.InputHandler();

        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            if (i == segmentPoses.Length - 1 && isHooked)
            {
                Debug.Log("skip the last one");
                continue;
            }
                

            if (i == 1)
                this.tempDist += 0.5f;
            else
                this.tempDist = targetDist;

            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * tempDist;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            bodyParts[i - 1].position = segmentPoses[i];
        }

        lineRend.SetPositions(segmentPoses);

        List<Vector2> points = new List<Vector2>();
        for (int position = 1; position < lineRend.positionCount; position++)
        {
            points.Add(lineRend.GetPosition(position));
        }

        myCollider.SetPoints(points);
    }
}
