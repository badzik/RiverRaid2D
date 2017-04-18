using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Assets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour {

    public static Player Player;
    public static List<Missile> missiles;
    public static List<Enemy> enemies;
    public static List<FuelTank> fuelTanks;
    public static bool Init = false;
	void Start () {
        Player = new Player(this.GetComponent<Rigidbody2D>());
        Player.UpdateBoxCollider();
        missiles = new List<Missile>();
        enemies = new List<Enemy>();
        fuelTanks = new List<FuelTank>();
	}
	

	void FixedUpdate () {

    }
}
