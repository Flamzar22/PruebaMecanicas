using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;
    public enum CameraStyle
    {
        Basic,
        Combat,
        TopDown
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchCameraStyle(CameraStyle.Basic); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchCameraStyle(CameraStyle.Combat); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SwitchCameraStyle(CameraStyle.TopDown); }


        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;
        
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.TopDown)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 combatLookAtDir = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = combatLookAtDir.normalized;

            playerObj.forward = combatLookAtDir.normalized;
        }
    }
    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) { thirdPersonCam.SetActive(true); }
        if (newStyle == CameraStyle.Combat) { combatCam.SetActive(true); }
        if (newStyle == CameraStyle.TopDown) { topDownCam.SetActive(true); }

        currentStyle = newStyle;
    }
}
