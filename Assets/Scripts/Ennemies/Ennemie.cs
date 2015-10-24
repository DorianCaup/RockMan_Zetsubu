using UnityEngine;
using System.Collections;

public class Ennemie : MonoBehaviour {

	
	private Animator m_Anim;
	private bool m_FacingRight = true;

	public int Health = 3;
	public float walkSpeed = 2.0f;
	public float wallLeft = 0.0f;
	public float wallRight = 5.0f;
	public int Damage = 1;
    public bool Turn = false;
    public bool Run = true;

	Vector3 Direction;
	float walkingDirection = 1.0f;
	Vector3 walkAmount;
	float DeadTimer = 0.35f;
	float startTransform;

	// Use this for initialization
	void Start () {
        if (Turn)
            Flip ();
		m_Anim = GetComponent<Animator>();
		startTransform = transform.position.x;
	}

	public void Loose_Health(int damage)
	{
		Health -= damage;
	}

	private void PatternMove()
	{
		walkAmount.x = walkingDirection * walkSpeed * Time.deltaTime;
        if (walkSpeed != 0)
        {
            if (walkingDirection > 0.0f && transform.position.x - startTransform >= wallRight)
            {
                walkingDirection = -1.0f;
                Flip();
            }
            else if (walkingDirection < 0.0f && transform.position.x - startTransform <= wallLeft)
            {
                walkingDirection = 1.0f;
                Flip();
            }
            transform.Translate(walkAmount);
        }
        m_Anim.SetBool("Run", Run);
		m_Anim.SetFloat ("Speed", walkSpeed);
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
		if (Health <= 0) 
		{
			m_Anim.SetBool("Dead", true);
			DeadTimer -= Time.deltaTime;
			if (DeadTimer <= 0)
				Destroy(gameObject);
		} else PatternMove ();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Projectil") 
		{
			Loose_Health(collision.gameObject.GetComponent<Ball>().Damage);
		}
	}
}
