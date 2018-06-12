using System.Collections;
using System.Collections.Generic;
using System;
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
		private int _maxInitiative;
		private Action<Vector3> _damagePlayerHealth;

		public EnemyObject(IGridObject gridObject, int maxInitiative, Action<Vector3> damagePlayerHealth):base(gridObject)
		{
			_maxInitiative = maxInitiative;
			_damagePlayerHealth = damagePlayerHealth;
		}

		public int Initiative
		{
			get{ return _initiative;}
			set{ 
				if (value < 0) 
				{
					_initiative = _maxInitiative;
					_damagePlayerHealth(GridObject.GridPos);
				}
				else
				{
					_initiative = value; 
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
			Initiative = _maxInitiative;
		}
	}
}