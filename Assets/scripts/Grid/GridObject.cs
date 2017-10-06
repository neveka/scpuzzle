using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public class GridObject : PropertiesObject, IGridObject
	{
		private IGrid _grid;
		private GameObject _gameObject;
		private Action<Vector3> _onDestroy;
		public GridObject(GameObject gameObject, IGrid grid, Action<Vector3> onDestroy)
		{
			_gameObject = gameObject;
			_gameObject.transform.localPosition = Vector3.back*100500;
			_grid = grid;
			_onDestroy = onDestroy;
		}

		public IGrid Grid 
		{
			get{ return _grid; }
		}

		public GameObject GameObject 
		{
			get{ return _gameObject; }
		}

		/*public string GetName()
		{
			return _gameObject.name;
		}*/

		public Vector3 WorldPos 
		{
			get{ return _gameObject.transform.localPosition; }
			set{ 
				Vector3 oldViewPos = GridPos;
				_gameObject.transform.localPosition = value;
				if (_grid != null) 
				{
					_grid.MoveObjectFromPosToPos(this, oldViewPos, GridPos, false);
				}; 
			}
		}

		public Vector3 GridPos 
		{
			get{ return _grid.Utils.WorldPosToGridPos(WorldPos); }
			set{ WorldPos = _grid.Utils.GridPosToWorldPos(value); }
		}

		public void SnapToGrid()
		{
			GridPos = _grid.Utils.SnapToGrid(GridPos);
			if (_grid != null) 
			{
				_grid.MoveObjectFromPosToPos(this, GridPos, GridPos, true);
			}
		}

		public void SnapToAxis()
		{
			Vector3 euler = _gameObject.transform.localRotation.eulerAngles;
			_gameObject.transform.localRotation = 
				Quaternion.Euler(Mathf.RoundToInt(euler.x/90)*90, Mathf.RoundToInt(euler.y/90)*90, Mathf.RoundToInt(euler.z/90)*90);
		}

		public override T GetProperty<T> ()
		{
			if(typeof(T).IsSubclassOf(typeof(Component)))
				return _gameObject.GetComponent<T>();
			return base.GetProperty<T>();
		}

		public void Destroy()
		{
			if(_onDestroy != null)
				_onDestroy (GridPos);
			Grid.RemoveObject (GridPos, this);
			GameObject.Destroy(_gameObject);
		}


	}
}