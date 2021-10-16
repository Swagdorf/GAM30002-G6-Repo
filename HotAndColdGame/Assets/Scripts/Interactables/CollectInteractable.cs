using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class dedicated to objects that are collectable by the player via the interaction ('E') button.
/// So far, this includes the Raygun and Journals. 
/// Last edit: InteractJournal() - Uses first entry of Journal list for now.
/// By: Charli - 23/9/21
/// </summary>
public class CollectInteractable : InteractableBase
{
    public string itemName;
    protected InteractionType interactionType = InteractionType.Use;
    private PlayerInput playerInput;
    public bool destroyOnCollect = true;
    public int int_data;

    public AudioClip pickup_sound;

    public void Start() 
    {
        
    }

    //Runs when interaction begins
    public override void OnInteractEnter(PlayerInput playerInputRef, float delay = 0)
    {
       switch(itemName)
        {
            case "Raygun":
                StartCoroutine(InteractRaygun(delay));
                break;

            case "Journal":
                StartCoroutine(InteractJournal(delay));
                break;

            case "Artifact":
                StartCoroutine(InteractArtifact(delay));
                break;
        }
    }

    IEnumerator InteractRaygun(float delay)
    {
        // wait for animation and stuff
        yield return new WaitForSeconds(delay);

        // do stuff
        GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.SetGunUpgradeState(int_data);
        GameObject.Find("Player").GetComponent<GunFXController>().SetWeaponMods(int_data);

        switch(GameObject.Find("Player").GetComponent<PlayerController>().raygunScript.gunUpgradeState)
        {
            case  RayCastShootComplete.gunUpgrade.None:
                GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Neutral);
                Camera.main.GetComponent<AudioSource>().clip = pickup_sound;
                Camera.main.GetComponent<AudioSource>().Play();
                break;

            case RayCastShootComplete.gunUpgrade.One:
                GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Negative);
                break;

            case RayCastShootComplete.gunUpgrade.Two:
                //GameObject.Find("Player").GetComponent<ReticleFXController>().ChangeState(ReticleFXController.ReticleState.Positive);
                break;
        }

        GameMaster.instance.GetComponent<CollectionSystem>().FoundCollectable(this);

        if (destroyOnCollect)
            Destroy(gameObject);
    }

    IEnumerator InteractJournal(float delay)
    {
        // wait for animation and stuff
        yield return new WaitForSeconds(delay);
        GameMaster.instance.GetComponent<CollectionSystem>().FoundCollectable(this);
        //GameObject.Find("UI").GetComponentInChildren<Journal_Reader>().Display_Journal(GameMaster.instance.GetComponent<CollectionSystem>().RetrieveFromAllJournals(GetComponent<CollectInteractable>().int_data).text[0], GameMaster.instance.GetComponent<CollectionSystem>().RetrieveFromAllJournals(GetComponent<CollectInteractable>().int_data).text[1], 0);

        Camera.main.GetComponent<AudioSource>().clip = pickup_sound;
        Camera.main.GetComponent<AudioSource>().Play();

       

        //GameObject.Find("UI").GetComponentInChildren<PauseController>().IsPaused = true;
        // do stuff
        if (destroyOnCollect)
            Destroy(gameObject);
    }

    IEnumerator InteractArtifact(float delay)
    {
        Debug.Log("FOUND ARTIFACT");
        // wait for animation and stuff
        yield return new WaitForSeconds(delay);
      /*  GameObject.Find("UI").GetComponentInChildren<Journal_Reader>().Display_Journal(GetComponent<Journal>()
            .EntryLog[0], GetComponent<Journal>().EntryLog[1], int_data);*/
        Camera.main.GetComponent<AudioSource>().clip = pickup_sound;
        Camera.main.GetComponent<AudioSource>().Play();

        GameMaster.instance.GetComponent<CollectionSystem>().FoundCollectable(this);

        // do stuff
        if (destroyOnCollect)
            Destroy(gameObject);
    }


    //Runs after interaction is complete
    public override void OnInteractExit()
    {
        //playerControls = null;      
       
    }

    //Runs every frame the interaction continues
    public override void OnInteracting()
    {      
        
    }    

    public override InteractionType pInteractionType
    {
        get
        {
            return interactionType;
        }
        set
        {
            interactionType = value;
        } 
    }

    public override PlayerInput pPlayerInput 
    {
        get
        {
            return playerInput;
        }
        set
        {
            playerInput = value;
        } 
    }
}
