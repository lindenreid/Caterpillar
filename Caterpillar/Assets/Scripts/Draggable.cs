using UnityEngine;

public class Draggable : MonoBehaviour {

    public bool draggable = true;
    private HUD hud;
    private Vector3 originalScale;

    public void SetHUD (HUD hud) {
        this.hud = hud;
        originalScale = transform.localScale;
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

    public void Scale(bool up) {
        if(up) {
            transform.localScale = transform.localScale * 1.5f;
        } else {
            transform.localScale = transform.localScale / 1.5f;
        }
    }

}
