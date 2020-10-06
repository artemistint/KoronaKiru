using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterThis : MonoBehaviour
{

  private Rigidbody rb;
  private Renderer rend;
  private GameObject animatedMesh;
  public GameObject particles;

	public ShatterScheduler scheduler = null;

	public float requiredForce = 1.0f;

	public float cooldownTime = 0.5f;

	private float timeSinceInstantiated = 0.0f;

  public void Start ()
  {
    rb = GetComponent<Rigidbody>();
    rend = GetComponent<Renderer>();
    animatedMesh = this.transform.Find("AnimatedMesh").gameObject;
  }
	public void Update()
	{
		timeSinceInstantiated += Time.deltaTime;
	}

	public void OnCollisionEnter(Collision collision)
	{
    if (collision.gameObject.tag=="Bullet")
      {
        Instantiate(particles, this.transform.position, this.transform.rotation);
        bool particlesOn = true;
        if (particlesOn)
        {
          rb.isKinematic = false;
          rb.useGravity = true;
          rend.enabled = true;
  				animatedMesh.SetActive(false);
          Destroy(animatedMesh);

    		if (timeSinceInstantiated >= cooldownTime)
    		{
    			if (collision.impactForceSum.magnitude >= requiredForce)
    			{
    				// Find the new contact point
    				foreach (ContactPoint contact in collision.contacts)
    				{
    					if (contact.otherCollider == collision.collider)
    					{
    						// Shatter at this contact point
    						if (scheduler != null)
    						{
    							scheduler.AddTask(new ShatterTask(contact.thisCollider.gameObject, contact.point));
    						}
    						else
    						{
    							contact.thisCollider.SendMessage("Shatter", contact.point, SendMessageOptions.DontRequireReceiver);
    						}

    						break;
    					}
    				}
    			}
    		}
    	}
    }
  }
}
