using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingTrail : MonoBehaviour
{
    public int length;
    public LineRenderer lineRend;
    public Vector3[] segmentPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;

    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;
    public Transform[] bodyParts;

    public EdgeCollider2D myCollider;

    [SerializeField] private float ExtendingDistance;
    void Start()
    {
        lineRend.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    private void InputHandler()
    {
        float LeftInput = Input.GetAxis("TriggerLeft");
        float RightInput = Input.GetAxis("TriggerRight");
       
        if (LeftInput != 0){ this.targetDist += ExtendingDistance * LeftInput; Debug.Log("Extend"); }
        if (RightInput != 0) { this.targetDist -= ExtendingDistance * RightInput; Debug.Log("No extend"); }
    }
    void LateUpdate()
    {
        this.InputHandler();

        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

        segmentPoses[0] = targetDir.position;

        for (int i = 1; i < segmentPoses.Length; i++)
        {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
            bodyParts[i - 1].position = segmentPoses[i];
        }

        lineRend.SetPositions(segmentPoses);

        List<Vector2> points = new List<Vector2>();
        for (int position = 1; position < lineRend.positionCount; position++)
        {
            //ignores z axis when translating vector3 to vector2
            points.Add(lineRend.GetPosition(position));
        }

        myCollider.SetPoints(points);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("TOUCHE");
        }
    }
}
