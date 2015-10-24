using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
	[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .5f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	
	public int Health = 5;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	public bool m_FacingWallSlidRight = false;
	public float ShootTimer = 0f;
	public AudioClip startChargingshootaudio;
	public AudioClip loopChargingshootaudio;
    public AudioClip hitted;
	public GameObject ShotPrefab;

    public int Pos_X_Matrice;
    public int Pos_Y_Matrice;
	
	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool IsCharging;
	private bool Hitted = false;
	private bool WallSliding = false;
	private float Timer_CrouchSlide = 0f;
	private float Timer_Hitted = 0f;
	private AudioSource m_audio;
    private bool startSoundPlayed = false;


	//private BoxCollider2D m_HitBox;
	// Use this for initialization
	void Start () {
		// Setting up references.
        DontDestroyOnLoad(transform.gameObject);
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		m_audio = GetComponent<AudioSource> ();
		//m_HitBox = GetComponent<BoxCollider2D> ();
        this.Pos_Y_Matrice = 0;
        this.Pos_X_Matrice = 0;
	}

	private void FixedUpdate()
	{
		m_Grounded = false;
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders [i].gameObject != gameObject)
				m_Grounded = true;
		}
		m_Anim.SetBool ("Ground", m_Grounded);
		
		// Set the vertical animation
		m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
		if (WallSliding)
			IsWallSliding ();
	}

	private void IsWallSliding()
	{
		var m_Jump = Input.GetKeyDown(KeyCode.Space);
		if (m_Jump) 
		{
			if(m_Grounded && !m_Anim.GetBool ("WallJump"))
			{
				m_Anim.SetBool ("OnWall", false);
				m_Anim.SetBool ("WallJump", true);
				if(m_FacingWallSlidRight)
					transform.position += new Vector3(0.25f,0.2f,0); 
				else transform.position += new Vector3(-0.25f,0.2f,0);
			}
		} else if (m_Grounded && m_Anim.GetBool ("WallJump"))
		{
			m_Anim.SetBool ("OnWall", true);
			m_Anim.SetBool ("WallJump", false);
		}
		transform.position = new Vector3(transform.position.x, transform.position.y - 0.01f, transform.position.z);
		if (m_FacingRight)
			m_FacingWallSlidRight = false;
		else m_FacingWallSlidRight = true;
	}
	
	public void Shoot(bool chargingShoot)
	{
		if (chargingShoot) 
		{
            if (ShootTimer > 1f && !startSoundPlayed)
            {
                m_audio.PlayOneShot(startChargingshootaudio, 0.7f);
                startSoundPlayed = true;
            }
            else if (ShootTimer > 2f)
                m_audio.PlayOneShot(loopChargingshootaudio, 0.4f);
			IsCharging = true;
			if(IsCharging)
				ShootTimer += Time.deltaTime;
			if (m_Anim.GetBool("Ground"))
			{
				if (!m_Anim.GetBool("Crouch"))
				{
					if((m_Rigidbody2D.velocity.x > 0.1 || m_Rigidbody2D.velocity.x < -0.1))
					{
						m_Anim.SetBool("ChargingShootRun",true);
						m_Anim.SetBool("ChargingShootStatic",false);
						m_Anim.SetBool("ChargingShootJump",false);
						m_Anim.SetBool("ChargingShootCrouch", false);
						m_Anim.SetBool("Shoot",false);
					} else if (m_Rigidbody2D.velocity.x == 0)
					{
						m_Anim.SetBool("ChargingShootRun",false);
						m_Anim.SetBool("ChargingShootStatic",true);
						m_Anim.SetBool("ChargingShootJump",false);
						m_Anim.SetBool("ChargingShootCrouch", false);
						m_Anim.SetBool("Shoot",false);
					}
				} else 
				{
					m_Anim.SetBool("ChargingShootRun",false);
					m_Anim.SetBool("ChargingShootStatic",false);
					m_Anim.SetBool("ChargingShootJump",false);
					m_Anim.SetBool("ChargingShootCrouch", true);
					m_Anim.SetBool("Shoot",false);
				}
			} else 
			{
				m_Anim.SetBool("ChargingShootRun",false);
				m_Anim.SetBool("ChargingShootStatic",false);
				m_Anim.SetBool("ChargingShootJump",true);
				m_Anim.SetBool("ChargingShootCrouch", false);
				m_Anim.SetBool("Shoot",false);
			}
		} else if (!chargingShoot && IsCharging)
		{
			IsCharging = false;
			m_Anim.SetBool("ChargingShootRun",false);
			m_Anim.SetBool("ChargingShootStatic",false);
			m_Anim.SetBool("ChargingShootJump",false);
			m_Anim.SetBool("ChargingShootCrouch", false);
			m_Anim.SetBool("Shoot",true);
			var Ball = Instantiate(ShotPrefab.transform) as Transform;
			Ball.GetComponent<Ball>().Set_Ball(ShootTimer, WallSliding ,m_FacingWallSlidRight);
            startSoundPlayed = false;
			ShootTimer = 0f;
		} else
		{
			IsCharging = false;
			m_Anim.SetBool("ChargingShootRun",false);
			m_Anim.SetBool("ChargingShootStatic",false);
			m_Anim.SetBool("ChargingShootJump",false);
			m_Anim.SetBool("ChargingShootCrouch", false);
			m_Anim.SetBool("Shoot",false);
		}
	}
	
	public void Move(float move, bool crouch, bool jump)
	{
		if(!crouch)
			Timer_CrouchSlide = 0f;
		if (!crouch && m_Anim.GetBool("Crouch"))
		{
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}
		
		// Set whether or not the character is crouching in the animator
		/*if (m_Rigidbody2D.velocity.x > 0.1 || m_Rigidbody2D.velocity.x < 0.1)
            	m_Anim.SetBool("Crouch", crouch);
		    else m_Anim.SetBool("Crouch", false);*/
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move*m_CrouchSpeed : move);
			if (m_Rigidbody2D.velocity.x < 0.1 &&  m_Rigidbody2D.velocity.x > -0.1)
			{
				m_Anim.SetBool("Crouch", false);
				m_Anim.SetBool("ChargingShootCrouch", false);
			}
			else if (!crouch && m_Anim.GetBool("Crouch") && (m_Rigidbody2D.velocity.x > 0.1 || m_Rigidbody2D.velocity.x < -0.1))
			{
				m_Anim.SetBool("Crouch", false);
				m_Anim.SetBool("ChargingShootCrouch", false);
			}
			else if (crouch && (m_Rigidbody2D.velocity.x > 0.1 || m_Rigidbody2D.velocity.x < -0.1) && Timer_CrouchSlide <= 0.18f)
			{
				move = move*m_CrouchSpeed*1.25f;
				m_Anim.SetBool("Crouch", true);
				Timer_CrouchSlide += 0.01f;
			} else
			{
				m_Anim.SetBool("Crouch", false);
			}
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat("Speed", Mathf.Abs(move));
			
			// Move the character
			if (Hitted)
				move = 0;
			m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
			
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
		// If the player should jump...
		if (m_Grounded && jump && m_Anim.GetBool("Ground") && !m_Anim.GetBool("Hitted"))
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Anim.SetBool("Ground", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
        switch (collision.gameObject.tag)
        {
            case "Monster":
                Hitted = true;              
                if (Timer_Hitted == 0f)
                {
                    Health -= collision.gameObject.GetComponent<Ennemie>().Damage;
                    m_audio.PlayOneShot(hitted, 0.7f);
                }        
                break;
            case "SlideWall":
                break;
            case "DeadZone":
                Destroy(gameObject);
                break;
            case "AxePick":
                Hitted = true;              
                if (Timer_Hitted == 0f)
                {
                    m_audio.PlayOneShot(hitted, 0.7f);
                    Health -= collision.gameObject.GetComponent<AxePick>().Damage;
                }                  
                break;

        }
		
		if (collision.gameObject.tag == "SlideWall") 
		{
			if (m_Anim.GetFloat("Speed") < 1)
			{
				m_Anim.SetBool("OnWall", true);
				WallSliding = true;
			} else
			{
				WallSliding = false;
				m_Anim.SetBool("OnWall", false);
			}
		} else
		{
			WallSliding = false;
			m_Anim.SetBool("OnWall", false);
		}
	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	// Update is called once per frame
	void Update () {
        if (Hitted)
            Timer_Hitted += Time.deltaTime;	
		if (Timer_Hitted >= 0.5f) 
		{
			Hitted = false;
			Timer_Hitted = 0f;
		}
		m_Anim.SetBool ("Hitted", Hitted);
	}
}
