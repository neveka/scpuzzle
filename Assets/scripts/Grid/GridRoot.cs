using UnityEngine;
using System.Collections;
using System;

namespace SCPuzzle
{
	/*public class GridRoot : MonoBehaviour 
	{
		protected IGrid _grid;
		// Use this for initialization
		protected virtual void Awake () {
			//SnapAllGridChildren();
		}
		
		public void SnapAllGridChildren(Component parent)
		{
			IGridObject[] objects = parent.transform.GetComponentsInChildren<IGridObject> ();
			Array.ForEach (objects, c => { 
				if(c != null)
				{
					c.Grid = Grid;
					c.SnapToGrid ();
				}
			});
		}
		
		public void RemoveAllGridChildren(Component parent)
		{
			IGridObject[] objects = parent.transform.GetComponentsInChildren<IGridObject> ();
			Array.ForEach (objects, c => { 
				if(c != null)
				{
					Grid.RemoveObject (c.GridPos, c);
				}
			});
		}

		public IGrid Grid
		{
			get{
				if(_grid == null)
				{
					_grid = new Grid();
				}
				return _grid;
			}
		}
	}*/
}