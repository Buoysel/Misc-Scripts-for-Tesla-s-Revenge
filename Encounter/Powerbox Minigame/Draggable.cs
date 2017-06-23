using UnityEngine;
using System.Collections;

public class Draggable : MonoBehaviour {

	//Makes objects draggable in the game. will be attached to the wires that have yet to be instantiated.

	private Vector3 screenPoint;
	private Vector3 offset;

	/*void OnMouseDown()
	{
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + 10f, Input.mousePosition.y, screenPoint.z /*-2f));
	}*/

	void Start()
	{
		float mousey = Input.GetAxis ("Mouse Y");
		offset.y = mousey - 0.3f;
	}

	void Update()
	{
		//get 2D mouse position
		Vector3 mousePos2D = Input.mousePosition;

		//convert mouse position to 3D world coords
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

		if (this.name.StartsWith("L_"))
			transform.position = mousePos3D + offset;
		else
			transform.position = mousePos3D;

		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

	}
}