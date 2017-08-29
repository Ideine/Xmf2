using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Xmf2.Commons.Rx.ViewModels
{
	public class StateAutomata
	{
		public class Node
		{
			public string Id { get; }

			internal List<Transition> Edges { get; }

			public Node(string id)
			{
				Id = id;
				Edges = new List<Transition>();
			}

			public void AddTransition(Func<Task> callback, Node destination)
			{
				Edges.Add(new Transition(this, callback, destination));
			}
		}

		internal class Transition
		{
			private readonly Func<Task> _callback;

			public Node Origin { get; }

			public Node Destination { get; }

			public Transition(Node origin, Func<Task> callback, Node destination)
			{
				Origin = origin;
				_callback = callback;
				Destination = destination;
			}

			public Task Apply()
			{
				return _callback();
			}
		}

		private readonly List<Node> _nodes;
		private Node _currentNode;

		public StateAutomata(Node initialNode, List<Node> nodes)
		{
			_currentNode = initialNode;
			_nodes = nodes;
		}

		public async Task<bool> ToState(string stateName)
		{
			List<Transition> path = PathToState(stateName);

			if (path == null)
			{
				Debug.WriteLine($"Can not go to state {stateName} from {_currentNode.Id}");
				return false;
			}

			Debug.WriteLine($"ToState({stateName}) from {_currentNode.Id} with path of length {path.Count}");
			foreach (Transition transition in path)
			{
				try
				{
					Debug.WriteLine($"[Graph] Transition from {transition.Origin.Id} to {transition.Destination.Id}");
					await transition.Apply();
					_currentNode = transition.Destination;
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"[Graph] Exception {ex}");
					throw; //Rethrow or catch and send to a logger ?
				}
			}

			return true;
		}

		private List<Transition> PathToState(string stateName)
		{
			if (_currentNode.Id == stateName) //do not need to find a path 
			{
				return new List<Transition>();
			}

			Dictionary<Node, bool> marked = _nodes.ToDictionary(x => x, x => false);
			Dictionary<Node, Transition> parent = _nodes.ToDictionary(x => x, x => (Transition)null);
			marked[_currentNode] = true;

			Queue<Node> exploreNodes = new Queue<Node>();
			exploreNodes.Enqueue(_currentNode);

			while (exploreNodes.Count > 0)
			{
				Node node = exploreNodes.Dequeue();

				foreach (Transition transition in node.Edges)
				{
					if (marked[transition.Destination]) // this node has already been visited, stop there
					{
						continue;
					}

					marked[transition.Destination] = true;
					parent[transition.Destination] = transition;

					if (transition.Destination.Id == stateName)
					{
						List<Transition> result = new List<Transition>();

						Node resultNode = transition.Destination;
						for (Transition t = parent[resultNode]; t != null; resultNode = t.Origin, t = parent[resultNode])
						{
							result.Insert(0, t);
						}

						return result;
					}

					exploreNodes.Enqueue(transition.Destination);
				}
			}

			return null;
		}
	}
}
