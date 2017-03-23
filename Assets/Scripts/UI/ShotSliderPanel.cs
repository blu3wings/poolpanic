using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OniFactory.Game.UI
{
	public class ShotSliderPanel : UIBase
	{
		public Slider PowerSlider;

		private UnityAction<float> _onShoot;

		public void Shoot()
		{
			if (System.Math.Abs(PowerSlider.value) > 0)
			{
				if (_onShoot != null)
				{
					_onShoot(PowerSlider.value);
				}
				PowerSlider.value = 0;
			}
		}

		public void Initialise(
			UnityAction<float> OnShoot)
		{
			_onShoot = OnShoot;
		}

	}	
}