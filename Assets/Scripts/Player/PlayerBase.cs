using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OniFactory.Player
{
	public class PlayerBase : MonoBehaviour
	{
		public string Name;

		private int _score = 0;
		private int _id;
		private UnityAction<int,int> _onScoreUpdated;
		private UnityAction<int> _onSwitchToPlayer;

		public int Score
		{
			get { return _score;}
		}

		public virtual void Initialise(int id,
		                               UnityAction<int,int> OnScoreUpdated,
		                               UnityAction<int> OnSwitchToPlayer)
		{
			_onScoreUpdated = OnScoreUpdated;
			_onSwitchToPlayer = OnSwitchToPlayer;
			_id = id;
		}

		public virtual void UpdateScore(int score)
		{
			_score = _score + score;
			if (_onScoreUpdated != null) _onScoreUpdated(_id,_score);
		}

		public virtual void SwitchToPlayer()
		{
			_onSwitchToPlayer(_id);
		}

		public virtual void Reset()
		{
			_score = 0;
			UpdateScore(_score);
		}
	}	
}