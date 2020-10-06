using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootIfGrabbed : MonoBehaviour
{
    private SimpleShoot simpleShoot;
    private OVRGrabbable ovrGrabbable;
    public bool isGrabbed;
    public OVRInput.Button shootingButton;
    public AudioSource source;
    public AudioClip[] shootingClips;
    private bool soundplayed = false;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
      simpleShoot = GetComponent<SimpleShoot>();
      ovrGrabbable = GetComponent<OVRGrabbable>();
      rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

      isGrabbed = ovrGrabbable.isGrabbed;

      if (isGrabbed)
        {

          rb.isKinematic = true;

          if (OVRInput.GetDown(shootingButton, ovrGrabbable.grabbedBy.GetController()))
          {
            simpleShoot.TriggerShoot();
            StartCoroutine(Haptics(1, 1, 0.15f));
          } else if (!source.isPlaying && !soundplayed)
          {
                int randomClip = Random.Range (0, shootingClips.Length);
                source.clip = shootingClips[randomClip];
                source.Play ();
                soundplayed = true;
          }

        }

      if (!isGrabbed)
        {
          soundplayed = false;
        //    gameObject.transform.parent = null;
          if (this.transform.localPosition == Vector3.zero)
           {
           } else {
             rb.isKinematic = false;
           }
        }



    }

    IEnumerator Haptics(float frequency, float amplitude, float duration)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, ovrGrabbable.grabbedBy.GetController());

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0, 0, ovrGrabbable.grabbedBy.GetController());
    }
}
