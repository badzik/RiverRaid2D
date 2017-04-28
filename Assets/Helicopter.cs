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
        public static int score = 60;
        public Helicopter(float health, float posx, float posy,int speed)
        {
            this.Health = health;
            this.Speed = speed;
            this.GameObject = GameObject.Instantiate(Resources.Load("Prefabs/HelicopterPrefab", typeof(GameObject))) as GameObject;
            this.GameObject.transform.position = new Vector2(posx, posy);
            this.IsFlyingOver = false;
            this.Speed = speed;
            base.Score = score;
            MainScript.enemies.Add(this);
        }
    }
}
