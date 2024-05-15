using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisiosnHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 3, true);
        Physics2D.IgnoreLayerCollision(6, 7, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HELLLOOOOO");
    }
}
