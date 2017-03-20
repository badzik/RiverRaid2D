using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Assets;

public class MainScript : MonoBehaviour {

    public static Player Player;


	void Start () {
        Player = new Player(this.GetComponent<Rigidbody2D>());
        Player.UpdateBoxCollider();
	}
	

	void FixedUpdate () {
    }
}
