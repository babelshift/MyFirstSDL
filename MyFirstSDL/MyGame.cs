using SharpDL;
using SharpDL.Events;
using SharpDL.Graphics;
using SharpDL.Input;
using SharpTiles;
using System;
using System.Collections.Generic;
using System.IO;

namespace MyFirstSDL
{
	public class MyGame : Game
	{
		private const int SCREEN_WIDTH = 1296;
		private const int SCREEN_HEIGHT = 728;
		private int xP = 0;
		private int yP = 0;

		private List<KeyInformation.VirtualKeyCode> keysPressed = new List<KeyInformation.VirtualKeyCode>();

		public MyGame()
		{
			KeyPressed += MyGame_KeyPressed;
			KeyReleased += MyGame_KeyReleased;
			MouseMoving += MyGame_MouseMoving;
			MouseButtonPressed += MyGame_MouseButtonPressed;
		}

		private void MyGame_MouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
			if (e.MouseButton == MouseButtonEventArgs.MouseButtonCode.Left)
			{
				xP = e.RelativeToWindowX;
				yP = e.RelativeToWindowY;
			}
		}

		private void MyGame_MouseMoving(object sender, MouseMotionEventArgs e)
		{
			xP = e.RelativeToWindowX;
			yP = e.RelativeToWindowY;
		}

		private void MyGame_KeyPressed(object sender, KeyboardEventArgs e)
		{
			if (!keysPressed.Contains(e.KeyInformation.VirtualKey))
				keysPressed.Add(e.KeyInformation.VirtualKey);
		}

		private void MyGame_KeyReleased(object sender, KeyboardEventArgs e)
		{
			if (keysPressed.Contains(e.KeyInformation.VirtualKey))
				keysPressed.Remove(e.KeyInformation.VirtualKey);
		}

		protected override void Initialize()
		{
			base.Initialize();

			CreateWindow("Lesson 2", 100, 100, SCREEN_WIDTH, SCREEN_HEIGHT, Window.WindowFlags.Shown);
			CreateRenderer(Renderer.RendererFlags.RendererAccelerated);
		}

		private TrueTypeText uiText;
		private Image tilePlainImage;
		private TiledMap map;

		protected override void LoadContent()
		{
			base.LoadContent();

			string text = String.Format("X: {0}, Y: {1}", xP, yP);
			SharpDL.Graphics.Color color = new SharpDL.Graphics.Color(255, 165, 0);
			Font font = new Font("Fonts/lazy.ttf", 28);
			Surface fontSurface = new Surface(font, text, color);
			uiText = new TrueTypeText(Renderer, fontSurface, text, font, color);

			//textTexture.LockTexture(fontSurface);

			Surface tilePlainSurface = new Surface("Images/Tile_Plain_32.png", Surface.SurfaceType.PNG);
			tilePlainImage = new Image(Renderer, tilePlainSurface, Image.ImageFormat.PNG);

			map = new TiledMap("L1A1_Large.tmx", Renderer);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			//MouseState mouseState = Mouse.GetState();
			//xP = mouseState.X;
			//yP = mouseState.Y;

			uiText.UpdateText(String.Format("X: {0}, Y: {1}", xP, yP));

			if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowUp))
				yP -= 10;
			if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowDown))
				yP += 10;
			if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowLeft))
				xP -= 10;
			if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowRight))
				xP += 10;
		}

		protected override void Draw(GameTime gameTime)
		{
			Renderer.ClearScreen();

			foreach (TileLayer tileLayer in map.TileLayers)
				foreach (Tile tile in tileLayer.Tiles)
					if (!tile.IsEmpty)
						Renderer.RenderTexture(tile.Texture, (int)tile.Position.X, (int)tile.Position.Y, tile.SourceTextureBounds);

			Renderer.RenderTexture(uiText.Texture, 0, 0);

			Renderer.RenderPresent();

			base.Draw(gameTime);
		}

		protected override void UnloadContent()
		{
			map.Dispose();
			uiText.Dispose();
			tilePlainImage.Dispose();
		}
	}
}
