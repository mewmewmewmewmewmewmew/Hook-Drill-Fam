using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joystickinput : MonoBehaviour
{
    public GameObject player;

    Vector2 joystickPosition;

    Vector2 prevJoyPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        transform.position = new Vector3(this.player.transform.position.x + joystickPosition.y, this.player.transform.position.y + joystickPosition.x, -1);

        this.prevJoyPos = this.joystickPosition;
    }
}
