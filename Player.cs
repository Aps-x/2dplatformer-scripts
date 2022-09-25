using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class Player : MonoBehaviour
{
    #region Header
    [Header("Components")]// Components
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D headCollider;
    public BoxCollider2D bodyCollider;
    [Header("Variables")] // Variables
    public float collisionRadius = 0.15f;
    Vector2 boxSize = new Vector2(0.75f, 0.2f);
    public Vector2 bottomOffset, rightOffset, leftOffset;
    [Space(10)]
    public float playerSpeed;
    public float midairSpeed;
    public float ladderSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float maxRBSpeed;
    [Space(10)]
    public float jumpInput;
    public float climbInput;
    public float crouchInput;
    public bool onLadder;
    public bool onGround;
    public bool leftWallGrab;
    public bool rightWallGrab;
    public bool onPlatform;
    public bool onConveyorBelt;
    [Space(10)]
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;
    // Hidden variables
    [HideInInspector] public float ladderExitTime;
    [HideInInspector] public float lastTeleportTime;
    [HideInInspector] public float ladderMaxHeight;
    [HideInInspector] public float ladderMinHeight;
    [HideInInspector] public float ladderPosX;
    [HideInInspector] public Vector2 movement;
    [HideInInspector] public LayerMask groundLayer = 1 << 6;
    [HideInInspector] public Collider2D oneWayPlatformCollider;
    [HideInInspector] public Collider2D platformCol;
    [HideInInspector] public Conveyor conveyor;
    // Controls & State
    public PlayerControls controls;
    public PlayerBaseState currentState;
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerDeathState deathState = new PlayerDeathState();
    public PlayerCrouchState crouchState = new PlayerCrouchState();
    public PlayerMidairState midairState = new PlayerMidairState();
    public PlayerRunningState runningState = new PlayerRunningState();
    public PlayerWallSlideState wallSlideState = new PlayerWallSlideState();
    public PlayerLadderClimbState ladderClimbState = new PlayerLadderClimbState();
	#endregion
	void Awake()
    {
        controls = new PlayerControls();
    }
    void OnEnable()
    {
        controls.Enable();
    }
    void Start()
    {
        currentState = idleState;
        currentState.EnterState(this);
    }
    void Update()
    {
        //Debug.Log(currentState.ToString());
        currentState.UpdateState(this);
		#region Wall Checks
		// Right wall check
		Collider2D rightWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        if (rightWallCollider != null && !rightWallCollider.gameObject.CompareTag("OneWayPlatform"))
        {
            rightWallGrab = true;
        }
        else
        {
            rightWallGrab = false;
        }
        // Left wall check
        Collider2D leftWallCollider = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        if (leftWallCollider != null && !leftWallCollider.gameObject.CompareTag("OneWayPlatform"))
		{
            leftWallGrab = true;
		}
		else
		{
            leftWallGrab = false;
		}
		#endregion
		// Ground check
		Collider2D groundCollider = Physics2D.OverlapBox((Vector2)transform.position - bottomOffset, boxSize, 0f, groundLayer);
        if (groundCollider != null)
		{
            // ConveyorBelt check
            if (groundCollider.gameObject.CompareTag("ConveyorBelt"))
			{
                rb.sharedMaterial = fullFriction;
                onConveyorBelt = true;
                conveyor = groundCollider.GetComponent<Conveyor>();
            }
            // Moving platform check
            if (groundCollider.gameObject.CompareTag("MovingPlatform") || groundCollider.gameObject.CompareTag("MovingPlatformSticky") && jumpInput == 0)
            {
                if (groundCollider.gameObject.CompareTag("MovingPlatformSticky"))
				{
                    platformCol = groundCollider.GetComponent<Collider2D>();
                    rb.interpolation = RigidbodyInterpolation2D.None;
                }
				else
				{
                    onPlatform = true;
                    playerSpeed = 10;
                }
                rb.sharedMaterial = fullFriction;
            }
			else
			{
                if (rb.interpolation == RigidbodyInterpolation2D.None)
                {
                    rb.interpolation = RigidbodyInterpolation2D.Interpolate;
                }
                platformCol = null;
                onPlatform = false;
                playerSpeed = 8;
            }
            // One way platform check
            if (groundCollider.gameObject.CompareTag("OneWayPlatform"))
			{
                // Allows player to pass through ground without becoming grounded
                if (rb.velocity.y > 0.1f)
				{
                    onGround = false;
                }
                else
                {
                    onGround = true;
                }
            }
            // Ground check
			else
			{
                onGround = true;
            }
        }
		else
		{
            // Not on ground or any other platform
            if (rb.interpolation == RigidbodyInterpolation2D.None)
			{
                rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            }
            onConveyorBelt = false;
            conveyor = null;
            onPlatform = false;
            platformCol = null;
            playerSpeed = 8;
            rb.sharedMaterial = noFriction;
            onGround = false;
		}
    }
	void LateUpdate()
	{
        if (platformCol != null)
        {
            transform.position = new Vector2(transform.position.x, platformCol.bounds.max.y + 1f);
        }
    }
	void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
        // Speed cap
        if (rb.velocity.magnitude > maxRBSpeed)
		{
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxRBSpeed);
		}
    }
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        platformCol = null;
        jumpInput = context.ReadValue<float>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }
    public void OnClimb(InputAction.CallbackContext context)
	{
        climbInput = context.ReadValue<float>();
	}
    public void OnCrouch(InputAction.CallbackContext context)
	{
        crouchInput = context.ReadValue<float>();
    }
	private void OnTriggerEnter2D(Collider2D other)
	{
        if (other.CompareTag("Ladder"))
        {
            onLadder = true;
            ladderPosX = other.transform.position.x;
            ladderMaxHeight = other.bounds.max.y;
            ladderMinHeight = other.bounds.min.y;
        }
        if (other.CompareTag("Hazard"))
		{
            if (currentState != deathState)
			{
                currentState.ExitState(this, deathState);
            }
		}
    }
	private void OnTriggerStay2D(Collider2D other)
	{
        if (other.CompareTag("WindArea"))
        {
            Vector2 forceOrigin = new Vector2(other.GetComponent<Collider2D>().bounds.center.x, other.GetComponent<Collider2D>().bounds.min.y);
            Vector2 forceTop = new Vector2(other.GetComponent<Collider2D>().bounds.center.x, other.GetComponent<Collider2D>().bounds.max.y);

            float distanceToPlayer = Vector2.Distance(transform.position, forceOrigin);
            float distanceToTop = Vector2.Distance(forceOrigin, forceTop);
            float distanceAsPercentage = (distanceToPlayer / distanceToTop) * 100;

            rb.AddForce(other.gameObject.transform.up * (100 - distanceAsPercentage) * 1.25f, ForceMode2D.Force);
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;
        }
    }
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("OneWayPlatform"))
		{
            oneWayPlatformCollider = other.gameObject.GetComponent<Collider2D>();
		}

    }
    public IEnumerator DropDown()
	{
        Physics2D.IgnoreCollision(bodyCollider, oneWayPlatformCollider);
        Physics2D.IgnoreCollision(headCollider, oneWayPlatformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(bodyCollider, oneWayPlatformCollider, false);
    }
	void OnDisable()
    {
        controls.Disable();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireCube((Vector2)transform.position - bottomOffset, boxSize);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}