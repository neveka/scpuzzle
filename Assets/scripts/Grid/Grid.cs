using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public class PosComparer : IEqualityComparer<Vector3>
	{
		public bool Equals (Vector3 x, Vector3 y)
		{ 
			return x.x == y.x && x.y == y.y && x.z == y.z; 
		}

		public int GetHashCode(Vector3 x)
		{
			return x.x.GetHashCode() + x.y.GetHashCode() + x.z.GetHashCode();
		}
	}

	public class Grid : IGrid
	{
		Dictionary<Vector3, List<IGridObject>> _objects = new Dictionary<Vector3, List<IGridObject>>(new PosComparer());//?
		private GridUtils _utils = new GridUtils();

		public GridUtils Utils { get { return _utils; }}

		public void RemoveObject(Vector3 oldPos, IGridObject obj)
		{
			List<IGridObject> list;
			if (_objects.TryGetValue (oldPos, out list)) 
			{
				list.Remove(obj);
				if(list.Count == 0)
				{
					_objects.Remove(oldPos);
				}
			}
		}

		public void SetToCell(Vector3 pos, IGridObject value)
		{
			if (value == null) 
			{
				Debug.LogError("SetToCell "+pos+" null object");
			}
			List<IGridObject> list;
			if (_objects.TryGetValue (pos, out list)) 
			{
				list.Add(value);
			} 
			else
			{
				_objects.Add(pos, new List<IGridObject>(){value});
			}
		}

		/*public IGridObject GetTopCell(Vector3 pos)
		{
			pos.y = -1;
			IGridObject topCell = null;
			int k = 5;//top
			while(k-->0)
			{
				pos = pos+Vector3.up;
				if(GetFromCell(pos)!=null)
					topCell = GetFromCell(pos);
			}
			return topCell;
		}

		public T GetTopCell<T>(Vector3 pos)
		{
			pos.y = -1;
			T topCell = default(T);
			int k = 5;//top
			while(k-->0)
			{
				pos = pos+Vector3.up;
				if(GetFromCell<T>(pos)!=null)
					topCell = GetFromCell<T>(pos);
			}
			return topCell;
		}*/

		public IGridObject GetFromCell(Vector3 pos)
		{
			IGridObject go = null;
			List<IGridObject> list;
			if (_objects.TryGetValue (pos, out list) && list.Count>0) 
			{
				go = list[0];//.Find(o=>o as GridObject);
			}
			return go;
		}

		public T FindFromCell<T>(Vector3 pos)
		{
			IGridObject go = null;
			List<IGridObject> list;
			if (_objects.TryGetValue (pos, out list) && list.Count>0) 
			{
				go = list.Find(o=>/*o as GridObject &&*/ o.GetProperty<T>()!=null);
			}
			return go!=null?go.GetProperty<T>():default(T);
		}

		public T GetFromCell<T>(Vector3 pos)
		{
			IGridObject o = GetFromCell(pos);
			if(o == null)
				return default(T);
			return o.GetProperty<T>();
		}

		public List<IGridObject> GetAllFromCell(Vector3 pos)
		{
			List<IGridObject> list;
			if (_objects.TryGetValue (pos, out list)) 
			{
				//list.RemoveAll(o=>!(o as GridObject));
				return list;
			}
			return null;
		}

		public List<T> GetAll<T>()
		{
			List<T> list = new List<T> ();
			foreach(KeyValuePair<Vector3, List<IGridObject>> pair in _objects)
			{
				foreach(IGridObject obj in pair.Value)
				{
					//if(!(obj as GridObject))
					//	continue;
					T property = obj.GetProperty<T>();
					if(property != null)
						list.Add(property);
				}
			}
			return list;
		}


		public void Clear()
		{
			_objects.Clear ();
		}

		/*public void ClearEmpties()
		{
			List<Vector3> empties = new List<Vector3>();
			foreach(KeyValuePair<Vector3, List<IGridObject>> pair in _objects)
			{
				pair.Value.RemoveAll(o=> o as GridObject == null);
				if(pair.Value.Count == 0)
				{
					empties.Add(pair.Key);
				}
			}
			foreach(Vector3 empty in empties)
				_objects.Remove(empty);
		}*/

		public void MoveObjectFromPosToPos(IGridObject obj, Vector3 oldPos, Vector3 newPos, bool force)
		{
			if(oldPos != newPos||force)//optimize
			{
				RemoveObject(oldPos, obj);
				SetToCell(newPos, obj);
			}
		}

		/*public void DebugDraw() 
		{
			foreach(KeyValuePair<Vector3, List<IGridObject>> pair in _objects)
			{
				IGridObject obj = pair.Value.Count>0?pair.Value[0]:null;
				Gizmos.color = obj!=null?Color.yellow:Color.red;
				GridUtils.Instance.DebugDraw(pair.Key);
			}
		}*/
	}
}