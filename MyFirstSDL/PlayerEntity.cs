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
	public class PlayerEntity : DrawableEntity, ICollidable
	{
		private bool IsMovingLeft { get; set; }
		private bool IsMovingRight { get; set; }
		private bool IsMovingDown { get; set; }
		private bool IsMovingUp { get; set; }
		private Rectangle PreviousCollisionBox { get; set; }

		public PlayerEntity(Sprite sprite, Vector Position, Vector speed)
			: base(sprite, Position, speed)
		{
			Activate();
		}

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
			}
		}

		public override void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed)
		{
			if (Status == EntityStatus.Active)
			{
				IsMovingLeft = false;
				IsMovingRight = false;
				IsMovingDown = false;
				IsMovingUp = false;

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.W))
				{
					IsMovingUp = true;
					Position = new Vector(Position.X, Position.Y - Speed.Y);
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.A))
				{
					IsMovingLeft = true;
					Position = new Vector(Position.X - Speed.X, Position.Y);
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.S))
				{
					IsMovingDown = true;
					Position = new Vector(Position.X, Position.Y + Speed.Y);
				}

				if (keysPressed.Contains(KeyInformation.VirtualKeyCode.D))
				{
					IsMovingRight = true;
					Position = new Vector(Position.X + Speed.X, Position.Y);
				}
			}
		}

		public void SaveCollisionBox()
		{
			PreviousCollisionBox = CollisionBox;
		}

		public void ResolveCollision(ICollidable collidable)
		{
			if (collidable is MapObject)
			{
				MapObject collidableMapObject = collidable as MapObject;
				if (collidableMapObject.IsCollidable)
				{
					Vector collisionDepth = CollisionBox.GetIntersectionDepth(collidableMapObject.CollisionBox);

					// no intersection, so no collision!
					if (collisionDepth != Vector.Zero)
					{
						float absDepthX = Math.Abs(collisionDepth.X);
						float absDepthY = Math.Abs(collisionDepth.Y);

						// this offset will push the entity 1px beyond the depth correction
						float offsetY = collisionDepth.Y < 0 ? -1f : 1f;
						float offsetX = collisionDepth.X < 0 ? -1f : 1f;

						// this vector resolves collision in the Y direction
						// entity keeps same X coordinate but moves from current Y to Y + the intersection depth correction factor + the offset
						Vector resolutionPositionY = new Vector(Position.X, Position.Y + collisionDepth.Y + offsetY);

						// this vector resolves collision in the X direction
						// entity keeps same Y coordinate but moves from current X to X + the intersection depth correction factor + the offset
						Vector resolutionPositionX = new Vector(Position.X + collisionDepth.X + offsetX, Position.Y);

						// collision is less severe in the Y direction, so correct in favor of Y
						if (absDepthY < absDepthX)
							Position = resolutionPositionY;
						// collision is less sever in the X direction, so correct in favor of X
						else if (absDepthX < absDepthY)
							Position = resolutionPositionX;
						// collision is equally severe in both the X and the Y directions, so we need to determine which direction the player is moving and
						// on which side of the boxes the collision is occurring
						else
						{
							if (IsMovingDown && IsMovingLeft)
							{
								// our bottom passed the top of a tile, we hit the floor, correct us above the floor
								if (PreviousCollisionBox.Bottom <= collidableMapObject.CollisionBox.Top)
									Position = resolutionPositionY;
								// our left passed the right of a tile, we hit the left wall, correct us to the right of the wall
								else if (PreviousCollisionBox.Left <= collidableMapObject.CollisionBox.Right)
									Position = resolutionPositionX;
							}
							else if (IsMovingUp && IsMovingLeft)
							{
								// our top passed the bottom of a tile, we hit the ceiling, correct us below the ceiling
								if (PreviousCollisionBox.Top <= collidableMapObject.CollisionBox.Bottom)
									Position = resolutionPositionY;
								// our left passed the right of a tile, we hit the left wall, correct us to the right of the wall
								else if (PreviousCollisionBox.Left <= collidableMapObject.CollisionBox.Right)
									Position = resolutionPositionX;
							}
							else if (IsMovingUp && IsMovingRight)
							{
								// our bottom passed the top of a tile, we hit the floor, correct us above the floor
								if (PreviousCollisionBox.Top <= collidableMapObject.CollisionBox.Bottom)
									Position = resolutionPositionY;
								// our right passed the left of a tile, we hit the right wall, correct us to the left of the wall
								else if (PreviousCollisionBox.Right >= collidableMapObject.CollisionBox.Left)
									Position = resolutionPositionX;
							}
							else if (IsMovingDown && IsMovingRight)
							{
								// our top passed the bottom of a tile, we hit the ceiling, correct us below the ceiling
								if (PreviousCollisionBox.Bottom <= collidableMapObject.CollisionBox.Top)
									Position = resolutionPositionY;
								// our right passed the left of a tile, we hit the right wall, correct us to the left of the wall
								else if (PreviousCollisionBox.Right >= collidableMapObject.CollisionBox.Left)
									Position = resolutionPositionX;
							}
						}
					}
				}
			}
		}
	}
}