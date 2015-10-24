using UnityEngine;
using System.Collections;

public class WallWithDoor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        MatriceMap MatriceMap = FindObjectOfType<MatriceMap>();
        Player player = FindObjectOfType<Player>();
        if (player.Pos_X_Matrice > 0 && player.Pos_X_Matrice < (MatriceMap.composition - 1))
        {
            foreach (var s in GetComponentsInChildren<SpriteRenderer>())
                s.enabled = false;
        }
        else if (player.Pos_X_Matrice == 0)
        {
            GameObject Door = GameObject.FindGameObjectWithTag("Doors_Right");
            foreach (var s in Door.GetComponentsInChildren<SpriteRenderer>())
                s.enabled = false;
        }
        else if (player.Pos_X_Matrice == (MatriceMap.composition - 1))
        {
            GameObject Door = GameObject.FindGameObjectWithTag("Doors_Left");
            foreach (var s in Door.GetComponentsInChildren<SpriteRenderer>())
                s.enabled = false;
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            MatriceMap MatriceMap = FindObjectOfType<MatriceMap>();
            Player player = FindObjectOfType<Player>();
    
            if (player.transform.lossyScale.x < 0)
                MatriceMap.Next_Map_On_Left(player.Pos_X_Matrice, player.Pos_Y_Matrice);
            else MatriceMap.Next_Map_On_Right(player.Pos_X_Matrice, player.Pos_Y_Matrice);
        }
    }
}
