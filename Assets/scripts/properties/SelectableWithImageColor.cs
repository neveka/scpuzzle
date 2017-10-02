using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCPuzzle
{
	public class SelectableWithImageColor : Property 
	{
		private Image _image;

		public SelectableWithImageColor(IGridObject gridObject):base(gridObject)
		{
		}

		public override void  Start () 
		{
			_image = _gridObject.GetProperty<Image> ();
		}

		public void Select()
		{
			_image.color = Color.yellow;
		}

		public void Deselect()
		{
			_image.color = Color.white;
		}
	}
}