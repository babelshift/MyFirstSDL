using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSDL
{
	public static class Utilities
	{
		public static void RaiseEvent<T>(EventHandler<T> eventHandler, object sender, T eventArguments)
			where T : EventArgs
		{
			if (eventHandler != null)
				eventHandler(sender, eventArguments);
		}
	}
}
