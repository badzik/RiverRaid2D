using UnityEngine;
using System.Collections;
using Assets;

public class BridgeScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //    float t = Mathf.PingPong(Time.time, 1f);
        //    Camera.main.backgroundColor = Color.Lerp(Color.red, Color.black, t);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Missile")
        {
            float t = Mathf.PingPong(Time.time, 1f);
            Camera.main.backgroundColor = Color.Lerp(Color.red, Color.black, t);
            gameObject.SetActive(false);
            GameObject bigExplosion = GameObject.Instantiate(Resources.Load("Prefabs/BigExplosionPrefab", typeof(GameObject))) as GameObject;
            bigExplosion.transform.position = new Vector2(collider.gameObject.transform.position.x, collider.gameObject.transform.position.y);
            Destroy(bigExplosion, 3);
            MainScript.Player.Points += 500;
        }
    }
}
