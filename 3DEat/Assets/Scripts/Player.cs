using System;
using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
    #region 欄位與屬性
    [Header("移動速度"), Range(1, 1000)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float height;

    /// <summary>
    ///是否在地上
    /// </summary>
    private bool isGround 
    {
        get 
        {
            //如果 Y 軸 小於 1f 傳回 trun
            if (transform.position.y < 1f) return true;
            //否 則傳回 false 
            else return false;
        }
    }
    
    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 angle;

    //動作
    //剛體
    private Animator ani;
    private Rigidbody rig;


    /// <summary>
    /// 跳躍力道：從0慢慢增加
    /// </summary>
    private float jump;
    #endregion
    
    #region 方法
    ///<summary>
    ///移動:透過鍵盤
    ///</summary>
    private void Move()
    {
        #region 移動
        //浮點數 前後左右值 = 輸入類別.取得軸向("垂直") - 垂直WS 上下 水平AD 左右
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        //剛體.添加外力(x,y,z) - 世界座標
        //rig.AddForce(0, 0 ,speed*v);
        //前方 transform.forward - Z
        //前方 transform.right   - X
        //前方 transform.up      - Y
        rig.AddForce(transform.forward * speed * Math.Abs(v));
        rig.AddForce(transform.forward * speed * Math.Abs(h));

        //動畫.設定布林("跑步參數",布林值) - 當前後 || 左右 取絕對值大於0 時勾選
        ani.SetBool("跑步開關", Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0);

        //邏輯運算子
        //ani.SetBool("跑步開關", v == 1 || v == -1);
        #endregion

        #region 轉向
       
        if (v == 1) angle = new Vector3(0, 0, 0);           //前 Y 0
        else if(v == -1) angle = new Vector3(0, 180 , 0);    //後 Y 180
        else if (h == 1) angle = new Vector3(0, 90 , 0);     //右 Y 90
        else if (h == -1) angle = new Vector3(0, 270 , 0);   //左 Y 270

        //只要類別後面有：MonoBehaviour
        //就可以直接使用關鍵字 transform 取得此物件的 Transform 元件
        //eulerAngles 歐拉角度 0 - 360
        transform.eulerAngles = angle;

        #endregion
    }

    ///<summary>
    ///跳躍：判斷在地板上並按下空白鍵時跳躍
    ///</summary>
    private void Jump()
    {
        //如果 在地板上 為 勾選 並且 按下空白鍵
        if (isGround && Input.GetButtonDown("Jump"))
        {
            jump = 0;
            //剛體.外力(0,跳躍高度,0)
            rig.AddForce(0, height, 0);
        }
        //如果 不在地板上(在空中)
        if (!isGround) 
        {
            jump += Time.deltaTime*2;
        }
        ani.SetFloat("跳躍力道",jump);
    }

    ///<summary>
    ///碰到道具：碰到帶有標籤【漢堡】【壽司】的物件
    ///</summary>
    private void HitProp()
    {

    }

    #endregion


    #region 事件

    private void Start()
    {
        //GetComponent<泛型>()  泛型方法 - 泛型 所有類別 Rigidbody, Transfrom, Collider..
        //剛體 = 取得元件<剛體>()
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move() ;
    }


    private void Update()
    {

        Jump() ;
        
    }
    #endregion



}
