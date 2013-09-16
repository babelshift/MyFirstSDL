using SharpDL;
using SharpDL.Graphics;
using SharpDL.Input;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class PlayerEntity : DrawableEntity
	{
		public PlayerEntity(Sprite sprite, Vector Position, Vector speed)
			: base(sprite, Position, speed)
		{
			Activate();
		}

		public override void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed)
		{
			if (Status == EntityStatus.Active)
			{
				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.W))
					Position = new Vector(Position.X, Position.Y - Speed.Y);

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.A))
					Position = new Vector(Position.X - Speed.X, Position.Y);

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.S))
					Position = new Vector(Position.X, Position.Y + Speed.Y);

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.D))
					Position = new Vector(Position.X + Speed.X, Position.Y);
			}
		}
	}
}