﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Player
    {
        private Rigidbody2D playerBody;
        private float fuelLevel;
        private int lives;
        private float defaultSpeed;
        private float actualSpeed;

        public Player(Rigidbody2D playerBody)
        {
            this.playerBody = playerBody;
            fuelLevel = 100;
            lives = 3;
            defaultSpeed = 0.001f;
            actualSpeed = defaultSpeed;
        }

        internal void UpdateBoxCollider()
        {
            Vector3 actualSize;
            actualSize = PlayerBody.GetComponent<SpriteRenderer>().bounds.size;
            PlayerBody.GetComponent<BoxCollider2D>().size = new Vector2(actualSize.x,actualSize.y);
        }

        public Rigidbody2D PlayerBody {
            get
            {
                return playerBody;
            }
            private set
            {
                playerBody = value;
            }
        }
        public float FuelLevel {
            get
            {
                return fuelLevel;
            }
            set
            {
                fuelLevel = value;
            }
        }

        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
            }
        }

        public float DefaultSpeed
        {
            get
            {
                return defaultSpeed;
            }
            set
            {
                defaultSpeed = value;
            }
        }

        public float ActualSpeed
        {
            get
            {
                return actualSpeed;
            }
            set
            {
                actualSpeed = value;
            }
        }

    }
}
