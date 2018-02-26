using UnityEngine;

public class Draggable : MonoBehaviour {

    public bool draggable = true;

	public void OnDrag(){ 
        if (draggable)
            transform.position = Input.mousePosition; 
	}

}
