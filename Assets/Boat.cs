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
        public Boat(float health,float posx,float posy)
        {
            this.Health = health;
            this.GameObject= GameObject.Instantiate(Resources.Load("Prefabs/BoatPrefab", typeof(GameObject))) as GameObject;
            this.GameObject.transform.position = new Vector2(posx, posy);
            this.IsFlyingOver = false;
            MainScript.enemies.Add(this);
        }
    }
}
