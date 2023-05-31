using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour
{
	Vector3 mousePositionOffset;
	Rigidbody2D rb;
	SpriteRenderer spriteRenderer;

	private Vector3 GetMouseWorldPosition()
	{
		//capture mouse position & return WorldPoint
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseDown()
	{
		mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
		rb.gravityScale = 0;
	}
	private void OnMouseDrag()
	{
		spriteRenderer.color = Color.cyan;

        if (Input.GetKey(KeyCode.Q))
		{
			transform.Rotate(Vector3.forward * 180 * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.E))
		{
			transform.Rotate(Vector3.forward * -180 * Time.deltaTime);
		}
		transform.position = Vector3.Lerp(transform.position, GetMouseWorldPosition() + mousePositionOffset, 0.1f);
	}
	private void OnMouseEnter()
	{
		spriteRenderer.color = Color.cyan;
	}

	private void OnMouseExit()
	{
		spriteRenderer.color = Color.white;
	}

	private void OnMouseUp()
	{
		rb.gravityScale = 1;
	}
}