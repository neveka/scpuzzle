using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SCPuzzle
{
	public class EnemyObject : Property 
	{
		private Image _healthBar;
		private Text _initiativeText;
		private float _health;
		private int _initiative;

		public EnemyObject(IGridObject gridObject):base(gridObject)
		{
			
		}

		public int Initiative
		{
			get{ return _initiative;}
			set{ 
				if (value < 0) 
				{
					_initiative = Random.Range (5, 20);
				}
				else
				{
					_initiative = value; 
					if (_initiative == 0)
					{
						GameObject.Find ("PlayerHealthBar").GetComponent<Image> ().fillAmount -= 0.1f;
					}
				}
					
				_initiativeText.text = _initiative.ToString (); 
			}
		}

		public float Health
		{
			get{ return _health;}
			set{ 
				_health = value; 
				_healthBar.fillAmount = _health; 
				if (_health <= 0) 
				{
					_gridObject.Destroy();

				}
			}
		}

		public override void  Start () 
		{
			Image []images = _gridObject.GameObject.GetComponentsInChildren<Image> ();
			_healthBar = System.Array.Find (images, i => i.name == "HealthBar");
			_initiativeText =  _gridObject.GameObject.GetComponentInChildren<Text> ();
			Health = 1;
			Initiative = Random.Range (5, 20);
		}
	}
}