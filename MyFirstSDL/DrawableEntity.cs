using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class DrawableEntity : Entity, IDrawable
	{
		public Sprite Sprite { get; private set; }

		public DrawableEntity(Sprite sprite)
		{
			Sprite = sprite;
		}

		public void Draw(Renderer renderer, GameTime gameTime)
		{
			Sprite.Draw(renderer, gameTime);
		}
	}
}
