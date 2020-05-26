using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    #region 欄位與屬性
    /// <summary>
    /// 玩家變形元件
    /// </summary>
    private Transform player;

    [Header("追蹤速度"), Range(0.1f, 50.5f)]
    public float speed = 1.5f;

    #endregion

    #region 方法
    /// <summary>
    /// 追蹤玩家
    /// </summary>
    private void Track() 
    {
        //觀察攝影機與小明 Y 軸之距離 2.5-1 = 1.5
        //觀察攝影機與小明 Z 軸之距離 -3-0  = -3

        Vector3 posTrack = player.position;
        posTrack.y +=  1.5f;
        posTrack.z += -2.5f;
        

        //攝影機座標 = 變形.座標
        Vector3 posCam = transform.position;
        //攝影機座標 = 三圍向量.差值(A點,B點,百分比)
        posCam = Vector3.Lerp(posCam, posTrack, 0.3f * Time.deltaTime * speed);
        //變形.座標 = 攝影機座標
        transform.position = posCam;
        
    }

    #endregion

    #region 事件
    /* 測試 Lerp 差值 變化  
    public float A = 0;
    public float B = 1000;

    private void Update()
    {
        A = Mathf.Lerp(A, B, 0.1f);
    }
    */


    private void Start()
    {
        //小明物件 = 遊戲物件.尋找("物件名稱").變形
        player = GameObject.Find("小明").transform;
    }

    //延遲更新：會在 Update 執行後 再執行
    //建議：需要追蹤座標要寫在此事件內
    private void LateUpdate()
    {
        Track();
    }

    #endregion

}
