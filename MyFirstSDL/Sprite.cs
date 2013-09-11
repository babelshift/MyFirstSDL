using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class Sprite : IDrawable
	{
		public Texture Texture { get; private set; }
		public int Width { get { return Texture.Width; } }
		public int Height { get { return Texture.Height; } }

		public static Random Random = new Random();

		public Sprite(Texture texture)
		{
			Texture = texture;
		}

		public void Draw(Renderer renderer, GameTime gameTime)
		{
			renderer.RenderTexture(Texture, Random.Next(0, 1200), Random.Next(0, 800));
		}
	}
}
