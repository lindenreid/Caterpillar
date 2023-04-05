using UnityEngine;

public class Draggable : MonoBehaviour {

    public bool draggable = true;
    private HUD hud;

    public void SetHUD (HUD hud) {
        this.hud = hud;
    }

	public void OnDrag(){ 
        if (draggable)
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mouse.x, mouse.y, transform.position.z); 

            if(hud != null) {
                hud.OnBodyPartDrag(this);
            }
        }
	}

    public void Rotate () {
        transform.Rotate(new Vector3(0, 0, 45), Space.Self);
    }

}
