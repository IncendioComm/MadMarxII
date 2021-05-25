using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 500f;							// Amount of force added when the player jumps.
	
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.

	[SerializeField] private float m_MaxVelocity = 10f;  
		
		//llider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]


	// Saltar fuera  de plats
	public float fGroundedRemember = 0;
    public float fGroundedRememberTime = 0.2f;

	public UnityEvent OnLandEvent;


	public float checkRadius;
	bool isTouchingFront;
	public Transform frontCheck;
	bool wallSliding;
	public float wallSlidingSpeed;

	bool wallJumping;
	public float xWallForce;
	public float yWallForce;
	public float wallJumpTime;

	private bool canDoubleJump = false;


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				canDoubleJump = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();

			}
		}

	}


	public void Move(float move, bool crouch, bool jump, float fJumpPressedRemember)
	{
		float input = Input.GetAxisRaw("Horizontal");
		
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
					

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move *m_MaxVelocity, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		
		isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius, m_WhatIsGround);
		
		if (isTouchingFront == true && m_Grounded == false && input != 0)
        {
			wallSliding = true;
        }
		else
        {
			wallSliding = false;
        }
		if (wallSliding)
        {
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
			
		if (Input.GetButtonDown("Jump") && wallSliding == true)
        {
			wallJumping = true;
			Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
		
		if (wallJumping == true)
        {
			m_Rigidbody2D.velocity = new Vector2(xWallForce * -input, yWallForce);
		}

		// Saltar fuera  de las plats
		fGroundedRemember -= Time.deltaTime;
		
		if (m_Grounded) 
		{
			fGroundedRemember = fGroundedRememberTime;
			//canDoubleJump = true;
		}



		// The player should jump...
		if (fJumpPressedRemember > 0 && fGroundedRemember > 0)
		{

			// Add a vertical force to the player.
			//jump = true;
			m_Grounded = false;	
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			fJumpPressedRemember = 0;
			fGroundedRemember = 0;
			
		}
		else
		{
			if (Input.GetButtonDown("Jump") && canDoubleJump == true && m_Grounded == false)
			{
				//fGroundedRemember = 0;
				//fJumpPressedRemember = 0;
				//m_Grounded = false;
				m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				canDoubleJump = false;
				
			}
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}

	void SetWallJumpingToFalse()
    {
		wallJumping = false;
    }
}
