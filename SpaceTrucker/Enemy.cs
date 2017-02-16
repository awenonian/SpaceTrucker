using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SpaceTrucker
{
    class Enemy : Ship
    {
        public Enemy(Mesh mesh, Vector2 position, float moveSpeed, float thrust, float turnSpeed) : base(mesh, position, moveSpeed, thrust, turnSpeed)
        {
        }

        public override void update(GameTime gameTime, int width, int height)
        {
            base.update(gameTime, width, height);
        }
    }
}
