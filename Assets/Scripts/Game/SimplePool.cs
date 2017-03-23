using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OniFactory.Ball;
using OniFactory.Game.UI;

namespace OniFactory.Game.Pool
{
	public class SimplePool : PoolGameBase
	{
		//Custom cue stick object.
		public CueStick CueStick; 

		//Custom UI object that can apply force.
		public ShotSliderPanel ShotSlider; 

		//Custom Player score panel UI.
		public PlayerPanel PlayerScorePanel;

		//Custom game start panel UI.
		public GameStartPanel GameStartPanel;

		//Custom game reset panel UI.
		public GameResetPanel GameResetPanel;

		public float ShotForceMultiplier;

		private bool _stopScoreUpdate = false;
		private bool _colourBallSunk = false;
		private bool _cueBallSunk = false;
		private Vector3 _direction = Vector3.zero;

		public override void HasAwake()
		{
			//Setting up the cue stick
			CueStick.Initialise(OnRotateCueStick);

			//Setting up the shot slider
			ShotSlider.Initialise(OnForceApplied);

			//Map the player object to the score UI
			PlayerScorePanel.Initialise(Players);

			//Hide the game reset panel
			GameResetPanel.Hide();
		}

		/// <summary>
		/// Starts the game.
		/// </summary>
		public override void StartGame()
		{
			//Custom procedure can run here when the
			//game start button is pressed.

			GameStartPanel.Hide();
			CueStick.NextShot();

			base.StartGame();
		}

		/// <summary>
		/// Resets the game.
		/// </summary>
		public override void ResetGame()
		{
			CueStick.Reset();
			GameResetPanel.Reset();

			base.ResetGame();
		}

		/// <summary>
		/// Verify result using the custom game rule.
		/// </summary>
		public override void VerifyResult(bool isGameEnd = true)
		{
			//Find out if all balls has been sunk into the pocket
			for (int i = 0; i < PoolBalls.Length; i++)
			{
				//Only colour balls are needed
				if (!PoolBalls[i].IsCueBall)
				{
					//If the ball is on the table, the game continues to next shot.
					if (PoolBalls[i].gameObject.activeInHierarchy)
					{
						isGameEnd = false;
					}
				}
			}

			if (!isGameEnd)
			{
				StartCoroutine(PrepareForNextShot());
			}
			else
			{
				GameResetPanel.Show();
				GameResetPanel.UpdateNotificationText(GetWinner());
			}
		}

		/// <summary>
		/// Setting up for next shot.
		/// </summary>
		/// <returns>The for next shot.</returns>
		private IEnumerator PrepareForNextShot()
		{
			yield return new WaitForSeconds(2);

			pCueBall.ActivateBall();
			CueStick.NextShot();

			if (!_colourBallSunk || _cueBallSunk)
				NextPlayer();

			_stopScoreUpdate = false;
			_colourBallSunk = false;
			_cueBallSunk = false;
		}

		/// <summary>
		/// Updates the score. Points are calculated here before it's added to player's score
		/// </summary>
		public override void UpdatePoint(int score = 0,BallBase baseObj = null)
		{
			if (!_stopScoreUpdate)
			{
				base.UpdatePoint(score, baseObj);
				if (score < 0)
				{
					_cueBallSunk = true;
					_stopScoreUpdate = true; //Stop subsequent point being added into the score
				}
				else if (score > 0) 
				{ 
					_colourBallSunk = true; 
				}
			}
		}

		/// <summary>
		/// Applies force to the cue ball and hide the cue stick.
		/// </summary>
		/// <param name="force">Force.</param>
		private void OnForceApplied(float force)
		{
			if (_direction == Vector3.zero)
				_direction = new Vector3(0, 0, 1);
			
			ApplyForceToCueBall(force * ShotForceMultiplier, _direction);
			CueStick.DisplayToggle(false);
		}

		/// <summary>
		/// Invoke this when the cue stick rotates.
		/// </summary>
		/// <param name="isRotate">If set to <c>true</c>, the cue stick is rotating.</param>
		/// <param name="direction">The direction where the cue stick is aiming.</param>
		private void OnRotateCueStick(bool isRotate,Vector3 direction)
		{
			bool isActive = isRotate ? false : true;

			_direction = direction;

			PlayerScorePanel.DisplayToggle(isActive);
			ShotSlider.DisplayToggle(isActive);
		}
	}	
}