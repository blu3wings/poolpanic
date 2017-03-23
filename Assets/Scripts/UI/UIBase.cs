using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OniFactory.Game.UI
{
	public class UIBase : MonoBehaviour
	{
		public virtual void Show() 
		{
			gameObject.SetActive(true);
		}

		public virtual void Hide() 
		{
			gameObject.SetActive(false);
		}

		public virtual void DisplayToggle(bool IsActive)
		{
			if (IsActive) Show();
			else Hide();
		}
	}	
}