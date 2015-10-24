using UnityEngine;
using System.Collections;

public class CameraOnPlayer : MonoBehaviour {

    public Player player;
    public bool IsFollowing = true;
    public float xOffset;
    public float yOffset;

    private Vector2 yzStartPlayer;

	// Use this for initialization
	void Start () {
        this.player = FindObjectOfType<Player>();
        yzStartPlayer = new Vector2(player.transform.position.y, player.transform.position.z - 10);
        
	}
	
	// Update is called once per frame
	void Update () {
        if (IsFollowing && player != null)
        {
            //transform.position = new Vector3(player.transform.position.x + xOffset, yzStartPlayer.x + yOffset, yzStartPlayer.y);
            transform.position = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + yOffset, yzStartPlayer.y);
        }
        else if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }         
	}
}
