using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class Boat : Enemy
    {
        public static int Propability = 35;
        public static int score = 30;
        public Boat(float health,float posx,float posy,int speed)
        {
            this.Health = health;
            this.GameObject= GameObject.Instantiate(Resources.Load("Prefabs/BoatPrefab", typeof(GameObject))) as GameObject;
            this.GameObject.transform.position = new Vector2(posx, posy);
            this.IsFlyingOver = false;
            this.Speed = speed;
            this.GameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(this.Speed * MainScript.Player.DefaultSpeed, 0);
            base.Score = score;
            MainScript.enemies.Add(this);
        }
    }
}
