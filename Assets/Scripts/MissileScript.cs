using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {

    bool seen = false;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (GetComponent<Renderer>().isVisible)
            seen = true;

        if (seen && !GetComponent<Renderer>().isVisible)
        {
            MainScript.missiles.Remove(MainScript.missiles.Find(x => x.GameObject.Equals(gameObject)));
            Destroy(gameObject);
        }
            
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Terrain" || collider.tag == "Finish")
        {
            MainScript.missiles.Remove(MainScript.missiles.Find(x => x.GameObject.Equals(gameObject)));
            Destroy(gameObject);
        }
    }
}
