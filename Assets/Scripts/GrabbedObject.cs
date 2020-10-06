using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbedObject : MonoBehaviour
{
public bool grabbed;
public AudioSource source;
public AudioClip[] swingClips;
private bool soundplayed = false;

//public slot for you to place gripTrans via inspector
public GameObject thing;
private Rigidbody rb;
public bool functionCalled = false;

void Awake() {
    AudioSource source = gameObject.GetComponent<AudioSource> ();
}

void Start()
   {
       rb = thing.GetComponent<Rigidbody>();
       soundplayed = false;
   }

private void Update()
  {

    grabbed = thing.GetComponent<OVRGrabbable>().isGrabbed;

  if (grabbed == true)
    {
    //sets transform of grabbed object to chosen parents rotation & position transform.SetParent(thing.transform) ;
//    transform.position = transform.parent.position;
//    transform.rotation = transform.parent.rotation;
    rb.isKinematic = true;

    if (!source.isPlaying && !soundplayed)
      {
            int randomClip = Random.Range (0, swingClips.Length);
            source.clip = swingClips[randomClip];
            source.Play ();
            soundplayed = true;
      }

    }

  if (grabbed == false)
    {
    soundplayed = false;
//    gameObject.transform.parent = null;
    if (thing.transform.localPosition == Vector3.zero)
     {
     } else {
       rb.isKinematic = false;
     }

    }
  }
}
