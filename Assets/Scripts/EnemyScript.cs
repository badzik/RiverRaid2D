﻿using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if ((Camera.main.transform.position.y - Camera.main.orthographicSize) > gameObject.transform.position.y)
        {
            MainScript.enemies.Remove(MainScript.enemies.Find(x => x.GameObject.Equals(gameObject)));
            Destroy(gameObject);
        }
        else if ((Camera.main.transform.position.y + Camera.main.orthographicSize) > gameObject.transform.position.y && gameObject.name == "AirPlanePrefab(Clone)")
        {
            var plane = MainScript.enemies.Find(x => x.GameObject.Equals(gameObject));
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(plane.Speed * MainScript.Player.DefaultSpeed, 0);
            int rotation = (plane.Speed > 0) ? 180:360; 
            gameObject.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.tag == "Terrain" || collider.tag == "Finish") && gameObject.name != "AirPlanePrefab(Clone)")
        {
            int rotation = 180;
            var enemySpeed = MainScript.enemies.Find(x => x.GameObject.Equals(gameObject)).Speed *= -1;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(enemySpeed * MainScript.Player.DefaultSpeed, 0);
            if (enemySpeed < 0 && gameObject.name == "BoatPrefab(Clone)")
            {
                    rotation += rotation;
                
            }
            if (enemySpeed > 0 && gameObject.name == "HelicopterPrefab(Clone)")
            {
                rotation += rotation;

            }
            if (enemySpeed > 0 && gameObject.name == "HelicopterPrefab(Clone)")
            {
                rotation += rotation;
            }
            gameObject.transform.localRotation = Quaternion.Euler(0, rotation, 0);
        }
    }
}
