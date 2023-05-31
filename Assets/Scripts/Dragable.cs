using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour
{
	Vector3 mousePositionOffset;
	private Vector3 GetMouseWorldPosition()
	{
		//capture mouse position & return WorldPoint
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseDown()
	{
		mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
		if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
		{
			rb.gravityScale = 0;
		}
	}
	private void OnMouseDrag()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			transform.Rotate(Vector3.forward * 180 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.E))
		{
			transform.Rotate(Vector3.forward * -180 * Time.deltaTime);
		}
		transform.position = GetMouseWorldPosition() + mousePositionOffset;
	}

	private void OnMouseEnter()
	{
		if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
		{
			sr.color = Color.cyan;
		}
	}

	private void OnMouseExit()
	{
		if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
		{
			sr.color = Color.white;
		}
	}

	private void OnMouseUp()
	{
		if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
		{
			rb.gravityScale = 1;
		}
	}
}