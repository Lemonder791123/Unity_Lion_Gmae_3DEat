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

    
    private Animator ani; //動作
    private Rigidbody rig; //剛體
    private AudioSource aud; //喇叭
    private GameManager gm; //遊戲管理者


    /// <summary>
    /// 跳躍力道：從0慢慢增加
    /// </summary>
    private float jump;

    [Header("壽司音效")]
    public AudioClip soundSu;
    public AudioClip soundDu;
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
    ///碰到道具：碰到帶有標籤【漢堡】【壽司】【臘腸】的物件
    ///</summary>
    private void HitProp(GameObject prop)
    {
        print("碰到的物件標籤為：" + prop.name);

        if (prop.tag == "壽司")
        {
            aud.PlayOneShot(soundSu, 2);    //喇叭.撥放一次音效(音效片段, 音量)
            Destroy(prop);                  //刪除(物件)
        }
        else if (prop.tag == "臘腸")
        {
            aud.PlayOneShot(soundDu, 2);
            Destroy(prop);
        }

        gm.GetProp(prop.tag);       //告知 GM 取得道具(將道具標籤傳過去)
    }

    #endregion


    #region 事件

    private void Start()
    {
        //GetComponent<泛型>()  泛型方法 - 泛型 所有類別 Rigidbody, Transfrom, Collider..
        //剛體 = 取得元件<剛體>()
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        aud = GetComponent<AudioSource>();

        //FOOT 僅限於場景上只有一個類別存在時使用
        //例如：場景上只有一個 GameManager 類別時可以使用他來取得;
        gm = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        Move() ;
    }


    private void Update()
    {

        Jump() ;
        
    }

    //碰撞事件：當物件碰撞開始時執行一次（沒有勾選　Is Trigger）
    //collision 碰到物件的碰撞資訊
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    //碰撞事件：當物件碰撞 "離開時" 執行一次（沒有勾選　Is Trigger）
    private void OnCollisionExit(Collision collision)
    {

    }
    //碰撞事件：當物件碰撞 "開始時" 持續執行（沒有勾選　Is Trigger）60FPS
    private void OnCollisionStay(Collision collision)
    {

    }
    //觸發事件：當物件碰撞 "開始時" 執行一次（有勾選　Is Trigger）
    private void OnTriggerEnter(Collider other)
    {
        //碰到道具(碰撞資訊.遊戲物件)
        HitProp(other.gameObject);
    }
    //觸發事件：當物件碰撞 "離開時" 執行一次（有勾選　Is Trigger）
    private void OnTriggerExit(Collider other)
    {
        
    }
    //觸發事件：當物件碰撞 "開始時" 持續執行（有勾選　Is Trigger）
    private void OnTriggerStay(Collider other)
    {
    
    }
    #endregion



}
