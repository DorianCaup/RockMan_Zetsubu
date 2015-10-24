using UnityEngine;
using System.Collections;

public class AxePick : MonoBehaviour {

    public float SpeedAxePick = 2f;
    public float ForceAxePick = 400f;
    public int Damage = 2;

    Vector2 Position;
    bool FaceingRight;
    Rigidbody2D Rigidbody;
	// Use this for initialization
	void Start () {
        Rigidbody = GetComponent<Rigidbody2D>();      
        if (!FaceingRight)
        {
            SpeedAxePick = -SpeedAxePick;
            Position = new Vector2(Position.x - 0.1f, Position.y + 0.1f);
        } else Position = new Vector2(Position.x + 0.1f, Position.y + 0.1f);
        transform.position = Position;
        Rigidbody.AddForce(new Vector2(0f, ForceAxePick));
        Destroy(gameObject, 4);
	}
	
	// Update is called once per frame
	void Update () {
        Rigidbody.velocity = new Vector2(SpeedAxePick, Rigidbody.velocity.y);
	}

    public void Set_AxePick(bool facingRight, Vector2 position)
    {
        FaceingRight = facingRight;
        Position = position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Monster")
            Destroy(gameObject);
    }
}
