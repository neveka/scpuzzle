using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCPuzzle
{
	public class InputManager : MonoBehaviour 
	{
		private IGrid _grid;
		private Vector3 _downMousePos;
		public void Init (IGrid grid) 
		{
			_grid = grid;
		}

		void Update ()
		{
			if (Input.GetMouseButtonDown (0)) 
			{
				_downMousePos = Input.mousePosition;
			}
			if (Input.GetMouseButton (0)) 
			{
				Vector3 dir = (Input.mousePosition - _downMousePos)*2;
				Vector3 downGridPos = _grid.Utils.MousePosToGridPos (_downMousePos);
				Vector3 upGridPos = _grid.Utils.MousePosToGridPos (_downMousePos+dir);
				MovingObject m1 = _grid.GetFromCell<MovingObject> (downGridPos);
				MovingObject m2 = _grid.GetFromCell<MovingObject> (upGridPos);
				SelectableWithArrows s = _grid.GetFromCell<SelectableWithArrows> (downGridPos);
				if ((downGridPos - upGridPos).sqrMagnitude == 1 &&
				   m1 != null && !m1.IsMoving () && m1.GridObject.GetProperty<AllyObject> () != null && m2 != null && !m2.IsMoving () &&
				   (m2.GridObject.GetProperty<AllyObject> () != null || m2.GridObject.GetProperty<EnemyObject> () != null)) {
					if (s != null)
						s.Deselect ();
					m1.GridObject.GetProperty<AllyObject> ().OnMoveStarted ();
					m1.StartMovingTo (m2.GridObject.GridPos, ()=>m1.GridObject.GetProperty<AllyObject> ().OnMoveFinished(m2.GridObject));
					m2.StartMovingTo (m1.GridObject.GridPos, null);
				} 
				else if (downGridPos == upGridPos && s != null && !m1.IsMoving ()) 
				{
					List<SelectableWithArrows> selectableObjects = _grid.GetAll<SelectableWithArrows> ();
					selectableObjects.ForEach(o=>o.Deselect());
					s.Select ();
				}
			}
			List<MovingObject> movingObjects = _grid.GetAll<MovingObject> ();
			movingObjects.ForEach (m => m.Update ());//temp
		}
	}
}