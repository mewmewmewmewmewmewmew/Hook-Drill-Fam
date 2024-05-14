using UnityEngine;

public class PlayerUpdate : MonoBehaviour
{
    public PlayerScriptableObject _playerValues;
    public BoxCollider2D _playerCollider;
    public GameObject anchor;
    public GameObject Objdirection;

    private bool isGrounded;
    private bool DrillMode;
    private bool isInGround;

    private float changeTime;
    private float playerDeceleration;
    private float currentSpeed;
    private float prevAngle;

    private Vector3 _playerVelocity;
    private Vector2 joyPos;
   


    private void UpdateNoDrill()
    {
        if (this.isGrounded) 
        {
            this._playerVelocity.x = 0;
            this._playerVelocity.x += Input.GetAxis("Horizontal") * this._playerValues.speed;
        }

        this._playerVelocity.y += -this._playerValues.gravity * Time.deltaTime;
      
        if (isGrounded && this._playerVelocity.y < 0) { this._playerVelocity.y = 0; }
        this.transform.position += this._playerVelocity * Time.deltaTime;
    }
    private void UpdateInGroundDrill()
    {
        if (this._playerValues.acceleration != 0 && this._playerValues.speed > 0 && this._playerValues.speed <= this._playerValues.maxSpeed)
            this.currentSpeed *= this._playerValues.acceleration;
    }
    private void UpdateOutofGroundDrill()
    {
        this._playerVelocity.y += (-this._playerValues.gravity * this._playerValues.AirDecelleration) * Time.deltaTime;
    }
    private void UpdateWithDrill()
    {
        if (this.currentSpeed > this._playerValues.maxSpeed)
            this.currentSpeed = this._playerValues.maxSpeed;

        if (this.isInGround) { this.UpdateInGroundDrill(); }
        else { this.UpdateOutofGroundDrill(); }

        float XaxisJoy = Input.GetAxis("Vertical");
        float YAxisJoy = Input.GetAxis("Horizontal");

        this.joyPos = new Vector2(XaxisJoy, YAxisJoy);

        Vector3 direction = new Vector3(this.transform.position.x + joyPos.y, this.transform.position.y + joyPos.x, -1);

        this.Objdirection.transform.position = direction;

        float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;

        if(angle == 0) { angle = this.prevAngle; }
        
        Debug.Log("JOY POSITION : " + this.joyPos + "ANGLE : " + angle);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this._playerValues.rotationSpeed);

        this.prevAngle = angle;

        if (this.isInGround) 
        { 
            this._playerVelocity = transform.TransformDirection(Vector2.down) * this.currentSpeed;
        }

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
        this.currentSpeed = this._playerValues.minSpeed;
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
        if (this.changeTime <= this._playerValues.changeTimeLimit + 0.5f)
            this.changeTime += Time.deltaTime;
    }
    void Start()    
    {
        this.currentSpeed = this._playerValues.speed;
        this.isGrounded = false;
        this.playerDeceleration = 1;
        this.prevAngle = -90;
    }
    void Update()
    {
        this.anchor.transform.position = this.transform.position;
        this.CoolDownUpdate();
        this.inputHandler();

        if (!this.DrillMode) { this.UpdateNoDrill(); } 

        if (this.DrillMode) { this.UpdateWithDrill(); }

        //Debug.Log(joyPos);
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
        if (this.DrillMode) 
        { 
            Debug.Log("DRILL"); 
        }
        else if (!this.DrillMode) 
        {
            this.isGrounded = true;
        }

        
    }
}
