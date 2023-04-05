using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO; // data import

public class HUD : MonoBehaviour {

    // UI
    public GameObject BodyImageHanger;
    public GameObject FinishButton;
    public GameObject EndGameUI;
    public GameObject TitleScreen;

    // Endgame body part hangers
    public Transform Hanger_Head;
    public Transform Hanger_LeftArm;
    public Transform Hanger_RightArm;
    public Transform Hanger_Torso;
    public Transform Hanger_Legs;
    
    // Prefabs
    public GameObject BodyPartImagePrefab;
    
    // Game references
    public CharacterMovement2D CharacterMovement;
    public Transform Indicator;
	public ReactionText ReactionText;
	public string ReactionTextFileName = "eatlines.json";

    // Internal
    private bool controlsEnabled = false; // please set with SetControlsEnabled
    private ArrayList currentCollisions;
    private List<BodyPart> bodyPartsCollected;
    // references to instantiated body part images, for easy clean-up upon restart
    private Dictionary<GameObject, BodyPartType> bodyPartGOs;
    private List<GameObject> endUIbodyPartGOs;
	private List<string> reactionTextList;

    private Draggable currentlyClickedDraggable;


    // ----- Monobehavior ----- 

    void Awake()
    {
        SetControlsEnabled(false);

        currentCollisions = new ArrayList();
        bodyPartsCollected = new List<BodyPart>();

		bodyPartGOs = new Dictionary<GameObject, BodyPartType>();
        endUIbodyPartGOs = new List<GameObject>();

		ImportReactionText ();
    }

    void Update()
    {
        if(controlsEnabled && Input.GetKeyUp(KeyCode.Space) && currentCollisions.Count > 0)
        {
            TryEatBodyPart(currentCollisions[0] as BodyPart);
        }

        if(!controlsEnabled && currentlyClickedDraggable != null)
        {
            if(Input.GetKeyDown("r"))
            {
                currentlyClickedDraggable.Rotate();
            }
            else if(Input.GetKeyDown("d"))
            {
                currentlyClickedDraggable.Scale(true);
            }
            else if(Input.GetKeyDown("a"))
            {
                currentlyClickedDraggable.Scale(false);
            }
        }
    }

    // ----- Button Clicks ----- 

    public void HandleBeginButtonClicked()
    {
        currentCollisions.Clear(); // this shouldn't have to be here, it's a hack because i automatically register 9 collisions upon starting the game and IDK WHY???
        SetControlsEnabled(true);
        TitleScreen.GetComponent<FadeComponent>().StartFadeOut();
    }

    public void HandleFinishButtonClicked()
    {
        // disable game
        BodyImageHanger.SetActive(false);
        FinishButton.SetActive(false);

        SetControlsEnabled(false);

        // show end UI
        EndGameUI.SetActive(true);

        foreach(BodyPart bodyPart in bodyPartsCollected)
        {
            Transform hanger = transform;
            switch(bodyPart.Type)
            {
                case BodyPartType.Head:
                    hanger = Hanger_Head; break;
                case BodyPartType.LeftArm:
                    hanger = Hanger_LeftArm; break;
                case BodyPartType.RightArm:
                    hanger = Hanger_RightArm; break;
                case BodyPartType.Torso:
                    hanger = Hanger_Torso; break;
                case BodyPartType.Legs:
                    hanger = Hanger_Legs; break;
            }

            GameObject bodyObject = Instantiate(BodyPartImagePrefab, hanger.position, Quaternion.identity);
            bodyObject.GetComponent<Image>().sprite = bodyPart.Sprite;
            bodyObject.GetComponent<Image>().preserveAspect = true;
            bodyObject.transform.SetParent(hanger, false);

            Draggable draggable = bodyObject.GetComponent<Draggable>();
            draggable.draggable = true;
            draggable.SetHUD(this);

            bodyObject.GetComponent<FadeComponent>().StartFadeIn();

            endUIbodyPartGOs.Add(bodyObject);
        }
    }

    public void HandleRestartButtonClicked()
    {
        // reset game data
        foreach (BodyPart part in bodyPartsCollected)
            part.gameObject.SetActive(true);

        currentCollisions.Clear();
        bodyPartsCollected.Clear();

        // disable end UI
        EndGameUI.SetActive(false);
        FinishButton.SetActive(false);

        // clear body part UI
		foreach (GameObject obj in bodyPartGOs.Keys)
            Destroy(obj);
        bodyPartGOs.Clear();

        foreach (GameObject obj in endUIbodyPartGOs)
            Destroy(obj);
        endUIbodyPartGOs.Clear();

        // enable UI
        BodyImageHanger.SetActive(true);

        // enable controls
        SetControlsEnabled(true);
    }

