using System;
using System.Collections.Generic;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.LinearList
{
	public class Add
	{
		public int NewPos { get; }

		public Add(int newPos)
		{
			NewPos = newPos;
		}
	}

	public class Remove
	{
		public int OldPos { get; }

		public Remove(int oldPos)
		{
			OldPos = oldPos;
		}
	}

	public class Move
	{
		public int OldPos { get; }
		public int NewPos { get; }

		public Move(int oldPos, int newPos)
		{
			NewPos = newPos;
			OldPos = oldPos;
		}
	}

	public static class DiffListUtil
	{
		public static (List<Remove>, List<Move>, List<Add>) DiffList(IEntityViewState[] old, IEntityViewState[] newList)
		{
			var removeList = new List<Remove>();
			var moveList = new List<Move>();
			var addList = new List<Add>();

			bool[] here = new bool[old.Length];

			for (int i = 0; i < newList.Length; i++)
			{
				if (i >= old.Length)
				{
					int pos = 0;
					if ((pos = Array.FindIndex(old, o => o.Id == newList[i].Id)) == -1)
					{
						addList.Add(new Add(i));
					}
					else
					{
						moveList.Add(new Move(pos, i));
						here[pos] = true;
					}
				}
				else
				{
					if (newList[i].Id != old[i].Id)
					{
						int pos = 0;
						if ((pos = Array.FindIndex(old, o => o.Id == newList[i].Id)) == -1)
						{
							addList.Add(new Add(i));
						}
						else
						{
							moveList.Add(new Move(pos, i));
							here[pos] = true;
						}
					}
					else
					{
						here[i] = true;
					}
				}
			}

			for (int i = 0; i < old.Length; i++)
			{
				if (!here[i])
				{
					removeList.Add(new Remove(i));
				}
			}

			return (removeList, moveList, addList);
		}
	}
}