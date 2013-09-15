using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public interface ICollidable
	{
		void ResolveCollision(ICollidable collidable);
		Rectangle CollisionBox { get; }
		Vector Position { get; }
	}
}
