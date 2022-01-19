using UnityEngine;

public class HealthManager : MonoBehaviour {


	public bool IsGodMod = false;
	public float hitPoint = 100f;
	//public AudioSource audioSource;
	//public AudioClip hitSound;



	public void ApplyDamage(float damage){
		//audioSource.PlayOneShot (hitSound);
		if (!IsGodMod)
		{
			hitPoint -= damage;
			if (hitPoint <= 0) {
				Destroy (gameObject);
				Debug.Log ("Dead " + this.gameObject.name);

			}
		}

	}
}