    public void OnBodyPartDrag (Draggable d)
    {
        currentlyClickedDraggable = d;
    }

    // ----- Public functions ----- 

    public void BodyColliderEnter(bool enter, BodyPart bodyPart)
	{
        if (enter)
        { 
            currentCollisions.Insert(0, bodyPart); // insert at 0 & pop at 0 so that we always get most recent collision
			Indicator.transform.position = bodyPart.transform.position;
            Indicator.transform.SetParent(bodyPart.transform, true);
        }
        else
        {
            currentCollisions.Remove(bodyPart);
        }
	}

    // ----- Private functions ----- 

    private void TryEatBodyPart(BodyPart bodyPart)
    {
		if (AlreadyHaveBodyPart (bodyPart.Type))
		{
			// remove old part from UI
			foreach (GameObject obj in bodyPartGOs.Keys) {
				if (bodyPartGOs [obj] == bodyPart.Type) {
					bodyPartGOs.Remove (obj);
					Destroy(obj);
					break;
				}
			}
			// remove from list of items collected
			foreach (BodyPart part in bodyPartsCollected){
				if (part.Type == bodyPart.Type) {
					part.gameObject.SetActive (true); // re-enable part
					bodyPartsCollected.Remove (part);
					break;
				}
		     }
		}

		bodyPartsCollected.Add(bodyPart);
		bodyPart.gameObject.SetActive(false);

        GameObject bodyObject = Instantiate(BodyPartImagePrefab, BodyImageHanger.transform.position, Quaternion.identity);
        bodyObject.GetComponent<Image>().sprite = bodyPart.Sprite;
        bodyObject.GetComponent<Image>().preserveAspect = true;
        bodyObject.transform.SetParent(BodyImageHanger.transform, false);

		bodyPartGOs.Add(bodyObject, bodyPart.Type);

        bodyObject.GetComponent<Draggable>().draggable = false;

		ReactionText.Show(reactionTextList[Random.Range(0, reactionTextList.Count - 1)]);

        if (!FinishButton.activeSelf)
            FinishButton.SetActive(true);
    }

    private bool AlreadyHaveBodyPart(BodyPartType type)
    {
        foreach(BodyPart part in bodyPartsCollected)
        {
            if (part.Type == type)
                return true;
        }
        return false;
    }

    private void SetControlsEnabled(bool enable)
    {
        controlsEnabled = enable;
        CharacterMovement.enabled = enable;
    }

	private class ReactionTextList{
		public List<string> lines;
	}

	private void ImportReactionText()
	{
		reactionTextList = new List<string> (){
            "wowzers. :0",
            "hubba hubba.",
            "i'm gonna be the most beautiful boy on the block.",
            "just call me Gerge Clooners ;0",
            "i am so HORNGRY",
            "gimme dat sexy exoskeleton",
            "every girl's crazy bout a sharp dressed man.",
            "this makes me feel like Soldier 69.",
            "yummy~",
            "deeeelish!",
            "nomnomnom",
            "THIS is how to be beautiful.",
            "will this make me feel good about myself?",
            "this is How To Man",
            "Cosmo makes the rules.",
            "All hail lord GQ",
            "NOM",
            "*crunch*",
            "i love the cronch",
            "mmm... tender...",
            "VALIDATE ME",
            "make me a beautiful boy plz!!!",
            "*slurp*",
            "you are what you eat!!",
            "CONSUME AND BECOME",
            "what is a man?",
            "a man is a collection of bOdY pArTs",
            "ooh!;0",
            "unf",
            "ahhh;)",
            "huuuuuhhhhf",
            "asDAHGASHDGH",
            "helP",
            "i want to be a MAYNE",
            "a MANLY MAN",
            "oh he's 6 foot YES",
            "slice me a piece of that CAKE",
            "this won't hurt;)",
            "FEED THE SOUL TO THE ELDER GODS",
            "TAKE YOUR HEART",
            "i will steal ur heart bby",
            "i will steal ur heart nd ur body",
            "C O N S U M E",
            "daddy af",
            ";0;0;0;0",
            "i ate the sherrif",
            "but i did not eat the deputee"
        };
	}
}
