using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using OniFactory.Game.Pool;

namespace OniFactory.Game.UI
{
	public class GameResetPanel : UIBase
	{
		public Text NotificationField;

		private string _origString;

		private void Awake()
		{
			_origString = NotificationField.text;
		}

		public void UpdateNotificationText(Result result)
		{
			string _temp = NotificationField.text;

			if (result.IsTie) _temp = "It's a Tie!";
			else _temp = _temp.Replace("<value>", result.WinnerName);

			NotificationField.text = _temp;
		}

		public void Reset()
		{
			NotificationField.text = _origString;
			Hide();
		}
	}	
}