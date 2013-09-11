using SharpDL;
using SharpDL.Graphics;
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

	public abstract class Entity : IUpdatable
	{
		public Guid ID { get; private set; }
		public Vector Position { get; private set; }
		public EntityStatus Status { get; private set; }

		public event EventHandler<EventArgs> InactivatedEvent;
		public event EventHandler<EventArgs> ActivatedEvent;
		public event EventHandler<EntityDeathEventArgs> DeathEvent;

		public Entity()
		{
			ID = Guid.NewGuid();
			Status = EntityStatus.Inactive;
		}

		public void TeleportTo(Vector position)
		{
			Position = position;
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
			Utilities.RaiseEvent<EventArgs>(InactivatedEvent, this, e);
		}

		private void OnActivatedEvent(EventArgs e)
		{
			Utilities.RaiseEvent<EventArgs>(ActivatedEvent, this, e);
		}

		private void OnDeathEvent(EntityDeathEventArgs e)
		{
			Utilities.RaiseEvent<EntityDeathEventArgs>(DeathEvent, this, e);
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
