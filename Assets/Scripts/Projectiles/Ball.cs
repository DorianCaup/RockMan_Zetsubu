using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	public Vector3 Direction;
	public int Damage;
	public bool AnyEnnemieHitted;
	public Animator m_Anim;
	public bool m_FacingRight = true;

	public AudioClip shootClassicSound;
	public AudioClip shootMediumSound;
	public AudioClip shootFullSound;

	private AudioSource sound;

	GameObject Shooter;

	// Use this for initialization
	void Start () 
	{
		m_Anim = GetComponent<Animator>();
		Shooter = GameObject.FindGameObjectWithTag("Player");
		Vector3 StartPosition;
		AnyEnnemieHitted = false;
		if(Shooter.GetComponent<Player>().m_FacingRight)
			StartPosition = new Vector3 (Shooter.transform.position.x +0.15f, Shooter.transform.position.y, Shooter.transform.position.z);
		else StartPosition = new Vector3 (Shooter.transform.position.x -0.15f, Shooter.transform.position.y, Shooter.transform.position.z);
		gameObject.transform.position = StartPosition;
		if (Shooter.transform.lossyScale.x < 0)
			Flip ();
		Destroy (gameObject, 4);
	}

	public void Set_Ball(float timeShoot, bool Sliding, bool FaceSliding)
	{
		sound = GetComponent<AudioSource>();
		Shooter = GameObject.FindGameObjectWithTag("Player");
		if (timeShoot > 1f && timeShoot < 2f) 
		{
			m_Anim.SetBool("UnCharged", false);										
			m_Anim.SetBool("MiddleCharged", true);
			m_Anim.SetBool("FullCharged", false);
			sound.PlayOneShot(shootMediumSound, 0.7f);
			Damage = 2;
		} else if (timeShoot > 2f) 
		{
			m_Anim.SetBool("UnCharged", false);
			m_Anim.SetBool("MiddleCharged", false);
			m_Anim.SetBool("FullCharged", true);
			sound.PlayOneShot(shootFullSound, 0.7f);
			Damage = 3;
		} else 
		{
			m_Anim.SetBool("UnCharged", true);
			m_Anim.SetBool("MiddleCharged", false);
			m_Anim.SetBool("FullCharged", false);
			sound.PlayOneShot(shootClassicSound, 0.7f);
			Damage = 1;
		}
		if (Sliding) {
			if (FaceSliding)
			{
				Direction = new Vector3 (0.05f, 0, 0);	
			} else Direction = new Vector3 (-0.05f, 0, 0);	
		} else 
		{
			if (Shooter.transform.lossyScale.x > 0) 
				Direction = new Vector3(0.05f, 0, 0);	
			else Direction = new Vector3(-0.05f, 0, 0);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		gameObject.transform.position += Direction;
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

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag != "Player") 
		{
			Destroy(gameObject);
		}
	}
}
