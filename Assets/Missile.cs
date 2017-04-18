using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public abstract class Missile
    {
        private float damage;
        private int coolDown;
        private GameObject gameObject;

        public Missile(GameObject gameObject,float damage, int coolDown)
        {
            this.gameObject = gameObject;
            this.damage = damage;
            this.coolDown = coolDown;
        }

        public Missile()
        {

        }

        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public int CoolDown
        {
            get { return coolDown; }
            set { this.coolDown = value; }
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }
    }
}
