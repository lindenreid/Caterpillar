using UnityEngine;

public class Draggable : MonoBehaviour {

    public bool draggable = true;

	public void OnDrag(){ 
        if (draggable)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouse.x, mouse.y, transform.position.z); 
        }
	}

}
