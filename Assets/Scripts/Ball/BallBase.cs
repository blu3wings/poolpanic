using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OniFactory.Game.Pool;

namespace OniFactory.Ball
{
	public class BallBase : MonoBehaviour
	{
		public bool IsCueBall = false;

		//Can be used to store information such as ball type
		//in a snooker game.
		public string UniqueIdentifier;

		//How much point to reward or penalise to the player when this
		//ball goes into the pocket.
		public int Points;

		private Rigidbody _r;
		private float _velocityDecayRate;
		private bool _isInitialised = false;
		private UnityAction<object, bool> _onTrackBallStatus;
		private UnityAction<int, BallBase> _onUpdatePoint;
		private Vector3 _originalPosition;
		private bool _hasStopped = false;

		#region Private methods
		/// <summary>
		/// Handles collision detection.
		/// </summary>
		/// <param name="col">Col.</param>
		private void OnCollisionEnter(Collision col)
		{
			if ((col.gameObject.GetComponent<PoolBall>()
				 || col.gameObject.name.Equals("Cushion"))
				&& _isInitialised)
			{
				if (_r.velocity.sqrMagnitude > 0)
				{
					ActivateBall();

					//Add this ball to the tracker.
					_onTrackBallStatus(this, false);
				}
			}

			if (col.gameObject.GetComponent<Pocket>()
				&& _isInitialised)
			{
				//_onUpdateScore(IsCueBall, 1);
				UpdatePoint();

				//Remove this ball object from the tracker.
				_onTrackBallStatus(this, true);
				DisplayToggle(false);
			}
		}

		private void FixedUpdate()
		{
			if (_r.velocity.sqrMagnitude > 0)
			{
				_r.velocity = _r.velocity * _velocityDecayRate;
				_r.angularVelocity = _r.angularVelocity * _velocityDecayRate;

				if (_r.velocity.sqrMagnitude < 0.09f)
				{
					_r.velocity = Vector3.zero;
					_r.angularVelocity = Vector3.zero;

					//Use this condition to resolve the jitter issue.
					if (!_hasStopped)
					{
						//Remove this ball object from the tracker.
						_onTrackBallStatus(this, true);
					}

					_hasStopped = true;
				}
			}
		}
		#endregion

		#region Virtual methods
		/// <summary>
		/// Initialises the ball.
		/// </summary>
		/// <param name="velocityDecayRate">Velocity decay rate.</param>
		/// <param name="OnTrackBallStatus">On track ball status.</param>
		/// <param name="OnUpdatePoint">On update score.</param>
		public virtual void InitialiseBall(float velocityDecayRate,
										   UnityAction<object, bool> OnTrackBallStatus,
										   UnityAction<int, BallBase> OnUpdatePoint)
		{
			_r = GetComponent<Rigidbody>();
			_originalPosition = gameObject.transform.localPosition;

			_velocityDecayRate = velocityDecayRate;
			_onTrackBallStatus = OnTrackBallStatus;
			_onUpdatePoint = OnUpdatePoint;
			_isInitialised = true;
		}

		public virtual void ApplyForce(Vector3 direction)
		{
			if (_r != null)
			{
				_r.AddForce(direction, ForceMode.Impulse);
			}
		}

		/// <summary>
		/// Invoke update point callback to PoolGameBase.
		/// </summary>
		public virtual void UpdatePoint()
		{
			if (_onUpdatePoint != null)
				_onUpdatePoint(Points, this);
		}

		/// <summary>
		/// Toggle to show or hide the ball
		/// </summary>
		/// <param name="IsActive">If set to <c>true</c> is active.</param>
		public virtual void DisplayToggle(bool IsActive, bool isFullReset = false)
		{
			_r.isKinematic = (IsActive == true ? false : true);
			if (!IsActive)
			{
				_r.velocity = Vector3.zero;
				_r.angularVelocity = Vector3.zero;
			}

			gameObject.SetActive(IsActive);

			if (isFullReset)
			{
				gameObject.transform.position = _originalPosition;
			}
			else if (!gameObject.activeInHierarchy)
			{
				gameObject.transform.position = _originalPosition;
			}
		}

		/// <summary>
		/// Prepares the ball for next shot.
		/// </summary>
		public void ActivateBall()
		{
			_hasStopped = false;
			DisplayToggle(true);
		}

		public virtual void Reset()
		{
			DisplayToggle(true, true);
			_hasStopped = false;
		}
		#endregion
	}
}