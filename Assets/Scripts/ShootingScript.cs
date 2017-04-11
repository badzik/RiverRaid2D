using UnityEngine;
using System.Collections;
using Assets;

public class ShootingScript : MonoBehaviour
{

    private int shootCooldown;
    private Rect topLeft;
    private Rect topRight;
    private Rect bottomRight;

    // Use this for initialization
    void Start()
    {
        shootCooldown = 0;
        topLeft = new Rect(0, Screen.height / 2, Screen.width / 2, Screen.height / 2);
        topRight = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height / 2);
        bottomRight = new Rect(Screen.width / 2, Screen.height / 2, Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && shootCooldown <= 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touchPos = Input.GetTouch(i).position;
                if (Input.GetTouch(i).phase==TouchPhase.Began  && (topLeft.Contains(touchPos) || topRight.Contains(touchPos) || bottomRight.Contains(touchPos)))
                {
                    GameObject missile = Instantiate(Resources.Load("Prefabs/NormalMissilePrefab", typeof(GameObject))) as GameObject;
                    missile.AddComponent<MissileScript>();
                    missile.transform.position = new Vector2(MainScript.Player.PlayerBody.position.x, MainScript.Player.PlayerBody.position.y + MainScript.Player.PlayerBody.GetComponent<BoxCollider2D>().size.y);
                    missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 3000 * MainScript.Player.DefaultSpeed);
                    NormalMissile nm = new NormalMissile(100, missile);
                    shootCooldown = nm.CoolDown;
                    MainScript.missiles.Add(nm);

                }

            }

        }
    }

    void FixedUpdate()
    {
        if (shootCooldown >= 0) shootCooldown--;
    }
}
