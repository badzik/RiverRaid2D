using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class Helicopter : Enemy
    {
        public static int Propability = 35;
        public Helicopter(float health, float posx, float posy,int speed)
        {
            this.Health = health;
            this.Speed = speed;
            this.GameObject = GameObject.Instantiate(Resources.Load("Prefabs/HelicopterPrefab", typeof(GameObject))) as GameObject;
            this.GameObject.transform.position = new Vector2(posx, posy);
            this.IsFlyingOver = false;
            this.Speed = speed;
            this.GameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Speed * MainScript.Player.DefaultSpeed, 0);
            MainScript.enemies.Add(this);
        }
    }
}
