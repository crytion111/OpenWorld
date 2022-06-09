using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainPlayerControlChara : MonoBehaviour
{

    public Animator anim;


    public Transform headTrans;
    public Transform playerTrans;
    public float mouseSensi = 5.0f;
    private Vector3 mouseMoveVec;
    public Vector2 headMoveLimit;

    public CharacterController playerController;


    public float moveSpeed = 2.0f;      //�ƶ��ٶ�
    public float speedUpRate = 2.7f;            //���ٱ���
    public float defaultSpeedUpRate = 1.0f;    //Ĭ��һ��������

    public float jumphight = 1.2f;       //��Ծ�߶�

    public float gravity = 9.8f;       //�������ٶ�

    private Vector3 Velocity = Vector3.zero;     //��ֱ�����ϵ�һ������


    public float margin;

    public RaycastHit hitInfo;

    public LayerMask mainPlayerMask;



    // Start is called before the first frame update
    void Start()
    {
        playerTrans = this.transform;
        playerController = this.GetComponent<CharacterController>();
        anim = this.GetComponent<Animator>();
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
        var myray = new Ray(playerTrans.position, playerTrans.TransformDirection(-Vector3.up));
        RaycastHit hit;
        Physics.Raycast(myray, out hit);

        return hit.distance <= margin || playerController.isGrounded;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (checkGround() && Input.GetKeyDown(KeyCode.Space))
        {
            Velocity.y += Mathf.Sqrt(jumphight * gravity);
            anim.Play("jump");
        }

        Velocity.y -= gravity * Time.deltaTime;    //�������ٶ� a += g*ʱ��

        if (checkGround() && Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        float vertical = 0;
        float horizontal = 0;
        //��������
        vertical = Input.GetAxis("Vertical");  //����ws
        horizontal = Input.GetAxis("Horizontal"); //����ad Horizontal


        if (Input.GetKey(KeyCode.LeftShift))
        {
            defaultSpeedUpRate = speedUpRate;
        }
        else
        {
            defaultSpeedUpRate = 1;
        }

        if (vertical != 0 || horizontal != 0)
        {
            if (vertical > 0)
            {
                if (defaultSpeedUpRate > 1)
                {

                    anim.Play("runForward");
                }
                else
                {
                    anim.Play("walkForward");
                }
            }
            else
            {
                anim.Play("walkBack");
            }
        }
        else
        {
            anim.Play("Idle01");
        }



        // Velocity.yΪ�����Զ��������Ծ���ϸ�,  horizontal��verticalΪǰ�������ƶ�
        var moveVec = new Vector3(horizontal, Velocity.y, vertical);
        moveVec = playerTrans.TransformDirection(moveVec);
        moveVec *= (moveSpeed * defaultSpeedUpRate);

        playerController.Move(moveVec * Time.deltaTime);
    }
}
