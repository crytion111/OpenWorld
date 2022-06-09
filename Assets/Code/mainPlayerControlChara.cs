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


    public float moveSpeed = 2.0f;      //移动速度
    public float speedUpRate = 2.7f;            //加速倍率
    public float defaultSpeedUpRate = 1.0f;    //默认一倍不加速

    public float jumphight = 1.2f;       //跳跃高度

    public float gravity = 9.8f;       //重力加速度

    private Vector3 Velocity = Vector3.zero;     //竖直方向上的一个向量


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
        // 视角
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
        //射线检测脚底有没有地面
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

        Velocity.y -= gravity * Time.deltaTime;    //重力加速度 a += g*时间

        if (checkGround() && Velocity.y < 0)
        {
            Velocity.y = 0;
        }

        float vertical = 0;
        float horizontal = 0;
        //控制主角
        vertical = Input.GetAxis("Vertical");  //键入ws
        horizontal = Input.GetAxis("Horizontal"); //键入ad Horizontal


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



        // Velocity.y为重力自动下落和跳跃的上浮,  horizontal和vertical为前后左右移动
        var moveVec = new Vector3(horizontal, Velocity.y, vertical);
        moveVec = playerTrans.TransformDirection(moveVec);
        moveVec *= (moveSpeed * defaultSpeedUpRate);

        playerController.Move(moveVec * Time.deltaTime);
    }
}
