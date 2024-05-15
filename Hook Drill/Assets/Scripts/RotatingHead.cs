using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class RotatingHead : MonoBehaviour
{
    public float rotationSpeed;
    private Vector2 direction;
    private float prevAngle;

    public float speed;

    void Update()
    {
        float XaxisJoy = Input.GetAxis("Vertical");
        float YAxisJoy = Input.GetAxis("Horizontal");

        Vector2 joyPos = new Vector2(XaxisJoy, YAxisJoy);

        this.direction = new Vector3(this.transform.position.x + joyPos.y, this.transform.position.y + joyPos.x);

        float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;

        if (angle == 0) { angle = this.prevAngle; }

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);

        this.prevAngle = angle;

        this.transform.position = Vector2.MoveTowards(this.transform.position, Vector2.down, speed * Time.deltaTime);
    }
}