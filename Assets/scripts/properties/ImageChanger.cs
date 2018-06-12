using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCPuzzle
{
	public class ImageChanger : Property 
	{
		private Image _icon;
		private Sprite _sprite;

		public ImageChanger(IGridObject gridObject, Sprite sprite):base(gridObject)
		{
			_sprite = sprite;
		}

		public override void  Start ()
		{
			Image[] images = _gridObject.GameObject.GetComponentsInChildren<Image> ();
			_icon = System.Array.Find (images, i => i.name == "Icon");
			_icon.sprite = _sprite;
		}
	}
}
