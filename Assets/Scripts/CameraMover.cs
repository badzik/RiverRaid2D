using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

            Vector3 pos = transform.position;
            pos.y = MainScript.Player.PlayerBody.transform.position.y + 0.3f;
            transform.position = pos;

    }

}
