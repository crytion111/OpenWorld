using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainPlayerControlChara : MonoBehaviour
{

    public Transform headTrans;
    public Transform playerTrans;
    public float mouseSensi = 5.0f;
    private Vector3 mouseMoveVec;
    public Vector2 headMoveLimit;

    public CharacterController playerController;


    public float m_movSpeed = 2.0f;      //�ƶ��ٶ�

    public float m_jumphight = 1.2f;       //��Ծ�߶�

    public float m_gravity = 9.8f;       //�������ٶ�

    private Vector3 Velocity = Vector3.zero;     //��ֱ�����ϵ�һ������


    public float margin;

    public RaycastHit hitInfo;

    public LayerMask mainPlayerMask;



    // Start is called before the first frame update
    void Start()
    {
        playerTrans = this.transform;
        playerController = this.GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �ӽ�
        var temp_mouseX = Input.GetAxis("Mouse X");
        var temp_mouseY = Input.GetAxis("Mouse Y");
        mouseMoveVec.y += temp_mouseX * mouseSensi;
        mouseMoveVec.x -= temp_mouseY * mouseSensi;
        mouseMoveVec.x = Mathf.Clamp(mouseMoveVec.x, headMoveLimit.x, headMoveLimit.y);
        playerTrans.rotation = Quaternion.Euler(0, mouseMoveVec.y, 0);
        headTrans.rotation = Quaternion.Euler(mouseMoveVec.x, mouseMoveVec.y, 0);
    }

    bool checkGround()
    {
        //���߼��ŵ���û�е���
        var myray = new Ray(playerTrans.position, playerTrans .TransformDirection(-Vector3.up));
        RaycastHit hit;
        Physics.Raycast(myray, out hit);

        return hit.distance <= margin || playerController.isGrounded;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (checkGround() && Input.GetKeyDown(KeyCode.Space))
        {
            Velocity.y += Mathf.Sqrt(m_jumphight * m_gravity);
        }

        Velocity.y -= m_gravity * Time.deltaTime;    //�������ٶ� a += g*ʱ��

        if (checkGround() && Velocity.y < 0)
        {
            Velocity.y = -0.01f;
        }

        //��������
        var vertical = Input.GetAxis("Vertical");  //����ws
        var horizontal = Input.GetAxis("Horizontal"); //����ad Horizontal


        // Velocity.yΪ�����Զ��������Ծ���ϸ�,  horizontal��verticalΪǰ�������ƶ�
        var moveVec = new Vector3(horizontal, Velocity.y, vertical);
        moveVec = playerTrans.TransformDirection(moveVec);
        moveVec *= m_movSpeed;

        playerController.Move(moveVec * Time.deltaTime);
    }
}
