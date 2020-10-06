using UnityEngine;
using System.Collections;

public class MeshCutter : MonoBehaviour {

	public Material capMaterial;
	public Material transMaterial;
	public bool isDestroyed = false;
	public bool isGrabbed = false;
	public AudioSource source;
	public AudioClip[] swingClips;
	public AudioClip[] destroyClips;
	private OVRGrabbable ovrGrabbable;
	public float speed;
	public double slashspeed = 3;
	public double killspeed = 1;

	void Awake() {
      AudioSource source = gameObject.GetComponent<AudioSource> ();
 	}

	// Use this for initialization
	void Start () {
		ovrGrabbable = GetComponent<OVRGrabbable>();
	}

	void Update () {

		isGrabbed = GetComponent<OVRGrabbable>().isGrabbed;

//		speed = GetComponent<Rigidbody>().velocity.magnitude;

		if (!source.isPlaying && isGrabbed && speed > slashspeed) {
		          int randomClip = Random.Range (0, swingClips.Length);
		          source.clip = swingClips[randomClip];
		          source.Play ();
		 } else if (isDestroyed == true)
		{
			isDestroyed = false;
			int randomClip = Random.Range (0, destroyClips.Length);
			source.clip = destroyClips[randomClip];
			source.Play ();
			StartCoroutine(Haptics(1, 1, 0.3f));
		}

	}

	void OnTriggerEnter(Collider collision)
     {
	     	if (collision.gameObject.tag=="Enemy" && speed > killspeed)
				{

					StartCoroutine(Haptics(1, 1, 0.3f));
					GameObject animatedMesh = collision.transform.Find("AnimatedMesh").gameObject;
					animatedMesh.SetActive(false);
					GameObject stillMesh = collision.transform.Find("StillMesh").gameObject;
					stillMesh.SetActive(true);

					GameObject victim = stillMesh.GetComponent<Collider>().gameObject;

					GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);


					if(!pieces[1].GetComponent<Rigidbody>())
					{
					pieces[1].AddComponent<Rigidbody>();
//					pieces[1].AddComponent<MeshCollider>();
//					pieces[1].GetComponent<Rigidbody>().isKinematic = true;
//					pieces[1].GetComponent<Rigidbody>().useGravity = false;
					}

					GameObject leftMesh = collision.transform.Find("left side").gameObject;
					leftMesh.gameObject.tag="InnerPiece";

				if(!leftMesh.GetComponent<Rigidbody>())
					{
//					leftMesh.AddComponent<Rigidbody>();
//					leftMesh.GetComponent<Rigidbody>().isKinematic = true;
//					leftMesh.GetComponent<Rigidbody>().useGravity = false;
//					leftMesh.AddComponent<MeshCollider>();
					}

					Destroy(pieces[1], 1);

					isDestroyed = true;

		      } else if (collision.gameObject.tag=="InnerPiece" && speed > killspeed)
					{
						GameObject victim = collision.GetComponent<Collider>().gameObject;

						GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);

						if(!pieces[1].GetComponent<Rigidbody>())
							pieces[1].AddComponent<Rigidbody>();

						Destroy(pieces[1], 1);

						isDestroyed = true;
	     	} else if (collision.gameObject.tag=="BonusObject" && speed > killspeed)
				{
					GameObject victim = collision.GetComponent<Collider>().gameObject;

					GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, transMaterial);

					if(!pieces[1].GetComponent<Rigidbody>())
						pieces[1].AddComponent<Rigidbody>();

					GameObject leftMesh = collision.transform.Find("left side").gameObject;
					leftMesh.gameObject.tag="InnerPiece";

					Destroy(pieces[1], 1);
					Destroy(leftMesh, 1);

					isDestroyed = true;
			}
			}


		IEnumerator Haptics(float frequency, float amplitude, float duration)
    {
        OVRInput.SetControllerVibration(frequency, amplitude, ovrGrabbable.grabbedBy.GetController());

        yield return new WaitForSeconds(duration);

        OVRInput.SetControllerVibration(0, 0, ovrGrabbable.grabbedBy.GetController());
    }

}
