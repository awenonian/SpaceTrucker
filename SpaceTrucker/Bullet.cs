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
        private static float moveSpeed = 1000f;

        private static Mesh bulletMesh;

        public Bullet(Vector2 position, Vector2 direction) : base(bulletMesh, position)
        {
            Speed = Vector2.Normalize(direction) * moveSpeed;
        }

        public Bullet(Mesh mesh, float moveSpeed) : base(mesh, Vector2.Zero)
        {
            Bullet.bulletMesh = mesh;
            Bullet.moveSpeed = moveSpeed;
        }
    }
}
