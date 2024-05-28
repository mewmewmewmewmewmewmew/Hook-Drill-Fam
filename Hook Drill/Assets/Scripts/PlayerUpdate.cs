using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class PlayerUpdate : MonoBehaviour
{
    public UnityEvent braking;
    public UnityEvent enterGround;
    public UnityEvent exitGround;
    public UnityEvent RopeCollide;
    public UnityEvent GliderMode;


    public PlayerScriptableObject _playerValues;
    public CameraObject _cameraValues;

    public Camera cam;

    public BoxCollider2D _playerCollider;
    public HingeJoint2D _playerJoint;
    public Rigidbody2D _playerRigidbody;

    public GameObject Objdirection;
    public GameObject Hook;
    public GameObject Glider;

    //public Text SpeedText;
    //public Text TurningTimeText;

    private bool isHooked;
    private bool isInGround;
    private bool DirectionTurn;
    private bool isBoosting;
    private bool vibrating;

    private float currentSpeed;
    private float boostBeforeSpeed;
    private float prevAngle;
    private float turningTime;
    private float boostAccelerationUpdtated;
    private float BoostTime;
    private float currentZoom;
    private float velocity;
    private float vibratingTime;
    private float hookCd;
    private float hookedMaxcd;
    private float GliderTime;

    static public float maxdistance;

    private Vector3 _playerVelocity;
    private Vector2 joyPos;
    private Vector2 PrevJoyPos;

    private void UpdateHooked()
    {
    }
    private void UpdateCamera()
    {
        this.currentZoom = this.currentSpeed * this._cameraValues.zoomMultiplier;

        if (!this.isBoosting)
            this.currentZoom = Mathf.Clamp(this.currentZoom, this._cameraValues.minimumZoom, this._cameraValues.maximumZoom);

        this.cam.orthographicSize = Mathf.SmoothDamp(this.cam.orthographicSize, this.currentZoom, ref this.velocity, this._cameraValues.smoothTime);
        this.cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);
    }
    private void UpdateInGroundDrill()
    {
        if (this._playerValues.acceleration != 0 && this._playerValues.speed > 0 && this._playerValues.speed <= this._playerValues.maxSpeedInGround)
            this.currentSpeed *= this._playerValues.acceleration;

        if (Input.GetButton("Fire2") && this.isInGround && this.currentSpeed >= this._playerValues.minSpeedInGround)
        {
            this.braking.Invoke();
            this.currentSpeed *= this._playerValues.BrakeDecelleration;
        }

        if(this.currentSpeed < this._playerValues.minSpeedInGround)
            this.currentSpeed = this._playerValues.minSpeedInGround;
    }
    private void UpdateOutofGroundDrill()
    {
        if (Input.GetButton("Glider") && this.GliderTime < this._playerValues.GliderTimeLimit)
        {
            this._playerVelocity.y = -this._playerValues.Glidergravity;
            this.GliderTime += Time.deltaTime;
            this.GliderMode.Invoke();
            this.Glider.SetActive(true);
            this.Glider.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.75f, this.transform.position.z);
        }
        else 
        {
            this._playerVelocity.y += (-this._playerValues.gravity) * Time.deltaTime;
            this.Glider.SetActive(false);
            if (this._playerVelocity.y < 0)
                this.currentSpeed *= this._playerValues.AirAcceleration;
        }
            
        if(this.currentSpeed > this._playerValues.minSpeedInAir && this._playerVelocity.y > 0)
            this.currentSpeed *= this._playerValues.AirDecelleration;

        float horizontalinput = Input.GetAxis("Horizontal");

        //if(horizontalinput != 0 && this.isBoosting)
        //    this._playerVelocity.x +=  horizontalinput * (this._playerValues.MovementSpeedInAir * this.boostAccelerationUpdtated) * Time.deltaTime;
        if(horizontalinput != 0)
            this._playerVelocity.x += horizontalinput * this._playerValues.MovementSpeedInAir * Time.deltaTime;
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
            this.currentSpeed = this.boostBeforeSpeed  + (this.boostAccelerationUpdtated * this._playerValues.boostCurve.Evaluate(this.BoostTime));

        if (this.isInGround) { this.UpdateInGroundDrill(); }
        else { this.UpdateOutofGroundDrill(); }

        this.JoystickHandler();

        this.CheckTheTurn();

        if (this.isInGround) 
            this._playerVelocity = transform.TransformDirection(Vector2.down) * this.currentSpeed;

        this.transform.position += this._playerVelocity * Time.deltaTime;
    }
    private void inputHandler()
    {
        if (Input.GetButton("Fire1") && !this.isInGround && this.hookCd > this.hookedMaxcd)
        {
            if(!this.isHooked)
            {
                this.isHooked = true;
                FollowingTrail.isHooked = this.isHooked;
                this.hookCd = 0;
                _playerJoint.connectedAnchor = new Vector2(this.Hook.transform.position.x, this.Hook.transform.position.y);
                //_playerJoint.anchor = new Vector2(maxdistance, 0); 
                Debug.Log(this.Hook.transform.position);
                _playerJoint.enabled = true;
                this._playerRigidbody.gravityScale = 1;
            }
            else if (this.isHooked)
            {
                this._playerRigidbody.gravityScale = 0;
                this.hookCd = 0;
                this.isHooked = false;
                _playerJoint.enabled = false;
                FollowingTrail.isHooked = this.isHooked;
            }

        }

    }
    private void CoolDownUpdate()
    {
        if (this.isBoosting && this.BoostTime < this._playerValues.boostTimeLimit + 0.5f)
            this.BoostTime += Time.deltaTime;

        if(this.BoostTime > this._playerValues.boostTimeLimit)
        {
            this.isBoosting = false;
            this.BoostTime = 0;
        }
            
        if (this.vibrating)
            this.vibratingTime += Time.deltaTime;

        if (this.vibratingTime > this._playerValues.VibrationTimeLinit)
        {
            this.vibrating = false;
            this.vibratingTime = 0;
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

        if (this.hookCd < this.hookedMaxcd + 0.5f)
            this.hookCd += Time.deltaTime;

    }
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 3, true);

        this.currentSpeed = this._playerValues.speed;
        this.prevAngle = -90;
        this.currentZoom = this._cameraValues.maximumZoom;
        this.vibrating = false;
        this.hookCd = 0;
        this.hookedMaxcd = 1f;
        this._playerJoint.enabled = false;
        this.isInGround = false;    
    }
    void Update()
    {
        this.CoolDownUpdate();
        this.inputHandler();

        //this.SpeedText.text = "Speed : " + string.Format("{0:0.00}", this.currentSpeed);
        //this.TurningTimeText.text = "TurningTime : " + string.Format("{0:0.00}", this.turningTime);

        if (!this.isHooked) { this.UpdateWithDrill(); }
        else if (this.isHooked) { this.UpdateHooked(); }

        if (this.vibrating && Gamepad.current != null)
            Gamepad.current.SetMotorSpeeds(this._playerValues.LowVibration, this._playerValues.HighVibration);

        this.UpdateCamera();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER COLLISION " + collision.tag);

        if (collision.CompareTag("Ground")) 
        {
            this.isInGround = true;
            collision.isTrigger = true;
            this.enterGround.Invoke();
            this.GliderTime = 0;
            this.Glider.SetActive(false);
        }
        else if (collision.CompareTag("Rope") && !isBoosting)
        {
            this.RopeCollide.Invoke();
            this.boostBeforeSpeed = this.currentSpeed;
            this.vibrating = true;
            this.isBoosting = true;
            float ratio =  this.turningTime / this._playerValues.MaxLoopTime;

            if (ratio > 1) { ratio = 1; }

            this.boostAccelerationUpdtated = this. _playerValues.MaxboostAcceleration  * ratio;

            if(boostAccelerationUpdtated < this._playerValues.MinBoostAcceleration)
                this.boostAccelerationUpdtated = this._playerValues.MinBoostAcceleration;

            this.turningTime = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")) 
        {
            this.isInGround = false;
            collision.isTrigger = false;
            this.exitGround.Invoke();
        }
    }
}
