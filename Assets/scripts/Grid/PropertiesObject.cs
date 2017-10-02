using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	/*public enum GridObjectProperty
	{
		Passable,
		PassableObstacle,
		PassableWorkPlace,
		PassableTeleportPlace,
		CrewMember
	}*/

	public class PropertiesObject 
	{
		//public List<GridObjectProperty> properties = new List<GridObjectProperty>();
		protected Dictionary<Type, Property> _createdProperties = new Dictionary<Type, Property>();

		protected virtual void Awake()
		{
			/*foreach(GridObjectProperty prop in properties)
			{
				if(prop.ToString().Contains("Passable"))
				{
					PassableObject cell = new PassableObject();
					cell.GridObject = this as IGridObject;//?
					if(prop.ToString().Contains("WorkPlace"))
						cell.isWorkPlace = true;
					if(prop.ToString().Contains("TeleportPlace"))
						cell.isTeleportPlace = true;
					if(prop.ToString().Contains("Obstacle"))
						cell.isObstacle = true;
					AddProperty(cell);
				}
				if(prop.ToString().Contains("CrewMember"))
				{
					CrewMember crewMember = new CrewMember();
					crewMember.GridObject = this as IGridObject;//?
					AddProperty(crewMember);
				}
			}*/
		}

		public void AddProperty(Property prop)
		{
			_createdProperties.Add(prop.GetType(), prop);
			prop.Start();
		}

		public virtual T GetProperty<T> ()
		{
			Property res;
			_createdProperties.TryGetValue(typeof(T), out res);
			return (T)((object)res);
		}

		/*#if UNITY_EDITOR
		void OnDrawGizmos() 
		{
			foreach(KeyValuePair<Type, Property> pair in _createdProperties)
			{
				pair.Value.OnDrawGizmos(transform.position);
			}
		}
		#endif*/
	}
}
