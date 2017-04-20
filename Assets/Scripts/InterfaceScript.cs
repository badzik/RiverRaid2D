using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class InterfaceScript : MonoBehaviour
    {
        static float minX = -0.31f, maxX = 0.31f;
        private GameObject fuelIndicator;
        void Start()
        {
            fuelIndicator = GameObject.Find("FuelIndicator");
        }

        void FixedUpdate()
        {
            float resut = (MainScript.Player.FuelLevel  * (maxX - minX))/100;
            fuelIndicator.transform.position = new Vector3(minX +  resut, fuelIndicator.transform.position.y);
        }
    }
}
