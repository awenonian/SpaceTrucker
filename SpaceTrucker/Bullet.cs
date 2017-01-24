using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceTrucker
{
    class Bullet : Object
    {
        public Bullet(Mesh mesh, Vector2 position, Vector2 speed) : base(mesh, position)
        {
            Speed = speed;
        }
    }
}
