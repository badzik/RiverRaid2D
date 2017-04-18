using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class FuelTank
    {
        private GameObject gameObject;
        public static int Propability = 20;
        public FuelTank(float x,float y)
        {
            this.GameObject = GameObject.Instantiate(Resources.Load("Prefabs/FuelTankPrefab", typeof(GameObject))) as GameObject;
            this.GameObject.transform.position = new Vector2(x, y);
            MainScript.fuelTanks.Add(this);
        }

        public GameObject GameObject
        {
            get { return gameObject; }
            set { gameObject = value; }
        }
    }
}
