using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

	// Player Specification
	public float hitPoint = 100f;
	private float maxHitPoint;

	// References
	public Text hpText;
	public Image hpBar;

	private void Start(){
		maxHitPoint = hitPoint;
		hpText.text = hitPoint + " ";
		hpBar.fillAmount = hitPoint / 100;
	}

	// Debug
	private void Update(){
		if (Input.GetKeyDown (KeyCode.K)) {
			ApplyDamage (Random.Range (1, 20));
		}
	}

	public void ApplyDamage(float damage){
		UpdateHP ();
		hitPoint -= damage;
        if (hitPoint > 5 && hitPoint <= 10)
        {
            hpBar.color = new Color(255 / 255f, 140 / 255f, 0);
        }
        if (hitPoint <= 5)
        {
            hpBar.color = Color.red;
        }
		if (hitPoint <= 0) {
			hitPoint = 0;
			// Die;
			Debug.Log("You Died!");
		}
	}

	private void UpdateHP(){
		hpText.text = hitPoint + "";
		hpBar.fillAmount = hitPoint / 100;
	}
}
