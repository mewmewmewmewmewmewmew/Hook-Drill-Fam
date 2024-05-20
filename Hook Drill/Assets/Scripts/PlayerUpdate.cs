using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpdate : MonoBehaviour
{
    public PlayerScriptableObject _playerValues;
    public CameraObject _cameraValues;

    public Camera cam;

    public BoxCollider2D _playerCollider;

    public GameObject Objdirection;
    public GameObject Hook;

    public Text SpeedText;
    public Text TurningTimeText;

    private bool isGrounded;
    private bool DrillMode;
    private bool isInGround;
    private bool DirectionTurn;
    private bool isBoosting;

    private float changeTime;
    private float currentSpeed;
    private float prevAngle;
    private float turningTime;
    private float boostAccelerationUpdtated;
    private float BoostTime;
    private float currentZoom;
    private float velocity;
    private float zoomMultiplier;

    private Vector3 _playerVelocity;
    private Vector2 joyPos;
    private Vector2 PrevJoyPos;

    private void UpdateCamera()
    {
        this.currentZoom = this.currentSpeed * this.zoomMultiplier;
        if(!this.isBoosting ) 
            this.currentZoom = Mathf.Clamp(this.currentZoom, this._cameraValues.minimumZoom, this._cameraValues.maximumZoom);

        this.cam.orthographicSize = Mathf.SmoothDamp(this.cam.orthographicSize, this.currentZoom, ref this.velocity, this._cameraValues.smoothTime);
        this.cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }
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
        if (this._playerValues.acceleration != 0 && this._playerValues.speed > 0 && this._playerValues.speed <= this._playerValues.maxSpeedInGround)
            this.currentSpeed *= this._playerValues.acceleration;
    }
    private void UpdateOutofGroundDrill()
    {
        this._playerVelocity.y += (-this._playerValues.gravity) * Time.deltaTime;

        if(this.currentSpeed > this._playerValues.minSpeedInAir && this._playerVelocity.y > 0)
            this.currentSpeed *= this._playerValues.AirDecelleration;

        if (this._playerVelocity.y < 0)
            this.currentSpeed *= this._playerValues.AirAcceleration;
    }
    private void JoystickHandler()
    {
        float XaxisJoy = Input.GetAxis("Vertical");
        float YAxisJoy = Input.GetAxis("Horizontal");

        this.joyPos = new Vector2(XaxisJoy, YAxisJoy);

        Vector3 direction = new Vector3(this.transform.position.x + joyPos.y, this.transform.position.y + joyPos.x, -1);

        this.Objdirection.transform.position = direction;

        float angle = Mathf.Atan2(direction.y - transform.position.y, direction.x - transform.position.x) * Mathf.Rad2Deg;

        if (angle == 0) { angle = this.prevAngle; }

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, this._playerValues.rotationSpeed);

        this.prevAngle = angle;
    }
    private void CheckTheTurn()
    {
        float angle = Vector2.SignedAngle(this.joyPos, this.PrevJoyPos);

        if(angle < 0)
        {
            if(!DirectionTurn)
                this.turningTime = 0;

            DirectionTurn = true;
            this.turningTime += Time.deltaTime;
        }
        else if(angle > 0)
        {
            if(DirectionTurn)
                this.turningTime = 0;

            DirectionTurn = false;
            this.turningTime += Time.deltaTime;
        }


        this.PrevJoyPos = this.joyPos;
    }
    private void UpdateWithDrill()
    {
        if (!this.isBoosting && this.currentSpeed > this._playerValues.maxSpeedInGround && this.isInGround)
            this.currentSpeed = this._playerValues.maxSpeedInGround;

        if (this.isBoosting && this.BoostTime < this._playerValues.boostTimeLimit)
            this.currentSpeed *= this.boostAccelerationUpdtated;

        if (this.isInGround) { this.UpdateInGroundDrill(); }
        else { this.UpdateOutofGroundDrill(); }

        this.JoystickHandler();

        this.CheckTheTurn();

        if (this.isInGround) 
            this._playerVelocity = transform.TransformDirection(Vector2.down) * this.currentSpeed;

        this.transform.position += this._playerVelocity * Time.deltaTime;
    }
    public void switchToBaseMode()
    {
        Debug.Log("SWITCH TO BASE");
        this.DrillMode = false;
        this.isGrounded = false;
        this._playerCollider.isTrigger = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        this.Hook.SetActive(false);
        this.changeTime = 0;
        this.currentSpeed = this._playerValues.minSpeedInGround;
    }   
    private void inputHandler()
    {
        if (Input.GetButton("Fire1") && this.changeTime >= this._playerValues.changeTimeLimit)
        {
            if (this.DrillMode && !this.isInGround)
            {
                Debug.Log("SWITCH WITH INPUT TO BASE");
                this.switchToBaseMode();
                return;
            }

            if (!this.DrillMode && this.isGrounded)
            {
                this.isInGround = true;
                this.DrillMode = true;
                this._playerCollider.isTrigger = true;
                this.Hook.SetActive(true);
                Debug.Log("SWITCH TO DRILL");
                this.changeTime = 0;
            }
        }
    }
    private void CoolDownUpdate()
    {
        if (this.changeTime <= this._playerValues.changeTimeLimit + 0.5f)
            this.changeTime += Time.deltaTime;

        if (this.isBoosting && this.BoostTime < this._playerValues.boostTimeLimit + 0.5f)
            this.BoostTime += Time.deltaTime;

        if (!this.isBoosting)
            this.BoostTime = 0;

        if(this.BoostTime > this._playerValues.boostTimeLimit)
            this.isBoosting = false;
    }
    void Start()    
    {
        Physics2D.IgnoreLayerCollision(7, 3, true);

        this.currentSpeed = this._playerValues.speed;
        this.isGrounded = false;
        this.prevAngle = -90;
        this.Hook.SetActive(false);
        this.currentZoom = this._cameraValues.maximumZoom;
        this.zoomMultiplier = this._playerValues.minSpeedInGround / this._cameraValues.minimumZoom;
    }
    void Update()
    {
        this.CoolDownUpdate();
        this.inputHandler();

        this.SpeedText.text = "Speed : " + string.Format("{0:0.00}", this.currentSpeed);
        this.TurningTimeText.text = "TurningTime : " + string.Format("{0:0.00}", this.turningTime);

        if (!this.DrillMode) { this.UpdateNoDrill(); } 

        if (this.DrillMode) { this.UpdateWithDrill(); }

        this.UpdateCamera();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER " + collision.tag);
        
        if (this.DrillMode && collision.CompareTag("Ground")) 
        { 
            this.isInGround = true;
            collision.isTrigger = true;
        }
        else if (this.DrillMode && collision.CompareTag("Rope") && !isBoosting)
        {
            this.isBoosting = true;
            float ratio =  this.turningTime / this._playerValues.MaxLoopTime;

            if (ratio > 1) { ratio = 1; }

            this.boostAccelerationUpdtated = (this._playerValues.MaxboostAcceleration - 1) * ratio;

            this.boostAccelerationUpdtated += 1;
        }
        else if (!this.DrillMode) { Debug.Log(" NO DRILL"); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("EXIT " + collision.tag);

        if (this.DrillMode && collision.CompareTag("Ground")) 
        {
            this.isInGround = false;
            collision.isTrigger = false;
        }
        else if (!this.DrillMode) { Debug.Log(" NO DRILL"); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
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
