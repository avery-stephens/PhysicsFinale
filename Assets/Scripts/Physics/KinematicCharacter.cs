using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicCharacter : MonoBehaviour
{
	[SerializeField] float speed = 5;
	[SerializeField] float turnRate = 180;
	[SerializeField] bool isRelative = false;

	// Update is called once per frame
	void Update()
	{
		// translation
		Vector3 direction = Vector3.zero;
		direction.z = Input.GetAxis("Vertical");
		transform.Translate(direction * speed * Time.deltaTime, isRelative ? Space.Self : Space.World);

		// rotation
		float yaw = Input.GetAxis("Horizontal");
		transform.Rotate(Vector3.up * yaw * turnRate * Time.deltaTime);


		// draw axis
		Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
		Debug.DrawRay(transform.position, transform.right * 2, Color.red);
		Debug.DrawRay(transform.position, transform.up * 2, Color.green);
	}
}
