using UnityEngine;
using System.Collections;
using Assets;

public class FuelTankScript : MonoBehaviour
{
    AudioSource[] fuelSound;
    AudioSource startSound;
    AudioSource restSound;
    float refuelingSpeed = 0.25f;
    int score = 80;
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
            MainScript.fuelTanks.Remove(MainScript.fuelTanks.Find(x => x.GameObject.Equals(gameObject)));
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
            if(MainScript.Player.FuelLevel > 100)
            {
                if (!restSound.isPlaying)
                {
                    restSound.pitch = 1.08f;
                    restSound.Play();
                }
            }
        }
        if((collider.tag == "Missile"))
        {
            FuelTank fuelTank = MainScript.fuelTanks.Find(x => x.GameObject.Equals(gameObject));
            Missile missile = MainScript.missiles.Find(x => x.GameObject.Equals(collider.gameObject));
            MainScript.Player.Points += score;
            GameObject smallExplosion = GameObject.Instantiate(Resources.Load("Prefabs/SmallExplosionPrefab", typeof(GameObject))) as GameObject;
            smallExplosion.transform.position = new Vector2(fuelTank.GameObject.transform.position.x, fuelTank.GameObject.transform.position.y);
            Destroy(smallExplosion, 1);
            Destroy(fuelTank.GameObject);
            MainScript.fuelTanks.Remove(fuelTank);
            MissileScript.CheckTypeOfMissile(ref missile);
        }
    }
}
