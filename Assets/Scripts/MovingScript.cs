using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;
using UnityEngine.SceneManagement;

public class MovingScript : MonoBehaviour
{

    public float MoveForce = 2;
    private float speedDelta;
    public float MaxSpeed;
    public float MinSpeed;
    float normalFlightSoundPitch;
    float maxFlightSoundPitch;
    float minFlightSoundPitch;
    float pitchDifference = 0.01f;
    AudioSource[] sounds;
    AudioSource flightSound;
    AudioSource explosionSound;
    AudioSource alert;
    bool isColliding;
    static bool wasPlayed = false;
    public LeftJoystick leftJoystick;
    private Vector3 leftJoystickInput;

    float xMov;
    float maxXMov;
    // Use this for initialization
    void Start()
    {
        xMov = 0;
        maxXMov = 0.01f;
        MaxSpeed = MainScript.Player.DefaultSpeed * 1.7f;
        MinSpeed = MainScript.Player.DefaultSpeed / 1.3f;
        sounds = GetComponents<AudioSource>();
        if (sounds.Length > 0)
        {
            explosionSound = sounds[0];
            flightSound = sounds[1];
            alert = sounds[3];
            normalFlightSoundPitch = flightSound.pitch;
            minFlightSoundPitch = flightSound.pitch - 0.2f;
            maxFlightSoundPitch = flightSound.pitch + 0.2f;
            flightSound.Play();
        }
    }

    void Update()
    {
        isColliding = false;
        
        if (!MainScript.Player.Destroyed && MainScript.Player.FuelLevel <= 25f && wasPlayed == false && SceneManager.GetActiveScene().isLoaded)
        {
            alert.Play();
            wasPlayed = true;
        }
        if (!MainScript.Player.Destroyed && MainScript.Player.FuelLevel >= 25f && wasPlayed == true && SceneManager.GetActiveScene().isLoaded)
        {
            alert.Stop();
            wasPlayed = false;
        }

        if (!MainScript.Player.Destroyed && MainScript.Player.FuelLevel <= 0f)
        {
            flightSound.Stop();
            alert.Stop();
            explosionSound.Play();
            MainScript.KillPlayer();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!MainScript.Player.Destroyed)
        {
            MainScript.Player.FuelLevel -= 0.025f;
            Vector2 moveVec = leftJoystickInput * MoveForce;
            Vector2 speedVec = leftJoystickInput;
            leftJoystickInput = leftJoystick.GetInputDirection();


            speedDelta = speedVec.y / 1000.0f;
            if (moveVec.x > 0)
            {
                MainScript.Player.PlayerBody.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Player/right", typeof(Sprite)) as Sprite;
            }
            if (moveVec.x < 0)
            {
                MainScript.Player.PlayerBody.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Player/left", typeof(Sprite)) as Sprite;
            }
            if (moveVec.x == 0)
            {
                MainScript.Player.PlayerBody.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Player/normal", typeof(Sprite)) as Sprite;
            }
            if (moveVec.x != 0)
            {
                if (Math.Sign(moveVec.x) != Math.Sign(xMov))
                {
                    xMov = 0.0f;
                }
                if (xMov < maxXMov && xMov > (-maxXMov)) xMov +=  0.0002f* moveVec.x;
            }
            if (speedVec.y > 0)
            {
                if (MainScript.Player.ActualSpeed < MaxSpeed)
                {
                    MainScript.Player.ActualSpeed += speedDelta;
                    if (flightSound != null && flightSound.pitch < maxFlightSoundPitch) flightSound.pitch += pitchDifference;
                }
            }
            if (speedVec.y < 0)
            {
                if (MainScript.Player.ActualSpeed > MinSpeed)
                {
                    MainScript.Player.ActualSpeed += speedDelta;
                    if (flightSound != null && flightSound.pitch > minFlightSoundPitch) flightSound.pitch -= pitchDifference;
                }
            }
            if (speedVec.y == 0.0f)
            {
                speedDelta = 1.5f / 10000.0f;
                if (MainScript.Player.DefaultSpeed <= MainScript.Player.ActualSpeed)
                {
                    MainScript.Player.ActualSpeed -= speedDelta;
                    if (flightSound != null && flightSound.pitch > normalFlightSoundPitch) flightSound.pitch -= pitchDifference;
                }
                if (MainScript.Player.DefaultSpeed >= MainScript.Player.ActualSpeed)
                {
                    MainScript.Player.ActualSpeed += speedDelta;
                    if (flightSound != null && flightSound.pitch < normalFlightSoundPitch) flightSound.pitch += pitchDifference;
                }
            }
            MainScript.Player.UpdateBoxCollider();
            MainScript.Player.PlayerBody.transform.position = new Vector3(MainScript.Player.PlayerBody.transform.position.x + xMov, MainScript.Player.PlayerBody.transform.position.y + MainScript.Player.ActualSpeed);
            if (moveVec == Vector2.zero) xMov = 0.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isColliding) return;
        isColliding = true;
        if (collider.tag == "Terrain" || collider.tag == "Finish" || collider.tag == "Enemy")
        {
            flightSound.Stop();
            explosionSound.Play();
            MainScript.KillPlayer();
        }
    }
}
