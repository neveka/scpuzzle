using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public class Property
	{
		public Property(IGridObject gridObject)
		{
			_gridObject = gridObject;
		}

		protected IGridObject _gridObject;
		public IGridObject GridObject
		{
			get{ return _gridObject; }
		}

		public virtual void OnDrawGizmos(Vector3 pos)
		{
		}

		public virtual void Start()
		{
		}
	}
}
