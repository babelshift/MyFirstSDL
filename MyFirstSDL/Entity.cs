using SharpDL;
using SharpDL.Graphics;
using SharpDL.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public enum EntityStatus
	{
		Active,
		Inactive,
		Dead
	}

	public abstract class Entity
	{
		public Guid ID { get; private set; }
		public Vector Position { get; protected set; }
		public EntityStatus Status { get; private set; }

		protected Vector Speed { get; private set; }
		protected float RadiansOfRotation { get; private set; }

		private Vector Direction
		{
			get
			{
				float xCoord = (float)Math.Cos((double)RadiansOfRotation);
				float yCoord = (float)Math.Sin((double)RadiansOfRotation);

				return new Vector(xCoord, yCoord);
			}
		}

		public event EventHandler<EventArgs> Inactivated;
		public event EventHandler<EventArgs> Activated;
		public event EventHandler<EntityDeathEventArgs> Death;

		public Entity()
		{
			ID = Guid.NewGuid();
			Status = EntityStatus.Inactive;
		}

		public Entity(Vector position, Vector speed)
			: this()
		{
			Position = position;
			Speed = speed;
		}

		public void TeleportTo(Vector position)
		{
			Position = position;
		}

		public virtual void Move(GameTime gameTime, IEnumerable<KeyInformation.VirtualKeyCode> keysPressed)
		{
			if (Status == EntityStatus.Active)
			{
				//RadiansOfRotation = (float)Math.Atan2((double)(currentDestination.Y - Position.Y), (double)(currentDestination.X - Position.X));

				//Vector previousPosition = Position;
				//Vector direction = Direction;
				//double dt = gameTime.ElapsedGameTime.TotalSeconds;
				//Position += new Vector((float)(direction.X * Speed.X * dt), (float)(direction.Y * Speed.Y * dt));
			}
		}

		public void Activate()
		{
			ChangeStatus(EntityStatus.Active);
		}

		public void Deactivate()
		{
			ChangeStatus(EntityStatus.Inactive);
		}

		public void ChangeStatus(EntityStatus status)
		{
			// only fire events if the new status is different than the current status
			if (Status != status)
			{
				EntityStatus previousStatus = Status;
				EntityStatus newStatus = status;

				Status = status;

				if (status == EntityStatus.Dead)
				{
					EntityDeathEventArgs entityDeathEventArgs = new EntityDeathEventArgs(previousStatus, newStatus, Position);
					OnDeathEvent(entityDeathEventArgs);
				}
				else if (status == EntityStatus.Active)
					OnActivatedEvent(EventArgs.Empty);
				else if (status == EntityStatus.Inactive)
					OnInactivatedEvent(EventArgs.Empty);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		private void OnInactivatedEvent(EventArgs e)
		{
			Utilities.RaiseEvent<EventArgs>(Inactivated, this, e);
		}

		private void OnActivatedEvent(EventArgs e)
		{
			Utilities.RaiseEvent<EventArgs>(Activated, this, e);
		}

		private void OnDeathEvent(EntityDeathEventArgs e)
		{
			Utilities.RaiseEvent<EntityDeathEventArgs>(Death, this, e);
		}
	}

	public class EntityDeathEventArgs : EntityStatusChangedEventArgs
	{
		public Vector Position { get; private set; }

		public EntityDeathEventArgs(EntityStatus previousStatus, EntityStatus newStatus, Vector position)
			: base(previousStatus, newStatus)
		{
			Position = position;
		}
	}

	public abstract class EntityStatusChangedEventArgs : EventArgs
	{
		public EntityStatus PreviousStatus { get; private set; }
		public EntityStatus NewStatus { get; private set; }

		public EntityStatusChangedEventArgs(EntityStatus previousStatus, EntityStatus newStatus)
		{
			PreviousStatus = previousStatus;
			NewStatus = newStatus;
		}
	}
}
