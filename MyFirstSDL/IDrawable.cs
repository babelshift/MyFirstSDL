using SharpDL;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public interface IDrawable : IDisposable
	{
		void Draw(Renderer renderer, GameTime gameTime);
	}
}
