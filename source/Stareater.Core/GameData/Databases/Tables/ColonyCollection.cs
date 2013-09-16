﻿using System;
using System.Collections.Generic;
using Stareater.Utils.Collections;
using Stareater.Galaxy;
using Stareater.Players;

namespace Stareater.GameData.Databases.Tables
{
	class ColonyCollection : ICollection<Colony>, IDelayedRemoval<Colony>
	{
		HashSet<Colony> innerSet = new HashSet<Colony>();
		List<Colony> toRemove = new List<Colony>();

		Dictionary<Player, List<Colony>> PlayersIndex = new Dictionary<Player, List<Colony>>();
		Dictionary<Planet, List<Colony>> PlanetsIndex = new Dictionary<Planet, List<Colony>>();

		public IEnumerable<Colony> Players(Player key) {
			if (PlayersIndex.ContainsKey(key))
				foreach (var item in PlayersIndex[key])
					yield return item;
		}

		public IEnumerable<Colony> Planets(Planet key) {
			if (PlanetsIndex.ContainsKey(key))
				foreach (var item in PlanetsIndex[key])
					yield return item;
		}
	
		public void Add(Colony item)
		{
			innerSet.Add(item); 

			if (!PlayersIndex.ContainsKey(item.Owner))
				PlayersIndex.Add(item.Owner, new List<Colony>());
			PlayersIndex[item.Owner].Add(item);

			if (!PlanetsIndex.ContainsKey(item.Location))
				PlanetsIndex.Add(item.Location, new List<Colony>());
			PlanetsIndex[item.Location].Add(item);
		}

		public void Add(IEnumerable<Colony> items)
		{
			foreach(var item in items)
				Add(item);
		}
		
		public void Clear()
		{
			innerSet.Clear();

			PlayersIndex.Clear();
			PlanetsIndex.Clear();
		}

		public bool Contains(Colony item)
		{
			return innerSet.Contains(item);
		}

		public void CopyTo(Colony[] array, int arrayIndex)
		{
			innerSet.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return innerSet.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Colony item)
		{
			if (innerSet.Remove(item)) {
				PlayersIndex[item.Owner].Remove(item);
				PlanetsIndex[item.Location].Remove(item);
			
				return true;
			}

			return false;
		}

		public IEnumerator<Colony> GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return innerSet.GetEnumerator();
		}

		public void PendRemove(Colony element)
		{
			toRemove.Add(element);
		}

		public void ApplyRemove()
		{
			foreach (var element in toRemove)
				this.Remove(element);
			toRemove.Clear();
		}
	}
}
