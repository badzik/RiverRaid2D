using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Assets;
using System.Collections.Generic;

public class MainScript : MonoBehaviour {

    public static Player Player;
    public static List<Missile> missiles;

	void Start () {
        Player = new Player(this.GetComponent<Rigidbody2D>());
        Player.UpdateBoxCollider();
        missiles = new List<Missile>();
	}
	

	void FixedUpdate () {
    }
}
