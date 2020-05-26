using UnityEngine;

public class Player : MonoBehaviour
{
    #region 欄位與屬性
    [Header("移動速度"), Range(1, 1000)]
    public float speed = 10;
    [Header("跳躍高度"), Range(1, 5000)]
    public float height;

    /// <summary>
    /// 是否在地板上：如果 Y 軸小於 0.051 代表在地板上 否則 在空中
    /// </summary>
    private bool isGround
    {
        get
        {
            if (transform.position.y < 0.051f) return true; // 如果 Y 軸 小於 0.051 傳回 true
            else return false;                              // 否則 傳回 false
        }
    }

    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 angle;

    private Animator ani;
    private Rigidbody rig;

    /// <summary>
    /// 跳躍力道：從 0 慢慢增加
    /// </summary>
    private float jump;
    #endregion

    #region 方法
    /// <summary>
    /// 移動：透過鍵盤
    /// </summary>
    private void Move()
    {
          #region 移動
        // 浮點數 前後值 = 輸入類別.取得軸向值("垂直") - 垂直 WS 上下
        float v = Input.GetAxisRaw("Vertical");
        // 水平 AD 左右
        float h = Input.GetAxisRaw("Horizontal");

        // 剛體.添加推力(x, y, z) - 世界座標
        // rig.AddForce(0, 0, speed * v);
        // 剛體.添加推力(三維向量)
        // 前方 transform.forward - Z
        // 右方 transform.right   - X
        // 上方 transform.up      - Y
        rig.AddForce(transform.forward * speed * Mathf.Abs(v));
        rig.AddForce(transform.forward * speed * Mathf.Abs(h));

        // 動畫.設定布林值("跑步參數"，布林值) - 當 前後取絕對值 大於 0 時勾選
        ani.SetBool("跑步開關", Mathf.Abs(v) > 0 || Mathf.Abs(h) > 0);
        //ani.SetBool("跑步開關", v == 1 || v == -1); // 使用邏輯運算子
        #endregion

        #region 轉向
        if (v == 1) angle = new Vector3(0, 0, 0);               // 前 Y 0
        else if (v == -1) angle = new Vector3(0, 180, 0);       // 後 Y 180
        else if (h == 1) angle = new Vector3(0, 90, 0);         // 右 Y 90
        else if (h == -1) angle = new Vector3(0, 270, 0);       // 左 Y 270
        // 只要類別後面有 : MonoBehaviour
        // 就可以直接使用關鍵字 transform 取得此物件的 Transform 元件
        // eulerAngles 歐拉角度 0 - 360
        transform.eulerAngles = angle;
        #endregion
    }

    /// <summary>
    /// 跳躍：判斷在地板上並按下空白鍵時跳躍
    /// </summary>
    private void Jump()
    {
        // 如果 在地板上 為 勾選 並且 按下空白鍵
        if (isGround && Input.GetButtonDown("Jump"))
        {
            // 每次跳躍 值都從 0 開始
            jump = 0;
            // 剛體.推力(0，跳躍高度，0)
            rig.AddForce(0, height, 0);
        }
        // 如果 不在地板上(在空中)
        if (!isGround)
        {
            // 跳躍 遞增 時間.一禎時間
            jump += Time.deltaTime;
        }
        // 動畫.設定浮點數("跳躍參數"，跳躍時間)
        ani.SetFloat("跳躍力道", jump);
    }

    /// <summary>
    /// 碰到道具：碰到帶有標籤【雞腿】的物件
    /// </summary>
    private void HitProp()
    {

    }
    #endregion

    #region 事件
    private void Start()
    {
        // GetComponent<泛型>() 泛型方法 - 泛型 所有類型 Rigidbody, Transform, Collider..
        // 剛體 = 取得元件<剛體>();
        rig = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
    }

    // 固定更新頻率事件：1 秒 50 禎，使用物理必須在此事件內
    private void FixedUpdate()
    {
        Move();
    }

    // 更新事件：1 秒約 60 禎
    private void Update()
    {
        Jump();
    }
    #endregion
}
