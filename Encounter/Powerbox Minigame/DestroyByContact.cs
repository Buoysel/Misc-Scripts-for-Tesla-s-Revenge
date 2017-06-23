using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	//Destroy any wires or connectors that are dragged into the trashcan.

	void OnTriggerStay2D(Collider2D other)
	{
		if (Input.GetButtonDown ("Fire1")) 
		{
			SpawnableObjectsList spawnList = PowerboxGameManager.instance.gameObject.GetComponent<SpawnableObjectsList>();
			spawnList.canSpawn = true;

			Destroy (other.gameObject);
		}
	}
}