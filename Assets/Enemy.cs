using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public abstract class Enemy
    {

        private float health;
        private GameObject gameObject;
        private bool isFlyingOver;
        public int Speed { get; set; }
        public int Score { get; set; }
        public Enemy(GameObject gameObject, float health,bool isFlyingOver,int speed,int score)
        {
            this.gameObject = gameObject;
            this.health = health;
            this.isFlyingOver = isFlyingOver;
            this.Speed = speed;
            this.Score = score;
        }

        public Enemy()
        {

        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }

        public bool IsFlyingOver
        {
            get { return isFlyingOver; }
            set { this.isFlyingOver = value; }
        }
    }
}
