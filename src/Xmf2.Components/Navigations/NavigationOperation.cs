using System;
using System.Collections.Generic;

namespace Xmf2.Components.Navigations
{
	public class NavigationOperation
	{
		private readonly List<Pop> _pops = new List<Pop>(4);
		private readonly List<Push> _pushes = new List<Push>(4);

		public IReadOnlyList<Pop> Pops => _pops;
		public IReadOnlyList<Push> Pushes => _pushes;
		
		internal void Add(Pop action)
		{
			_pops.Add(action);
		}
		
		internal void Add(Push action)
		{
			_pushes.Add(action);
		}

		public abstract class NavigationAction
		{
			public ScreenDefinition Screen { get; }
			
			public ScreenInstance Instance { get; }

			internal NavigationAction(ScreenInstance screen)
			{
				Screen = screen.Definition;
				Instance = screen;
			}
		}

		public class Pop : NavigationAction
		{
			internal Pop(ScreenInstance screen) : base(screen)
			{
#if DEBUG
				Console.WriteLine($"\t\tPop: {screen.Definition.RelativeRoute} (parameter: {screen.Parameter})");
#endif
			}
		}

		public class Push : NavigationAction
		{
			internal Push(ScreenInstance screen) : base(screen)
			{
#if DEBUG
				Console.WriteLine($"\t\tPush: {screen.Definition.RelativeRoute} (parameter: {screen.Parameter})");
#endif
			}
		}
	}
}