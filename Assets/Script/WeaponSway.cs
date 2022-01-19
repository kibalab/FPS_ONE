using UnityEngine;

public class WeaponSway : MonoBehaviour {

	public float swayAmount = 0.02f;
	public float smoothAmount = 6f;
	public float maxAmount = 0.06f;

	private Vector3 originalPosition;

	// Use this for initialization
	private void Start () {
		originalPosition = transform.localPosition;
	}

	// Update is called once per frame
	private void Update () {
		float positionX = -Input.GetAxis ("Mouse X") * swayAmount;		
		float positionY = -Input.GetAxis ("Mouse Y") * swayAmount;

		Mathf.Clamp (positionX, -maxAmount, maxAmount);
		Mathf.Clamp (positionY, -maxAmount, maxAmount);

		Vector3 swayPosition = new Vector3 (positionX, positionY, 0);

		transform.localPosition = Vector3.Lerp (transform.localPosition, originalPosition + swayPosition, Time.deltaTime * smoothAmount);
	}
}