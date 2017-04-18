using UnityEngine;
using System.Collections;

public class FuelTankScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((Camera.main.transform.position.y - Camera.main.orthographicSize) > gameObject.transform.position.y)
        {
            MainScript.enemies.Remove(MainScript.enemies.Find(x => x.GameObject.Equals(gameObject)));
            Destroy(gameObject);
        }
    }
}
