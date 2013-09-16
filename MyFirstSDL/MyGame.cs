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
			//if (e.MouseButton == MouseButtonEventArgs.MouseButtonCode.Left)
			//{
			//	xP = e.RelativeToWindowX;
			//	yP = e.RelativeToWindowY;
			//}
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

			CreateWindow("Lesson 2", 100, 100, SCREEN_WIDTH, SCREEN_HEIGHT, WindowFlags.Shown);
			CreateRenderer(RendererFlags.RendererAccelerated);
		}

		private TrueTypeText uiText;
		private Font coordinateFont;
		private SharpDL.Graphics.Color color = new SharpDL.Graphics.Color(255, 165, 0);

		//private Image tilePlainImage;
		private TiledMap map;
		private Image redImage;
		private Image blueImage;
		private Image greenImage;

		private Sprite redSprite;
		private Sprite blueSprite;
		private Sprite greenSprite;

		private DrawableEntity redEntity;
		private DrawableEntity blueEntity;
		private DrawableEntity greenEntity;

		private PlayerEntity playerEntity;

		private List<TrueTypeText> coordinateTexts = new List<TrueTypeText>();

		protected override void LoadContent()
		{
			base.LoadContent();

			string text = String.Format("X: {0}, Y: {1}", xP, yP);
			Font font = new Font("Fonts/Arcade N.ttf", 24);
			Surface fontSurface = new Surface(font, text, color);
			uiText = new TrueTypeText(Renderer, fontSurface, text, font, color);
			
			coordinateFont = new Font("Fonts/Arcade N.ttf", 6);


			//textTexture.LockTexture(fontSurface);

			//Surface tilePlainSurface = new Surface("Images/Tile_Plain_32.png", Surface.SurfaceType.PNG);
			//tilePlainImage = new Image(Renderer, tilePlainSurface, Image.ImageFormat.PNG);

			map = new TiledMap("Maps/test_walls.tmx", Renderer, Directory.GetCurrentDirectory());
			
			foreach (MapObjectLayer mapObjectLayer in map.MapObjectLayers)
			{
				foreach (MapObject mapObject in mapObjectLayer.MapObjects)
				{
					string coordinateTextString = String.Format("({0},{1})", mapObject.Bounds.Center.X, mapObject.Bounds.Center.Y);
					Surface coordinateFontSurface = new Surface(coordinateFont, coordinateTextString, color);
					TrueTypeText coordinateText = new TrueTypeText(Renderer, coordinateFontSurface, coordinateTextString, coordinateFont, color);
					coordinateTexts.Add(coordinateText);
				}
			}

			Surface redSurface = new Surface("Images/redEntity.png", Surface.SurfaceType.PNG);
			//Surface blueSurface = new Surface("Images/blueEntity.png", Surface.SurfaceType.PNG);
			//Surface greenSurface = new Surface("Images/greenEntity.png", Surface.SurfaceType.PNG);

			redImage = new Image(Renderer, redSurface, Image.ImageFormat.PNG);
			//blueImage = new Image(Renderer, blueSurface, Image.ImageFormat.PNG);
			//greenImage = new Image(Renderer, greenSurface, Image.ImageFormat.PNG);

			//redSprite = new Sprite(redImage.Texture);
			//blueSprite = new Sprite(blueImage.Texture);
			//greenSprite = new Sprite(greenImage.Texture);

			Surface playerSurface = new Surface("Images/Iso_Cubes_01_32x32_Alt_00_001.png", Surface.SurfaceType.PNG);
			Image playerImage = new Image(Renderer, playerSurface, Image.ImageFormat.PNG);
			Sprite playerSprite = new Sprite(playerImage);
			playerEntity = new PlayerEntity(playerSprite, Vector.Zero, new Vector(6, 3));
		}

		private List<DrawableEntity> entities = new List<DrawableEntity>();

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			uiText.UpdateText(String.Format("X: {0}, Y: {1}", xP, yP));

			playerEntity.Move(gameTime, keysPressed);

			//foreach (var entity in entities)
			//	entity.Move(gameTime);

			//if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowUp))
			//	yP -= 10;
			//if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowDown))
			//	yP += 10;
			//if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowLeft))
			//	xP -= 10;
			//if (keysPressed.Contains(KeyInformation.VirtualKeyCode.ArrowRight))
			//	xP += 10;
		}

		protected override void Draw(GameTime gameTime)
		{
			Renderer.ClearScreen();

			foreach (TileLayer tileLayer in map.TileLayers)
				foreach (Tile tile in tileLayer.Tiles)
					if (!tile.IsEmpty)
						Renderer.RenderTexture(tile.Texture, tile.Position.X, tile.Position.Y, tile.SourceTextureBounds);

			foreach (MapObjectLayer mapObjectLayer in map.MapObjectLayers)
			{
				int i = 0;
				foreach (MapObject mapObject in mapObjectLayer.MapObjects)
				{
					Renderer.RenderTexture(redImage.Texture, mapObject.Bounds.Center.X - redImage.Texture.Width / 2, mapObject.Bounds.Center.Y - redImage.Texture.Height / 2);

					TrueTypeText coordinateText = coordinateTexts[i];
					Renderer.RenderTexture(coordinateText.Texture, mapObject.Bounds.Center.X, mapObject.Bounds.Center.Y);

					i++;
				}
			}

			Renderer.RenderTexture(uiText.Texture, 0, 0);

			playerEntity.Draw(Renderer, gameTime);

			//foreach (var entity in entities)
			//	entity.Draw(Renderer, gameTime);

			Renderer.RenderPresent();

			base.Draw(gameTime);
		}

		protected override void UnloadContent()
		{
			if (map != null)
				map.Dispose();
			if (uiText != null)
				uiText.Dispose();
			if (playerEntity != null)
				playerEntity.Dispose();
			//if (tilePlainImage != null)
			//	tilePlainImage.Dispose();
		}
	}
}
