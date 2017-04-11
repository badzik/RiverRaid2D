using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class NormalMissile : Missile
    {
        public NormalMissile(float damage,GameObject body) : base(body,damage,20)
        {

        }
    }
}
