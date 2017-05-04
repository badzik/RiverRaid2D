using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using Assets;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{

    public static Player Player;
    public static List<Missile> missiles;
    public static List<Enemy> enemies;
    public static List<FuelTank> fuelTanks;
    public static bool Init = false;
    public static bool start = true;

    static int flashingTime = 50;
    int flashingCounter;
    Color orgColor;
    GameObject firstStart = null;

    void Start()
    {
        flashingCounter = 0;
        orgColor = Camera.main.backgroundColor;

        Player = new Player(this.GetComponent<Rigidbody2D>());
        Player.UpdateBoxCollider();
        missiles = new List<Missile>();
        enemies = new List<Enemy>();
        fuelTanks = new List<FuelTank>();
        if (PlayerPrefs.HasKey("Lives")) Player.Lives = PlayerPrefs.GetInt("Lives");
        if (PlayerPrefs.HasKey("Score")) Player.Points = PlayerPrefs.GetInt("Score");
        if (PlayerPrefs.HasKey("Level")) Player.Level = PlayerPrefs.GetInt("Level");
        if (Player.Points == 0 && Player.Lives == 3)
        {
            firstStart = GameObject.Instantiate(Resources.Load("Prefabs/PressStart", typeof(GameObject))) as GameObject;
            firstStart.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.17f);
            Time.timeScale = 0;
            AudioListener.pause = true;
            start = true;
        }
        AdjustLives();
    }


    void FixedUpdate()
    {
        GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = Player.Points.ToString();
        if (BridgeScript.bridgeDestroyed)
        {
            if (flashingCounter <= flashingTime)
            {
                if (flashingCounter % 3 == 0)
                {
                    Camera.main.backgroundColor = Color.red;
                }
                else
                {
                    Camera.main.backgroundColor = orgColor;
                }
                flashingCounter++;
            }
            else
            {
                Camera.main.backgroundColor = orgColor;
                BridgeScript.bridgeDestroyed = false;
                flashingCounter = 0;
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && start)
        {
            Destroy(firstStart);
            Time.timeScale = 1;
            AudioListener.pause = false;
            start = false;
        }
        if (Player.Destroyed)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Player.Lives >= 0) ResetLevel();
                else 
                    ResetGame();
            }
        }
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void KillPlayer()
    {
        Player.Destroyed = true;
        Player.PlayerBody.velocity = Vector2.zero;
        Player.Lives -= 1;
        var player = GameObject.Find("Player");
        player.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Explosions/playerExplosion", typeof(Sprite)) as Sprite;
        if (Player.Lives >= 0)
        {
            GameObject presStart = GameObject.Instantiate(Resources.Load("Prefabs/PressStart", typeof(GameObject))) as GameObject;
            presStart.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.17f);
        }
        else
        {
            GameObject gameOver = GameObject.Instantiate(Resources.Load("Prefabs/GameOver", typeof(GameObject))) as GameObject;
            gameOver.transform.position = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y + 0.17f);
        }
        //freeze all enemies
        foreach (Enemy e in enemies)
        {
            e.GameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
        }
        
    }

    public static void KillEnemy(Enemy enemy)
    {
        Player.Points += enemy.Score;
        if (enemy.GameObject.name != "BoatPrefab(Clone)")
        {
            GameObject smallExplosion = GameObject.Instantiate(Resources.Load("Prefabs/SmallExplosionPrefab", typeof(GameObject))) as GameObject;
            smallExplosion.transform.position = new Vector2(enemy.GameObject.transform.position.x, enemy.GameObject.transform.position.y);
            Destroy(smallExplosion, 1);
        }
        else
        {
            GameObject bigExplosion = GameObject.Instantiate(Resources.Load("Prefabs/BigExplosionPrefab", typeof(GameObject))) as GameObject;
            bigExplosion.transform.position = new Vector2(enemy.GameObject.transform.position.x, enemy.GameObject.transform.position.y);
            Destroy(bigExplosion, 1);
        }
            Destroy(enemy.GameObject);
            enemies.Remove(enemy);
    }

    public void ResetLevel()
    {
        PlayerPrefs.SetInt("Score", Player.Points);
        PlayerPrefs.SetInt("Lives", Player.Lives);
        PlayerPrefs.SetInt("Level", Player.Level);
        SceneManager.LoadScene("gameplay");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("gameplay");
        PlayerPrefs.DeleteAll();
        Container.i.SavedLevel = 1;
    }

    public static void AdjustLives()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (i <= Player.Lives)
            {
                GameObject.Find("Live" + i).GetComponent<Renderer>().enabled = true;
            }
            else
            {
                GameObject.Find("Live" + i).GetComponent<Renderer>().enabled = false;
            }
        }
    }
}
