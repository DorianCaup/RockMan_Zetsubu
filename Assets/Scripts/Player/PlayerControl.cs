using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private Player m_Character;
	private bool m_Jump;
    private bool m_Climb;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
		m_Character = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_Jump) {
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = Input.GetKeyDown (KeyCode.Space);
		}
	}

	private void FixedUpdate()
	{
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		bool chargingShoot = Input.GetKey(KeyCode.X);
        bool climbing = Input.GetKey(KeyCode.UpArrow);
		float h = Input.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		m_Character.Move(h, crouch, m_Jump);
		m_Character.Shoot(chargingShoot);
        //m_Character.Climb(climbing);
		m_Jump = false;
	}
}
