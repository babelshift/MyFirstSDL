using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class DrawableEntity : Entity
	{
		protected Sprite Sprite { get; set; }

		public DrawableEntity(Sprite sprite)
			: base(Vector.Zero, Vector.Zero)
		{
			Sprite = sprite;
		}

		public DrawableEntity(Sprite sprite, Vector position, Vector speed)
			: base(position, speed)
		{
			Sprite = sprite;
		}

		public void Draw(Renderer renderer, GameTime gameTime)
		{
			Sprite.Draw(renderer, Position, gameTime);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DrawableEntity()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if (Sprite != null)
				Sprite.Dispose();
		}
	}
}
