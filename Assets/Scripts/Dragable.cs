using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragable : MonoBehaviour
{
	Vector3 mousePositionOffset;
	[SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] LineHandler lineHandler;
    public bool dragable = true;

    private Vector3 GetMouseWorldPosition()
	{
		//capture mouse position & return WorldPoint
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	private void OnMouseDown()
	{
		mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
		rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        lineHandler.Activate();
        lineHandler.destination = FindObjectOfType<ControllerCharacter2D>().transform;
    }
    private void OnMouseUp()
    {
        rb.gravityScale = 1;
        lineHandler.Deactivate();
        spriteRenderer.color = Color.white;
    }
    private void OnMouseDrag()
	{
        if (dragable)
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
	}
	private void OnMouseEnter()
	{
        if (dragable) spriteRenderer.color = Color.cyan;
    }

	private void OnMouseExit()
	{
        if (!Input.GetMouseButton(0)) spriteRenderer.color = Color.white;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("CollisionEnter");
        if (collision.gameObject.CompareTag("Player")) dragable = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) dragable = true;
    }
}