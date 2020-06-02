using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  //場景管理 API

public class GameManager : MonoBehaviour
{
    #region 欄位

    
    [Header("道具")]
    public GameObject[] porps;
    [Header("文字介面：道具總數")]
    public Text textCount;
    [Header("文字介面：倒數時間")]
    public Text textTime;
    [Header("文字介面：結束畫面標題")]
    public Text textTitle;
    [Header("結束畫面")]
    public CanvasGroup final;

    /// <summary>
    /// 道具總數
    /// </summary>
    private int countTotal;

    /// <summary>
    /// 取得道具數量
    /// </summary>
    private int countProp;



    private float gameTime = 30;

    #endregion

    #region 方法
    /// <summary>
    /// 生成道具
    /// </summary>
    /// <param name="prop">想要生成的道具</param>
    /// <param name="count">想要生成的數量+隨機值+ -5</param>
    /// <returns></returns>
    private int CreatProp(GameObject prop, int count) 
    {
        //取得隨機道具數量 = 指定的數量 + - 5
        int total = count + Random.Range(-5, 5);

        //for 迴圈
        for (int i = 0; i < total; i++)
        {
            //座標 = (隨機,1,隨機)
            Vector3 pos = new Vector3(Random.Range(-9, 9), 1f, Random.Range(-9, 9));
            //生成 (物件,座標,角度)
            Instantiate(prop, pos, Quaternion.identity);
        }
        //傳回 道具數量
        return total;
    }


    private void CountTime()
    {


        if (countProp == countTotal) return;
        
        //遊戲時間  遞減 一禎的時間
        gameTime -= Time.deltaTime;

        //遊戲時間 = 數學.鉗住(遊戲時間,最小值,最大值)
        gameTime = Mathf.Clamp(gameTime, 0, 100);

        //更新倒數時間介面 ToString("f小數點位置")
        textTime.text = "倒數時間：" + gameTime.ToString("f2");
        Lose();
    }

    /// <summary>
    /// 取得道具：雞腿　更新數量與介面、高粱  扣兩秒並更新介面
    /// </summary>
    /// <param name="prop"></param>
    public void GetProp(string prop)
    {
        if (prop == "壽司")
        {
            countProp++;
            textCount.text = "道具數量：" + countProp + " / " + countTotal;

            Win();   //呼叫勝利方法

        }
        else if (prop == "臘腸")
           
        {
                gameTime -= 2;
                textTime.text = "倒數時間：" + gameTime.ToString("f2");  
        }
    }

    /// <summary>
    /// 勝利：吃光所有雞腿
    /// </summary>
    private void Win()
    {
        if (countProp == countTotal)            //如果壽司數量 = 壽司總數
        {
            final.alpha = 1;                    //顯示結束畫面、啟動互動、啟動遮擋
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "過關";             //更新結束畫面標題
        }
    }
    /// <summary>
    /// 失敗：時間為零
    /// </summary>
    private void Lose()
    {
        if (gameTime == 0)            
        {
            final.alpha = 1;                    
            final.interactable = true;
            final.blocksRaycasts = true;
            textTitle.text = "失敗";             
            FindObjectOfType<Player>().enabled = false;     //取得 玩家.啟動 = false
        }
    }


    public void Replay()
    {
        SceneManager.LoadScene("遊戲場景");
    }

    public void Quit()
    {
        Application.Quit();  //應用程式.離開()
    }



    #endregion


    #region 事件

    private void Start()
    {
        //道具總數 = 生成道具(道具一號,指定數量)
        countTotal = CreatProp(porps[0],10);

        textCount.text = "道具數量：0 / " + countTotal;

        //生成道具(道具二號,指定數量)
        CreatProp(porps[2], 5);         
    }


    private void Update()
    {
        CountTime();
    }
    #endregion
}
