using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

	// Use this for initialization
    public Transform respawnZone;
    public Transform player;
    public Player currentPlayer;
	void Start () {
        this.Spawn();
        this.currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.player = this.currentPlayer.GetComponent<Transform>();
	}

    private void Spawn()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject Player = (GameObject)Instantiate(Resources.Load("Objects/Megaman/Megaman"));
            Player.transform.position = respawnZone.transform.position;
        }
        if (GameObject.FindGameObjectWithTag("MatriceMap") == null)
        {
           Instantiate(Resources.Load("Objects/Events/MatriceMap"));
        }
        this.currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
        if (this.currentPlayer == null)
        {
            Instantiate(this.player, this.respawnZone.position, this.respawnZone.rotation);
            this.currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
	}
}
