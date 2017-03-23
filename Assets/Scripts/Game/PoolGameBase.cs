using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OniFactory.Ball;
using UnityEngine.Events;
using OniFactory.Game.UI;
using OniFactory.Player;

namespace OniFactory.Game.Pool
{
	public class PoolGameBase : MonoBehaviour
	{
		//This is the core of the game. You can inherit this class to expand the game rule
		//and behaviour

		public PoolBall[] PoolBalls;
		public PlayerBase[] Players;
		public float VelocityDecayRate;
		public float BounceTreshold;

		private Vector3 _shotDirection;
		private List<object> _activeBalls = new List<object>();

		protected PoolBall pCueBall;
		protected int pCurrentPlayerId = 0;

		#region Private methods

		private void Awake()
		{
			//Save orginal position so we can use later for the reset
			for (int i = 0; i < PoolBalls.Length; i++)
			{
				if (PoolBalls[i].IsCueBall) pCueBall = PoolBalls[i];
				PoolBalls[i].InitialiseBall(VelocityDecayRate,
											TrackBallStatus, UpdatePoint);
			}

			//Display a warning if cue ball is not found
			if (pCueBall == null)
			{
				Debug.LogWarning("Cue ball not found. Can't start a game without one, right?");
			}

			//Setting up the game engine's physics bounce threshold,
			//if the threshold is too high, the ball will not bounce
			//of the side cushion if the velocity lower than the threshold
			Physics.bounceThreshold = BounceTreshold;

			HasAwake();
		}

		/// <summary>
		/// Tracks the ball movement and invoke VerifyResult once all the balls have stopped.
		/// </summary>
		/// <param name="obj">BallBase objects that are triggered by collision.</param>
		/// <param name="isRemove">If set to <c>true</c> , the BallBase object will be removed.</param>
		private void TrackBallStatus(object obj, bool isRemove = false)
		{
			if (!isRemove)
			{
				//Add BallBase object triggered by collision to the list
				//so we know the amount of objects to wait.
				if (!_activeBalls.Contains(obj))
					_activeBalls.Add(obj);
			}
			else
			{
				//As the BallBase object stop moving, it will remove
				//itself from the list
				if (_activeBalls.Contains(obj))
					_activeBalls.Remove(obj);
			}

			//Once the list is empty, we can now proceed to the next
			//state.
			if (_activeBalls.Count == 0) VerifyResult();
		}

		/// <summary>
		/// Updates the player score.
		/// </summary>
		/// <param name="Score">Score.</param>
		private void UpdatePlayerScore(int Score)
		{
			Players[pCurrentPlayerId].UpdateScore(Score);
		}

		#endregion

		#region Protected methods

		protected Result GetWinner()
		{
			int highestScoreId = 0;
			bool isTie = false;

			for (int i = 0; i < Players.Length; i++)
			{
				if (i > 0)
				{
					if (Players[i].Score > Players[i - 1].Score)
					{
						highestScoreId = i;
					}

					isTie = (Players[i].Score == Players[i - 1].Score) ? true : false;
				}
			}

			Result r = new Result
			{
				WinnerName = (isTie) ? string.Empty : Players[highestScoreId].Name,
				IsTie = isTie
			};

			return r;
		}

		/// <summary>
		/// Move to next player in the list.
		/// </summary>
		protected void NextPlayer()
		{
			pCurrentPlayerId++;
			if (pCurrentPlayerId >= Players.Length) pCurrentPlayerId = 0;
			Players[pCurrentPlayerId].SwitchToPlayer();
		}

		/// <summary>
		/// Applies the force and direction to the cue ball. 
		/// </summary>
		/// <param name="force">Force.</param>
		/// <param name="direction">The direction to move the ball to.</param>
		protected void ApplyForceToCueBall(float force, Vector3 direction)
		{
			((BallBase)pCueBall).ApplyForce(direction * force);
		}

		#endregion

		#region Virtual methods

		/// <summary>
		/// This will invoke the HasAwake override method
		/// </summary>
		public virtual void HasAwake() { }

		/// <summary>
		/// Activates all the components and starts the game.
		/// </summary>
		public virtual void StartGame() { }

		/// <summary>
		/// Resets the game to its original state.
		/// </summary>
		public virtual void ResetGame()
		{
			//Reset balls to its original position
			for (int i = 0; i < PoolBalls.Length; i++)
			{
				PoolBalls[i].Reset();
			}

			//Reset all player's score and switch back to first player's turn
			for (int i = 0; i < Players.Length; i++)
			{
				if (i == 0) Players[i].SwitchToPlayer();
				Players[i].Reset();
			}
			pCurrentPlayerId = 0;
		}

		public virtual void VerifyResult(bool isGameEnd = true) { }

		public virtual void UpdatePoint(int score = 0, BallBase baseObj = null)
		{
			UpdatePlayerScore(score);
		}

		#endregion
	}

}