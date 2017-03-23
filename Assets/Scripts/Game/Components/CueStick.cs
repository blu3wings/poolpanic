using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OniFactory.Game.Pool
{
	public class CueStick : MonoBehaviour
	{
		public float Sensitivity;
		public GameObject CueObject;
		public LineRenderer lr;

		private Vector3 _mouseOffset;
		private Vector3 _rotation;
		private Vector3 _mouseReference;
		private bool _isRotating = false;
		private float _origCueDistance;
		private UnityAction<bool,Vector3> _onRotateCueStick;
		private bool _isActivated = false;
		private Vector3 _direction;

		public void Initialise(UnityAction<bool,Vector3> OnRotateCueStick)
		{
			_rotation = Vector3.zero;
			_origCueDistance = CueObject.transform.localPosition.z;
			lr.gameObject.SetActive(false);

			_onRotateCueStick = OnRotateCueStick;
		}

		public void Reset()
		{
			_mouseOffset = Vector3.zero;
			_rotation = Vector3.zero;
			_mouseReference = Vector3.zero;
			//DisplayToggle(true);
			NextShot();
		}

		private void Update()
		{
			if (!_isActivated) return;

			Ray origin = new Ray(transform.position, transform.forward);
			Aim(origin);

			if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

			if (Input.GetMouseButtonDown(0)) OnMouseDown();
			if (Input.GetMouseButtonUp(0)) OnMouseUp();

			if (_isRotating)
			{
				// offset
				_mouseOffset = (Input.mousePosition - _mouseReference);

				// apply rotation
				_rotation.y = -(_mouseOffset.x + _mouseOffset.y) * Sensitivity;

				// rotate
				transform.Rotate(_rotation);

				// store mouse
				_mouseReference = Input.mousePosition;

				_onRotateCueStick(_isRotating,_direction);
			}
		}

		private void Aim(Ray r)
		{
			RaycastHit hit;
			if (Physics.Raycast(r, out hit, 1000))
			{
				lr.SetPosition(0, r.origin);
				lr.SetPosition(1, hit.point);

				_direction = hit.point - r.origin;
				_direction = _direction.normalized;
			}
		}

		private void OnMouseDown()
		{
			// rotating flag
			_isRotating = true;

			// store mouse
			_mouseReference = Input.mousePosition;
		}

		private void OnMouseUp()
		{
			_isRotating = false;
			_onRotateCueStick(_isRotating,_direction);
		}

		public void MoveCueStick(float MoveDelta)
		{
			CueObject.transform.localPosition = new Vector3(
				CueObject.transform.localPosition.x, 
				CueObject.transform.localPosition.y, 
				_origCueDistance - MoveDelta);

		}

		public void DisplayToggle(bool IsActive)
		{
			lr.gameObject.SetActive(IsActive);
			CueObject.SetActive(IsActive);
		}

		/// <summary>
		/// Set the cue stick and guide ready for next shot.
		/// </summary>
		/// <param name="IsActive">If set to <c>true</c> is active.</param>
		public void NextShot(bool IsActive = true)
		{
			_isActivated = true;
			DisplayToggle(IsActive);
		}
	}	
}