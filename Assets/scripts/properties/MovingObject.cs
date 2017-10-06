using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace SCPuzzle
{
	public class MovingObject : Property
	{
		public float speed = 200;
		protected MovingPath _path;
		protected Vector3 _oldDir;
		protected Action _onPathComplete;
		public Func<MovingObject, bool> onInteract;
		public Action onWait;

		protected bool _waits;
		private CoroutineStarter _coroutineStarter;

		//protected SpriteChanger _spriteChanger;

		public MovingObject(IGridObject gridObject, CoroutineStarter coroutineStarter):base(gridObject)
		{ 
			_coroutineStarter = coroutineStarter;
		}

		/*public bool StartMoveToNear(int minPathLength, int pathLength, out PassableObject selection, Func<PassableObject, bool> isCellOk, Action onPathComplete)
		{
			PassableObject underCell = _gridObject.Grid.GetFromCell<PassableObject>(_gridObject.GridPos+Vector3.down);
			PassableObject reachableCell = null;
			_gridObject.Grid.Utils.PathFinder.GetReachableCells(underCell, pathLength, false, (PassableObject cell) => {
				if(isCellOk(cell) && (cell.GridObject.GridPos - underCell.GridObject.GridPos).sqrMagnitude>minPathLength*minPathLength)
				{
					reachableCell = cell;
					return true;
				}
				return false;
			});
			selection = reachableCell;
			if(reachableCell == null)
			{
				return false;
			}
			return StartMovingTo(selection.GridObject.GridPos+Vector3.up, onPathComplete);
		}*/

		public bool StartMovingTo (Vector3 endPoint, Action onPathComplete)
		{
			_onPathComplete = onPathComplete;
			if(IsMoving()||endPoint == _gridObject.GridPos) 
			{
				return false;
			}
			_path = _gridObject.Grid.Utils.PathFinder.CreatePath (
				_gridObject.Grid.Utils.GridPosToWorldPos(_gridObject.GridPos), 
				_gridObject.Grid.Utils.GridPosToWorldPos(endPoint), speed);//FindPath (_gridObject, endPoint, speed);
			if(_path != null)
			{

				return true;
			}
			return false;
		}

		/*public void StartMovingFromTo(Vector3 fromPos, Vector3 toPos)
		{
			_path = new MovingPath (new Vector3[]{fromPos, toPos}, speed);
		}*/

		public bool IsMoving()
		{
			return _path != null;
		}

		public void OnStop()
		{
			if (!IsMoving())
				return;
			_gridObject.WorldPos = _path.GetTargetPosition();
			if(_onPathComplete != null)
			{
				_onPathComplete();
				_onPathComplete = null;
			}
			ForseStop();
		}

		public void ForseStop()
		{
			if(!IsMoving())
				return;
			//_spriteChanger.SetSpeed(0, Vector3.zero);
			_oldDir = Vector3.zero;
			_path = null;
			_waits = false;
		}

		public void Update()
		{
			_waits = false;
			if (_path != null) 
			{
				Vector3 delta;
				if(_path.UpdateMovement(Time.deltaTime, _gridObject.WorldPos, out delta))
				{
					/*if(delta!=Vector3.zero && _oldDir != delta.normalized)
					{
						_spriteChanger.SetSpeed(_path.speed, delta.normalized);
					}*/

					Vector3 newGridPos = _gridObject.Grid.Utils.WorldPosToGridPos(_gridObject.WorldPos + delta);
					if(_gridObject.GridPos != newGridPos)
					{
						MovingObject other = _gridObject.Grid.FindFromCell<MovingObject>(newGridPos);
						if(other!=null && other != this && other._gridObject.GridPos == newGridPos)
						{
							if(onInteract!=null && onInteract(other))
								return;
							if(!other._waits && other.IsMoving())
							{
								if(onWait!=null)
									onWait();
								_waits = true;
								return;
							}
						}
					}

					_gridObject.WorldPos += delta;
					_oldDir = delta.normalized;
				}
				else
				{
					Vector3 oldPos = _gridObject.WorldPos;
					OnStop();
					Vector3 offset = _gridObject.GridPos-oldPos;
					offset.y = 0;
				}
			}	
		}



		/*protected virtual void OnDrawGizmos() 
		{
			Gizmos.color = Color.red;
			if(_path!=null)
				Gizmos.DrawLine (_gridObject.Grid.Utils.GridPosToWorldPos(_path.GetTargetPosition())+Vector3.down*2, transform.position+Vector3.down);
		}*/
	}
}
