using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OniFactory.Player;

namespace OniFactory.Game.UI
{
	public class PlayerPanel : UIBase
	{
		public Text[] PlayersLabel;
		public Text[] PlayersScore;
		public Image[] Indicator;

		public void Initialise(PlayerBase[] Players)
		{
			for (int i = 0; i < PlayersLabel.Length; i++)
			{
				//Map the UI elements to player object
				Players[i].Initialise(i,UpdatePlayerScore,SwitchToPlayer);
				PlayersLabel[i].text = Players[i].Name;
				PlayersScore[i].text = "0";
				Indicator[i].gameObject.SetActive(i == 0 ? true : false);
			}
		}

		public void UpdatePlayerScore(int Id, int Score)
		{
			PlayersScore[Id].text = Score.ToString();
		}

		public void SwitchToPlayer(int Id)
		{
			for (int i = 0; i < Indicator.Length; i++)
			{
				Indicator[i].gameObject.SetActive(i == Id ? true : false);
			}
		}
	}	
}