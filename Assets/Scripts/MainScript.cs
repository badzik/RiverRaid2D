using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Assets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = Player.Points.ToString();
        
    }

    public static void KillPlayer()
    {
        Player.Destroyed = true;
        Player.PlayerBody.velocity = Vector2.zero;
        Player.Lives -= 1;
        var player = GameObject.Find("Player");
        player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Explosions/playerExplosion", typeof(Sprite)) as Sprite;
        if (Player.Lives > 0)
        {
            Destroy(GameObject.Find("Live" + (Player.Lives + 1).ToString()));
        }
        //freeze all enemies
        foreach(Enemy e in enemies)
        {
            e.GameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
    }

    public static void KillEnemy(Enemy enemy)
    {
        Player.Points+=enemy.Score;
        var anim = enemy.GameObject.GetComponent<Animator>();
        if (anim != null) anim.Stop();
        if (enemy.GameObject.name != "BoatPrefab(Clone)")
        {
            GameObject smallExplosion = GameObject.Instantiate(Resources.Load("Prefabs/SmallExplosionPrefab", typeof(GameObject))) as GameObject;
            smallExplosion.transform.position = new Vector2(enemy.GameObject.transform.position.x, enemy.GameObject.transform.position.y);
        }
        else
        {
            GameObject bigExplosion = GameObject.Instantiate(Resources.Load("Prefabs/BigExplosionPrefab", typeof(GameObject))) as GameObject;
            bigExplosion.transform.position = new Vector2(enemy.GameObject.transform.position.x, enemy.GameObject.transform.position.y);
           // Destroy(bigExplosion);
        }
        Destroy(enemy.GameObject);    //jeszcze z listy
        GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = Player.Points.ToString();
    }
}
