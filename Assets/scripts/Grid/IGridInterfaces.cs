using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public interface IGridObject
	{
		IGrid Grid { get; }
		GameObject GameObject { get; }
		Vector3 WorldPos{ get; set; }
		//Vector3 Dir{get; set;}
		Vector3 GridPos{ get; set; }
		void Destroy ();

		void SnapToGrid();
		void SnapToAxis();
		//string GetName();
		T GetProperty<T> ();
	}

	public interface IGrid
	{
		GridUtils Utils { get; }
		List<T> GetAll<T>();
		void SetToCell(Vector3 pos, IGridObject value);
		IGridObject GetFromCell(Vector3 pos);
		//IGridObject GetTopCell(Vector3 pos);
		T FindFromCell<T>(Vector3 pos);
		T GetFromCell<T>(Vector3 pos);
		//T GetTopCell<T>(Vector3 pos);
		List<IGridObject> GetAllFromCell(Vector3 pos);
		void RemoveObject(Vector3 oldPos, IGridObject obj);
		void MoveObjectFromPosToPos (IGridObject obj, Vector3 oldPos, Vector3 newPos, bool force);
		void Clear();
	}
}
