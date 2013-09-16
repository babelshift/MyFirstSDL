using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using SharpDL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{

	public class PhysicsManager
	{
		private List<Body> bodies = new List<Body>();
		private List<Fixture> fixtures = new List<Fixture>();

		public World World { get; private set; }
		public float StepTime { get; private set; }
		public IEnumerable<Body> Bodies { get { return bodies; } }
		public IEnumerable<Fixture> Fixtures { get { return fixtures; } }

		public PhysicsManager(float stepTime, Vector gravity)
		{
			World = new World(new Microsoft.Xna.Framework.Vector2(gravity.X, gravity.Y));
			StepTime = stepTime;
		}

		public void Update()
		{
			World.Step(StepTime);
		}

		private Microsoft.Xna.Framework.Vector2 GetVectorInSimUnits(int displayX, int displayY)
		{
			float simX = ConvertUnits.ToSimUnits(displayX);
			float simY = ConvertUnits.ToSimUnits(displayY);
			return new Microsoft.Xna.Framework.Vector2(simX, simY);
		}

		public Body CreateCircle(float radius, float density, BodyType bodyType)
		{
			Body body = BodyFactory.CreateCircle(World, radius, density);
			body.BodyType = bodyType;
			bodies.Add(body);
			return body;
		}

		public Body CreateEdge(int startX, int startY, int endX, int endY, BodyType bodyType)
		{
			Body body = BodyFactory.CreateEdge(World, GetVectorInSimUnits(startX, startY), GetVectorInSimUnits(endX, endY));
			body.BodyType = bodyType;
			bodies.Add(body);
			return body;
		}

		public Fixture AttachEdge(Body body, int startX, int startY, int endX, int endY)
		{
			Fixture fixture =FixtureFactory.AttachEdge(GetVectorInSimUnits(startX, startY), GetVectorInSimUnits(endX, endY), body);
			fixtures.Add(fixture);
			return fixture;
		}

		public Fixture AttachCircle(Body body, float radius, float density, float restitution = 0f)
		{
			Fixture fixture = FixtureFactory.AttachCircle(radius, density, body);
			fixture.Restitution = restitution;
			fixtures.Add(fixture);
			return fixture;
		}
	}
}
