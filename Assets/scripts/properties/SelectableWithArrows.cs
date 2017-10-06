using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCPuzzle
{
	public class SelectableWithArrows : Property 
	{
		private Image _arrowUp;
		private Image _arrowDown;
		private Image _arrowLeft;
		private Image _arrowRight;

		public SelectableWithArrows(IGridObject gridObject):base(gridObject)
		{
		}

		// Use this for initialization
		public override void Start () 
		{
			Image []images = _gridObject.GameObject.GetComponentsInChildren<Image> ();
			_arrowUp = System.Array.Find (images, i => i.name == "ArrowUp");
			_arrowDown = System.Array.Find (images, i => i.name == "ArrowDown");
			_arrowLeft = System.Array.Find (images, i => i.name == "ArrowLeft");
			_arrowRight = System.Array.Find (images, i => i.name == "ArrowRight");
			Deselect ();
		}

		public void Select()
		{
			_arrowUp.gameObject.SetActive (CanPass(Vector3.up));
			_arrowDown.gameObject.SetActive (CanPass(Vector3.down));
			_arrowLeft.gameObject.SetActive (CanPass(Vector3.left));
			_arrowRight.gameObject.SetActive (CanPass(Vector3.right));
			_gridObject.GameObject.transform.SetAsLastSibling ();
		}

		bool CanPass(Vector3 dir)
		{
			return _gridObject.Grid.GetFromCell<EnemyObject> (GridObject.GridPos + dir) != null ||
			_gridObject.Grid.GetFromCell<AllyObject> (GridObject.GridPos + dir) != null;
		}

		public void Deselect()
		{
			_arrowUp.gameObject.SetActive (false);
			_arrowDown.gameObject.SetActive (false);
			_arrowLeft.gameObject.SetActive (false);
			_arrowRight.gameObject.SetActive (false);
		}
	}
}