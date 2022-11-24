using Newtonsoft.Json.Linq;
using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] Image healthbarImage;
    [SerializeField]  GameObject ui;
    [SerializeField] float mouseSensitivity, walkSpeed, jumpForce,smoothTime;
    public float playerSpeed = 5f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex=-1;
    float verticalLookRotation;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    PhotonView PV;
    const float maxHealth = 100f;
    float currenHealth = maxHealth;
    public GameObject ModoPan;
    public GameObject ModoMasa;
    public GameObject ModeloArma;
    public GameObject camara_masa;
    Vector3 position;
    public bool cambio=false;
    PlayerManager playerManager;
    public CapsuleCollider capsule;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public bool cambio_camara=true;
    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;
    private Vector3 movePlayer;
    bool bloqueomouse;
    [SerializeField] CanvasGroup canvasGroup;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        controller = gameObject.GetComponent<CharacterController>();

        playerManager =  PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        
    }
    private void Start()
    {
        if (PV.IsMine)
        {
            EquipItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(ui);
        }
    }
    private void Update()
    {

        if (!PV.IsMine)
        {
            return;
        }
      

        Look();
        camDirection();
        move();



      if (Input.GetKey(KeyCode.Escape))
        Cursor.lockState = CursorLockMode.None;

       if (Input.GetMouseButtonDown(0)&& !bloqueomouse)
         Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetMouseButtonDown(0)&&cambio_camara==true)
        {
            items[itemIndex].Use();
        }
       
        if (transform.position.y < -10f)
        {
            Die();
        }

        if (Input.GetKey(KeyCode.E))
        {
            cambio = true;
            playerSpeed = 30f;
            metodoCambio(cambio);
        }
        else
        {
           cambio= false;
            metodoCambio(cambio);

            playerSpeed = 20f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvasGroup.alpha = 1;
            bloqueomouse = true;
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            canvasGroup.alpha = 0;
            bloqueomouse= false;
        }

    }
  
    void Look()
    {
        if (!cambio_camara)
        {
          
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            camara_masa.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }
        else
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
            cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }
        
    }
    void move()
    {
     
       groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move =  new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
       
        moveAmount = Vector3.SmoothDamp(moveAmount, move * (Input.GetKey(KeyCode.LeftShift) ?   walkSpeed:playerSpeed), ref smoothMoveVelocity, smoothTime);
        movePlayer = moveAmount.x*camRight+moveAmount.z*camForward;
        controller.Move(movePlayer * Time.deltaTime );
        if (Input.GetButtonDown("Jump") && groundedPlayer && !cambio)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
     
        controller.Move(playerVelocity * Time.deltaTime);

    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward = camForward.normalized;

        camRight = camRight.normalized;
    }
  
    void EquipItem(int _index)
    {
        if(_index == previousItemIndex)
       
            return;
        
        itemIndex = _index;
       
        items[itemIndex].itemGameObject.SetActive(true);
        if (previousItemIndex!= -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }
        previousItemIndex = itemIndex;
        if (PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("intemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey("itemIndex") && !PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipItem((int)changedProps["itemIndex"]);
        }

    }

  

    public void TakeDamage(float damage)
    {
       
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }
    [PunRPC]
    void RPC_TakeDamage(float damage,PhotonMessageInfo info)
    {
     
        currenHealth -= damage;
        healthbarImage.fillAmount = currenHealth / maxHealth;
        if (currenHealth <=0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
       
    }

   public void metodoCambio(bool cambiar)
    {
       
        if (cambiar)
        {
            capsule.height = 0;
            capsule.radius = 0.95f;
            controller.radius = 0.95f;
            capsule.center = new Vector3(0f, 0.200000003f, 0f);
            controller.height = 0;
            controller.center = new Vector3(0f, 0.200000003f, 0f);
            ModoMasa.SetActive(true);
            ModoPan.SetActive(false);
            ModeloArma.SetActive(false);
            cameraHolder.SetActive(false);
            camara_masa.SetActive(true);
            cambio_camara = false;
            walkSpeed = 30f;


        }
        if (!cambiar)
        {
            capsule.height = 5.07f;
            capsule.radius = 2.03f;
            capsule.center = new Vector3(0f, 2.46000004f, 0f);
            controller.height = 5.07f;
            controller.radius = 2.03f;
            controller.center = new Vector3(0, 2.46000004f, 0);
            ModoMasa.SetActive(false);
            ModoPan.SetActive(true);
            ModeloArma.SetActive(true);
            cameraHolder.SetActive(true);
            camara_masa.SetActive(false);
            cambio_camara = true;
            walkSpeed =20f;

        }
        photonView.RPC(nameof(cambiarforma), RpcTarget.OthersBuffered, cambiar);



    }
    [PunRPC]
    void cambiarforma(bool cambia)
    {
        if (cambia)
        {
            capsule.height = 0;
            capsule.radius = 0.95f;
            controller.radius = 0.95f;
            capsule.center = new Vector3(0f, 0.200000003f, 0f);
            controller.height = 0;
            controller.center = new Vector3(0f, 0.200000003f, 0f);
            ModoMasa.SetActive(true);
            ModoPan.SetActive(false);
            ModeloArma.SetActive(false);
            walkSpeed = 30f;
        }
        if (!cambia)
        {
            capsule.height = 5.07f;
            capsule.radius = 2.03f;
            capsule.center = new Vector3(0f, 2.46000004f, 0f);
            controller.height = 5.07f;
            controller.radius = 2.03f;
            controller.center = new Vector3(0, 2.46000004f, 0);
            ModoMasa.SetActive(false);
            ModoPan.SetActive(true);
            ModeloArma.SetActive(true);
            walkSpeed = 20f;
        }
        
    }


    void Die()
    {
        playerManager.Die();
    }
}
