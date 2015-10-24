using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Un nombre est divisible par 2 si son dernier chiffre est 0, 2, 4, 6 ou 8.
    Un nombre est divisible par 3 si la somme de ses chiffres est divisible par 3.
    Un nombre est divisible par 4 si le nombre formé par ses deux derniers chiffres est divisible par 4.
    Un nombre est divisible par 5 si son dernier chiffre est 0 ou 5.
    Un nombre est divisible par 6 s'il est divisible à la fois par 2 ET par 3.
    Un nombre est divisible par 9 si la somme de ses chiffres est divisible par 9.
    Un nombre est divisible par 10 si son dernier chiffre est 0.*/

public class MatriceMap : MonoBehaviour {

    public int NeededScenes;
    public int TotalScenes;
    public string Monde;

    public int[,] map_Matrice { get; private set; }
    public int composition { get; private set; }

    private bool popOnLeft;
    private int selectedSceneID;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(transform.gameObject);
        this.NeededScenes = 10;
        this.TotalScenes = 5;
        string[] divisibleParDeux = new string[] { "0", "2", "4", "6", "8" };
        this.composition = 0;
        string sNbrScenes = this.NeededScenes.ToString();
        bool finded = false;

        foreach (string s in divisibleParDeux)
        {
            if (s == sNbrScenes[sNbrScenes.Length - 1].ToString())
                finded = true;
            if (finded)
                break;
        }
        if (!finded)
        {
            //Other ways
        }
        else
        {
            //Matrice divisible par 2
            this.composition = this.NeededScenes / 2;
            /*switch ((int)Random.Range(0, 1))
            {
                case 0:
                    this.map_Matrice = new int[composition, 2];

                    break;

                case 1:
                    this.map_Matrice = new int[2, composition];
                    break;
            }*/
            this.map_Matrice = new int[this.composition, 2];
        }
        for (int i = 0; i < this.composition; i++)
        {
            this.selectedSceneID = (int)Random.Range(0, this.TotalScenes);
            this.map_Matrice[i, 0] = this.selectedSceneID;
            this.selectedSceneID = (int)Random.Range(0, this.TotalScenes);
            this.map_Matrice[i, 1] = this.selectedSceneID;
        }
        this.map_Matrice[0, 0] = 1;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Next_Map_On_Right(int _positionActuelleX, int _positionActuelleY)
    {    
        Player player = FindObjectOfType<Player>();
        if (player.Pos_X_Matrice < this.composition - 1)
        {
            this.popOnLeft = true;
            if (this.map_Matrice[_positionActuelleX, _positionActuelleY] != this.map_Matrice[_positionActuelleX + 1, _positionActuelleY])
                Application.LoadLevel(this.map_Matrice[_positionActuelleX + 1, _positionActuelleY]);
            else Application.LoadLevel(Application.loadedLevel);
            player.Pos_X_Matrice += 1;
        }
    }

    public void Next_Map_On_Left(int _positionActuelleX, int _positionActuelleY)
    {
        Player player = FindObjectOfType<Player>();
        if (player.Pos_X_Matrice > 0)
        {    
            this.popOnLeft = false;
            if (this.map_Matrice[_positionActuelleX, _positionActuelleY] != this.map_Matrice[_positionActuelleX - 1, _positionActuelleY])
                Application.LoadLevel(this.map_Matrice[_positionActuelleX - 1, _positionActuelleY]);
            else Application.LoadLevel(Application.loadedLevel);
            player.Pos_X_Matrice -= 1;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        StartCoroutine(Wait(1));
        Player player = FindObjectOfType<Player>();
        GameObject popZone;
        if (popOnLeft)
            popZone = GameObject.FindGameObjectWithTag("PopLeft");
        else
            popZone = GameObject.FindGameObjectWithTag("PopRight");
        player.transform.position = popZone.transform.position;
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
