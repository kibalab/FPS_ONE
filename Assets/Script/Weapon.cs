using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class Weapon : MonoBehaviourPunCallbacks
{

    public FirstPersonController FPSController;
    

	// Weapon Specification
	public string weaponName;
	public int bulletsPerMag;
	public int bulletsTotal;
	public int currentBullets;
	public float range;
	public float fireRate;
	public Vector3 aimPosition;
    public Vector3 firePosition;
    public Quaternion fireRotation;
    private Vector3 originalPosition;
    private float originalAccuracy;
    public bool continuity;

    // Weapon Specification
    public float accuracy;
	public float damage;
    public float Dpower;
    public float Dstart;



    // Parameters
    private float fireTimer;
	public bool isReloading;
	private bool isAiming;
    private bool isRunning;

    // References
    public Transform shootPoint;
	private Animator anim;
	public Text bulletsCurrentText;
    public Text bulletsTotalText;
    public Text gunName;
	public Image aimImage;
	public Transform bulletCasingPoint;
    private CharacterController characterController;

	public ParticleSystem muzzleFlash;
    public GameObject hitSparkPrefab;
	public GameObject hitHolePrefab;
	public GameObject bulletCasing;

	// Sounds
	public AudioSource audioSource;
	public AudioClip shootSound;
	public AudioClip reloadSound;
    public AudioClip drawSound;
    public float drawSoundTime;


    // Recoil
    public Transform camRecoil;
	public Vector3 recoilKickback;
	public float recoilAmount;
	private float originalRecoil;


    // Use this for initialization
    private void Start () {
        if (!photonView.IsMine)
        {
            return;
        }
        FPSController = GameObject.Find("FPSController").GetComponent("FirstPersonController") as FirstPersonController;
        characterController = GetComponentInParent<CharacterController>();
        currentBullets = bulletsPerMag;
		anim = GetComponent<Animator> ();
        bulletsCurrentText.text = currentBullets+"";
        bulletsTotalText.text = " / " + bulletsTotal / bulletsPerMag;
        originalPosition = transform.localPosition;
        originalAccuracy = accuracy;
		originalRecoil = recoilAmount;
	}

	// Update is called once per frame
	private void Update () {
        if (!photonView.IsMine)
        {
            return;
        }
        Run();
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo (0);
		isReloading = info.IsName ("Reload");
        FPSController.r_isReloading = isReloading;


        if (Input.GetKeyDown (KeyCode.R)) {
			DoReload ();
		}

		if (fireTimer < fireRate) {
			fireTimer += Time.deltaTime;
		}
		AimDownSights ();
        if (continuity == false && Input.GetButtonDown("Fire1") || continuity == true && Input.GetButton("Fire1"))
        {
            if (currentBullets > 0)
            {
                Fire();
                if (currentBullets <= 0)
                {
                    DoReload();
                }
            }
            else
            {
                DoReload();
            }
        }
        RecoilBack ();
		gunName.text = weaponName;
	}

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.CrossFadeInFixedTime("Draw", 0.01f);
        audioSource.clip = drawSound;
        audioSource.time = drawSoundTime;
        audioSource.Play();
        bulletsCurrentText.text = currentBullets + " ";
        bulletsTotalText.text = " / " + bulletsTotal / bulletsPerMag;
    }

    private void Fire(){
        if (fireTimer < fireRate || isReloading || isRunning)
        {
            return;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, firePosition, Time.deltaTime * 8f);
        Debug.Log("Fired!");
        RaycastHit hit;
        
        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range)){
            GameObject hitSpark = Instantiate(hitSparkPrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
			Destroy (hitSpark, 0.2f); // Destroying automatically
			GameObject hitHole = Instantiate(hitHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
			Destroy (hitHole, 0.2f); // Destroying automatically

			muzzleFlash.Play();

            HealthManager healthManager = hit.transform.GetComponent<HealthManager>();
            Rigidbody rigidbody = hit.transform.GetComponent<Rigidbody>();
            if (healthManager)
            {
                Debug.Log((int)(hit.distance) + "M");
                if ((int)(hit.distance) >= Dstart)
                {
                    healthManager.ApplyDamage((damage - (int)(hit.distance / Dpower)));
                    Debug.Log(damage - (int)(hit.distance / Dpower ) + " Damage");
                }
                else
                {
                    healthManager.ApplyDamage(damage);
                    Debug.Log(damage + " Damage");
                }


            }
            if (rigidbody)
            {
                rigidbody.AddForceAtPosition(transform.forward * 2f * damage, transform.position);
            }
        
		}


		
		BulletEffect ();
		currentBullets--;
		fireTimer = 0.0f;
		anim.CrossFadeInFixedTime ("Fire", 0.01f); // fire animation
		audioSource.pitch = 1f;
		audioSource.PlayOneShot (shootSound); // shoot sound
        bulletsCurrentText.text = currentBullets + "";
        bulletsTotalText.text = " / " + bulletsTotal / bulletsPerMag;
        Recoil ();
        
    }

    private void Run()
    {
        //anim.CrossFadeInFixedTime("Run", 0.01f);
        anim.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));
        isRunning = characterController.velocity.sqrMagnitude > 40 ? true : false;
        anim.SetFloat("Speed", characterController.velocity.sqrMagnitude);
    }


    private void BulletEffect(){
		Quaternion randomQuaternion = new Quaternion (Random.Range (0, 360f), Random.Range (0, 360f), Random.Range (0, 360f), 1);
		GameObject casing = Instantiate (bulletCasing, bulletCasingPoint);
		casing.transform.localRotation = randomQuaternion;
		casing.GetComponent<Rigidbody> ().AddRelativeForce (new Vector3 (Random.Range (50f, 100f), Random.Range (50f, 100f), Random.Range (-30f, 30f)));
		Destroy (casing, 50f);
	}

	private void DoReload(){
		if (!isReloading && currentBullets < bulletsPerMag && bulletsTotal > 0) {
			anim.CrossFadeInFixedTime ("Reload", 0.01f); // Reloading
			audioSource.PlayOneShot(reloadSound);
			audioSource.pitch = 1.5f;
		}
	}

	public void Reload(){
        //GameObject.Find("FPSController").GetComponent<FirstPersonController>().ga= 2f;
        //FPSController.FixedUpdate.speed = 2f;
        int bulletsToReload = bulletsPerMag - currentBullets;
		if (bulletsToReload > bulletsTotal) {
			bulletsToReload = bulletsTotal;
		}
		currentBullets = bulletsPerMag;
		bulletsTotal -= bulletsPerMag;
        bulletsCurrentText.text = currentBullets + "";
        bulletsTotalText.text = " / " + bulletsTotal / bulletsPerMag;
    }

	private void AimDownSights(){
		if (Input.GetButton ("Fire2") && !isReloading) {
            anim.CrossFadeInFixedTime("no_ani", 0.01f);
            anim.SetBool("isRunning", !Input.GetButton("Fire2"));
            transform.localPosition = Vector3.Lerp (transform.localPosition, aimPosition, Time.deltaTime * 8f);
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, 40f, Time.deltaTime * 8f);
			isAiming = true;
			aimImage.enabled = false;
			accuracy = originalAccuracy / 2f;
			recoilAmount = originalRecoil / 2f;
		} 
		else
        {
            //anim.CrossFadeInFixedTime("idle", 0.01f);
            anim.enabled = true;
            transform.localPosition = Vector3.Lerp (transform.localPosition, originalPosition, Time.deltaTime * 5f);
			Camera.main.fieldOfView = Mathf.Lerp (Camera.main.fieldOfView, 70f, Time.deltaTime * 8f);
			isAiming = false;
			aimImage.enabled = true;
			accuracy = originalAccuracy;
			recoilAmount = originalRecoil;
		}
	}

	private void Recoil(){
		Vector3 recoilVector = new Vector3 (Random.Range (-recoilKickback.x, recoilKickback.x), recoilKickback.y, recoilKickback.z);
		Vector3 recoilCamVector = new Vector3 (-recoilVector.y * 400f, recoilVector.x * 200f, 0);

		transform.localPosition = Vector3.Lerp (transform.localPosition, transform.localPosition + recoilVector, recoilAmount / 2f); // position recoil
		camRecoil.localRotation = Quaternion.Slerp(camRecoil.localRotation, Quaternion.Euler(camRecoil.localEulerAngles + recoilCamVector), recoilAmount); // cam recoil
	}

	private void RecoilBack(){
		camRecoil.localRotation = Quaternion.Slerp (camRecoil.localRotation, Quaternion.identity, Time.deltaTime * 2f);
	}

}