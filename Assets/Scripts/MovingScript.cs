using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

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
    // Use this for initialization
    void Start()
    {
        MaxSpeed = MainScript.Player.DefaultSpeed * 4;
        MinSpeed = MainScript.Player.DefaultSpeed / 1.4f; //TODO
        sounds = GetComponents<AudioSource>();
        if (sounds.Length > 0)
        {
            explosionSound = sounds[0];
            flightSound = sounds[1];
            normalFlightSoundPitch = flightSound.pitch;
            minFlightSoundPitch = flightSound.pitch-0.2f;
            maxFlightSoundPitch = flightSound.pitch+1f;
            flightSound.Play();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!MainScript.Player.Destroyed)
        {
            MainScript.Player.FuelLevel -= 0.025f;
            Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"), 0) * MoveForce;
            Vector2 speedVec = new Vector2(0, CrossPlatformInputManager.GetAxis("Vertical"));
            speedDelta = speedVec.y / 10000.0f;
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
            if (speedVec.y > 0)
            {
                if (MainScript.Player.ActualSpeed < MaxSpeed)
                {
                    MainScript.Player.ActualSpeed += speedDelta;
                    if(flightSound!=null && flightSound.pitch<maxFlightSoundPitch) flightSound.pitch += pitchDifference;
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
                speedDelta = 0.9f / 10000.0f;
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
            MainScript.Player.PlayerBody.AddForce(moveVec);
            MainScript.Player.PlayerBody.transform.position = new Vector3(MainScript.Player.PlayerBody.transform.position.x, MainScript.Player.PlayerBody.transform.position.y + MainScript.Player.ActualSpeed);
            if (moveVec == Vector2.zero) MainScript.Player.PlayerBody.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.tag == "Terrain" || collider.tag == "Finish" || collider.tag == "Enemy")
        //{
<<<<<<< HEAD
        //    MainScript.Player.Destroyed = true;
        //    MainScript.Player.PlayerBody.velocity = Vector2.zero;
        //    MainScript.Player.Lives -= 1;
        //    normalSpeedSound.Stop();
        //    explosion.Play();
=======
        //    MainScript.KillPlayer();
>>>>>>> 0882ffcd4093d696b38e547ac0ed8c1626fd2b80
        //}
    }
}
