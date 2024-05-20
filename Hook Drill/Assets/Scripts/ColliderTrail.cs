using System.Collections.Generic;
using UnityEngine;

public class ColliderTrail : MonoBehaviour
{
    TrailRenderer myTrail;
    EdgeCollider2D myCollider;

    // Start is called before the first frame update
    void Awake()
    { 
        myTrail = this.GetComponent<TrailRenderer>();
        GameObject colliderGameObject = new GameObject("TrailCollider", typeof(EdgeCollider2D));
        myCollider = colliderGameObject.GetComponent<EdgeCollider2D>();
        myCollider = this.GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderPointsFromTrail(myTrail, myCollider);
    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        for (int position = 0; position < trail.positionCount; position++)

            //ignores z axis when translating vector3 to vector2
            points.Add(trail.GetPosition(position));

        collider.SetPoints(points);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("TOUCHE");
        }
    }
}
