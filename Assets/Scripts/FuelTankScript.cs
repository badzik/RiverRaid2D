using UnityEngine;
using System.Collections;

public class FuelTankScript : MonoBehaviour
{
    AudioSource[] fuelSound;
    AudioSource startSound;
    AudioSource restSound;
    float refuelingSpeed = 0.25f;
    // Use this for initialization
    void Start()
    {
        fuelSound = GetComponents<AudioSource>();
        startSound = fuelSound[0];
        restSound = fuelSound[1];
    }

    // Update is called once per frame
    void Update()
    {
        if ((Camera.main.transform.position.y - Camera.main.orthographicSize) > gameObject.transform.position.y)
        {
            MainScript.enemies.Remove(MainScript.enemies.Find(x => x.GameObject.Equals(gameObject)));
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if(MainScript.Player.FuelLevel < 100)
            {
                MainScript.Player.FuelLevel += refuelingSpeed;
                startSound.Play();
            }
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (MainScript.Player.FuelLevel < 100)
            {
                MainScript.Player.FuelLevel += refuelingSpeed;
                if (!restSound.isPlaying && !startSound.isPlaying)
                {
                    restSound.Play();
                }
            }
        }
    }
}
