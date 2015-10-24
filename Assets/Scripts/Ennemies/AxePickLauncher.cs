using UnityEngine;
using System.Collections;

public class AxePickLauncher : MonoBehaviour {
    public int AxeDamage = 3;
    public float SpeedAxePick = 2f;
    public float FrequencyAxePick = 2f;
    public bool FacingRight = false;
    public Transform AxePick;

    Vector2 DirectionAxePick;
    float Current_Frequency;
    float Timer_Anim_AxePickLaunch = 0.35f;
    bool Anim_PickAxeLaunched = false;
    Animator m_Animation;    

	// Use this for initialization
	void Start () {
        Current_Frequency = FrequencyAxePick;
        m_Animation = GetComponent<Animator>();

        if (FacingRight)
            Flip();         
	}
	
	// Update is called once per frame
	void Update () {
        LaunchingAxe();
	}

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void LaunchingAxe()
    {
        if (Anim_PickAxeLaunched && Timer_Anim_AxePickLaunch >= 0)
            Timer_Anim_AxePickLaunch -= Time.deltaTime;
        if (Current_Frequency == FrequencyAxePick)
        {
            m_Animation.SetBool("Launching", true);
            Current_Frequency -= Time.deltaTime;
            Anim_PickAxeLaunched = true;
        } else
        {
            if (Timer_Anim_AxePickLaunch <= 0)
            {
                var newAxePick = Instantiate(AxePick.transform) as Transform;
                newAxePick.GetComponent<AxePick>().Set_AxePick(FacingRight, transform.position);
                m_Animation.SetBool("Launching", false);
                Timer_Anim_AxePickLaunch = 0.35f;
                Anim_PickAxeLaunched = false;
            }              
            Current_Frequency -= Time.deltaTime;
            if (Current_Frequency <= 0)
                Current_Frequency = FrequencyAxePick;
        }
    }
}
