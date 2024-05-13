using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerUpdate : MonoBehaviour
{
    public PlayerScriptableObject _playerValues;
    public BoxCollider2D _playerCollider;

    private bool isGrounded;
    private bool DrillMode;
    private bool isInGround;

    private float changeTime;
    private float playerDeceleration;
    private float airTime;

    private Vector3 _playerVelocity;

    private void UpdateNoDrill()
    {
        this._playerVelocity.x = 0;
        this._playerVelocity.y += -this._playerValues.gravity * Time.deltaTime;

        this._playerVelocity.x += Input.GetAxis("Horizontal") * this._playerValues.speed;

        if (isGrounded && this._playerVelocity.y < 0) { this._playerVelocity.y = 0; }
        this.transform.position += this._playerVelocity * Time.deltaTime;
    }
    private void UpdateWithDrill()
    {
        if (this._playerValues.speed > this._playerValues.maxSpeed)
            this._playerValues.speed = this._playerValues.maxSpeed;

        if (this.playerDeceleration != 0 && this._playerValues.speed > 0 && this._playerValues.speed <= this._playerValues.maxSpeed)
            this._playerValues.speed *= this.playerDeceleration;

        if (!this.isInGround) { this._playerVelocity.y += -this._playerValues.gravity * Time.deltaTime; }

        if (this.airTime >= this._playerValues.airTimeLimit) { this.switchToBaseMode(); return; }

        this._playerVelocity = transform.TransformDirection(Vector2.down) * this._playerValues.speed;

        this.transform.rotation *= Quaternion.Euler(0, 0, Input.GetAxis("Horizontal") * this._playerValues.rotationSpeed);

        this.transform.position += this._playerVelocity * Time.deltaTime;
    }
    public void switchToBaseMode()
    {
        Debug.Log("SWITCH TO BASE");
        this.DrillMode = false;
        this.isGrounded = false;
        this._playerCollider.isTrigger = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        this.changeTime = 0;
        this.airTime = 0;
    }   
    private void inputHandler()
    {
        if (Input.GetButton("Fire1") && this.changeTime >= this._playerValues.changeTimeLimit)
        {
            if (this.DrillMode && !this.isInGround)
            {
                Debug.Log("SWITCH WITH INPUT");
                this.switchToBaseMode();
                return;
            }

            if (!this.DrillMode && this.isGrounded)
            {
                this.isInGround = true;
                this.DrillMode = true;
                this._playerCollider.isTrigger = true;
                Debug.Log("SWITCH TO DRILL");
                this.changeTime = 0;
            }
        }
    }
    private void CoolDownUpdate()
    {
        if (!this.isInGround && this.DrillMode)
            this.airTime += Time.deltaTime;

        if (this.changeTime <= this._playerValues.changeTimeLimit + 0.5f)
            this.changeTime += Time.deltaTime;

    }
    void Start()    
    {
        this.isGrounded = false;
        this.playerDeceleration = 1;
    }
    void Update()
    {
        this.CoolDownUpdate();
        this.inputHandler();

        if (!this.DrillMode) { this.UpdateNoDrill(); } 

        if (this.DrillMode) { this.UpdateWithDrill(); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER " + collision.tag);

        if(this.DrillMode) 
        { 
            this.isInGround = true;
            collision.isTrigger = true;
        }
        else if (!this.DrillMode) { Debug.Log(" NO DRILL"); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("EXIT " + collision.tag);

        if (this.DrillMode) 
        {
            this.isInGround = false;
            collision.isTrigger = false;
        }
        else if (!this.DrillMode) { Debug.Log(" NO DRILL"); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLIDER");
        if (this.DrillMode) { Debug.Log("DRILL"); }
        else if (!this.DrillMode) 
        {
            this.isGrounded = true;
        }

        
    }
}
