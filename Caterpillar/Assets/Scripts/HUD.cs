using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {

    // UI
	public GameObject EatMessage;
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

    // Sharing
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWEET_LANGUAGE = "en";
    private string tweetMsg = "Visit So Good Games at https://sogoodgames.itch.io/";

    // Internal
    private bool controlsEnabled = false; // please set with SetControlsEnabled
    private ArrayList currentCollisions;
    private List<BodyPart> bodyPartsCollected;
    // references to instantiated body part images, for easy clean-up upon restart
    private List<GameObject> bodyPartGOs;
    private List<GameObject> endUIbodyPartGOs;


    // ----- Monobehavior ----- 

    void Awake()
    {
        SetControlsEnabled(false);

        currentCollisions = new ArrayList();
        bodyPartsCollected = new List<BodyPart>();

        bodyPartGOs = new List<GameObject>();
        endUIbodyPartGOs = new List<GameObject>();
    }

    void Update()
    {
        if(controlsEnabled && Input.GetKeyUp(KeyCode.Space) && currentCollisions.Count > 0)
        {
            TryEatBodyPart(currentCollisions[0] as BodyPart);
        }
    }

    // ----- Button Clicks ----- 

    public void HandleBeginButtonClicked()
    {
        SetControlsEnabled(true);
        TitleScreen.GetComponent<FadeComponent>().StartFadeOut();
    }

    public void HandleFinishButtonClicked()
    {
        // disable game
        BodyImageHanger.SetActive(false);
        EatMessage.SetActive(false);
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

            bodyObject.GetComponent<FadeComponent>().StartFadeIn();

            endUIbodyPartGOs.Add(bodyObject);
        }
    }

    public void HandleShareButtonClicked()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(tweetMsg));
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
        foreach (GameObject obj in bodyPartGOs)
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

    // ----- Public functions ----- 

    public void BodyColliderEnter(bool enter, BodyPart bodyPart)
	{
        if (enter)
        { 
            currentCollisions.Insert(0, bodyPart); // insert at 0 & pop at 0 so that we always get most recent collision
            EatMessage.SetActive(true);
        }
        else
        {
            currentCollisions.Remove(bodyPart);
            if(currentCollisions.Count < 1)
                EatMessage.SetActive(false);
        }
	}


    // ----- Private functions ----- 

    private void TryEatBodyPart(BodyPart bodyPart)
    {
        if (AlreadyHaveBodyPart(bodyPart.Type))
            return;
        bodyPartsCollected.Add(bodyPart);

        bodyPart.gameObject.SetActive(false);

        GameObject bodyObject = Instantiate(BodyPartImagePrefab, BodyImageHanger.transform.position, Quaternion.identity);
        bodyObject.GetComponent<Image>().sprite = bodyPart.Sprite;
        bodyObject.GetComponent<Image>().preserveAspect = true;
        bodyObject.transform.SetParent(BodyImageHanger.transform, false);

        bodyPartGOs.Add(bodyObject);

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

}
