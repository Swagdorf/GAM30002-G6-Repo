﻿using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class RayCastShootComplete : MonoBehaviour {

	public float tempchange = 1f;
	public float weaponRange = 50f;
	public float hitForce = 5f;
	public Transform gunEnd;
	private Camera fpsCam;
    public AudioManager audioManager;
    public GameObject spherecollider;
    public GameObject particleAtEnd_ice;
    public GameObject particleAtEnd_fire;

    //Laser Properties
    public LineRenderer laserLine;
    public LineRenderer lightning;
    public Material hotMaterial;
    public Material coldMaterial;
    public bool cold = true;
    //public Color col = Color.blue;

    public bool CanShoot { get; set;}
    public bool TriggerHeld { get; set;}
    public bool ModeSwitched { get; set;}

	void Start () 
	{
		laserLine = GetComponent<LineRenderer>();
		fpsCam = GetComponentInParent<Camera>();
        CanShoot = true;

        Color colour = GameMaster.instance.colourPallete.Neutral;
        laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
        //lightning.startColor = colour;
        //lightning.endColor = colour;
    }

    public void SwapBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ModeSwitched = true;
            ChangeMode();
        } 
    }

    public void FireBeam(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TriggerHeld = true;
        }
        else if (context.canceled)
        {
            TriggerHeld = false;
        }
    }   

    public void ChangeMode()
    {
        cold = !cold;
    }
	

	void LateUpdate () 
	{
        if (ModeSwitched)
        {
            Debug.Log("MODE SWITCHED");
            ModeSwitched = false;
        }
           
        // Change Temperature States
        /*        
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            cold = true;
            print(cold);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            cold = false;
            print(cold);
        }
        */

        if (TriggerHeld && CanShoot && !GetComponentInParent<GunFXController>().inspectingWeapon) 
		{
            if (audioManager != null)
            {
                //audioManager.Play("LazerStart");
                audioManager.Play("Lazer");
            }

            laserLine.enabled = true;
            lightning.enabled = true;

            if (cold == true)
            {
                //col = Color.blue;
                Color colour = GameMaster.instance.colourPallete.Negative;
                laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
                laserLine.material = coldMaterial;
                lightning.startColor = colour;
                lightning.endColor = colour;
                tempchange = -60f * Time.deltaTime;
            }

            if (cold == false)
            {
                //col = Color.red;
                Color colour = GameMaster.instance.colourPallete.Positive;
                laserLine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colour);
                laserLine.material = hotMaterial;
                lightning.startColor = colour;
                lightning.endColor = colour;
                tempchange = 60f * Time.deltaTime;
            }

            //laserLine.startColor = col;
            //laserLine.endColor = col;

            Vector3 rayOrigin = fpsCam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;
			laserLine.SetPosition (0, gunEnd.position);
			//lightning.SetPosition (0, gunEnd.position);

			if (Physics.Raycast (rayOrigin, fpsCam.transform.forward, out hit, weaponRange))
			{
				laserLine.SetPosition (1, hit.point);
                lightning.SetPosition(1, hit.point);
                lightning.SetPosition(1, rayOrigin + (fpsCam.transform.forward * 2));

                //ITemperature objtemp = hit.collider.GetComponent<ITemperature>();
                ITemperature objtemp = hit.collider.GetComponentInParent<ITemperature>();

				if (objtemp != null)
				{
					objtemp.ChangeTemperature(tempchange);
				}

				if (hit.rigidbody != null)
				{
					//hit.rigidbody.AddForce (-hit.normal * 20);
                }

                
                //Instantiate(spherecollider, hit.point, Quaternion.identity);
                //spherecollider.GetComponent<ShootEnd>().temperature = tempchange;
                if (cold == true)
                {
                    //particleAtEnd_ice.SetActive(true);
                    //particleAtEnd_fire.SetActive(false);
                    particleAtEnd_ice.transform.position = hit.point;

                }
                if (cold == false)
                {
                    //particleAtEnd_fire.SetActive(true);
                    //particleAtEnd_ice.SetActive(false);
                    particleAtEnd_fire.transform.position = hit.point;
                }
                

            }
			else
			{
                laserLine.SetPosition (1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                //lightning.SetPosition (1, rayOrigin + (fpsCam.transform.forward * 2));
			}
		}
        else
        {
            laserLine.enabled = false;
            lightning.enabled = false;
            //particleAtEnd_ice.SetActive(false);
            //particleAtEnd_fire.SetActive(false);
            if (audioManager != null)
            {
                //audioManager.Play("LazerEnd");
                audioManager.Stop("Lazer");
            }
        }        
	}
}