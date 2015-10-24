using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	// Use this for initialization
    public int NumberOfScenes;
    public string Monde;

    int selectedSceneID;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.selectedSceneID = (int)Random.Range(0, this.NumberOfScenes);
            Application.LoadLevel(this.selectedSceneID);
        }
    }
}
