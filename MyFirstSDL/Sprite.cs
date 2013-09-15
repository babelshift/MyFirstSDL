using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public class Sprite
	{
		public Image Image { get; private set; }
		public int Width { get { return Image.Texture.Width; } }
		public int Height { get { return Image.Texture.Height; } }

		public static Random Random = new Random();

		public Sprite(Image image)
		{
			Image = image;
		}

		public void Draw(Renderer renderer, Vector position, GameTime gameTime)
		{
			renderer.RenderTexture(Image.Texture, position.X, position.Y);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~Sprite()
		{
			Dispose(false);
		}

		private void Dispose(bool disposing)
		{
			if(Image != null)
				Image.Dispose();
		}
	}
}
