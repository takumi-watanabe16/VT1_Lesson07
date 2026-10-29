using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region----変数--
    [Header("***移動処理***")]
    [SerializeField] private float _moveSpeed;//基本移動速度

    [Header("***カメラ設定***")]
    [SerializeField] private CameraController _cameraController;

    [Header("***PlayerInput***")]
    [SerializeField]private PlayerInput _playerInput;

    //===Private変換
    private Vector3 _velo;//移動ベクトル
    private Vector3 _inputMoveVector;//移動用の入力値
    #endregion

    void Start()
    {
        MoveInputEnabul();//移動入力の有効化
    }

    void Update()
    {
        Movement(); //移動処理
    }

    /// <summary>
    /// 移動メソッド
    /// </summary>
    public void Movement()
    {
        //入力値を実際の移動平面に変換
        var inputAxis = new Vector3(_inputMoveVector.x,
            0, 
            _inputMoveVector.y);

        

        //カメラの向いている方向を移動ベクトルとする
        //1.カメラの正面ベクトルを取得
        var cameraForward = _cameraController.transform.forward;

        //2.入力値と合わせる
        var moveVector = (inputAxis.x * _cameraController.transform.right) + (inputAxis.z * cameraForward);
        moveVector.y = 0;


        //仮　入力をそのまま移動ベクトルへ...
        _velo = moveVector.normalized;

        Vector3 finalVelo = _moveSpeed * _velo * Time.deltaTime;
        transform.Translate(finalVelo,Space.World);
    }

    /// <summary>
    /// 移動入力の有効化
    /// </summary>
    public void MoveInputEnabul()
    {
        if(_playerInput == null)
        {
            Debug.Log("PlayerInputが設定されていません");
            return;
        }

        //移動入力中の処理
        _playerInput.actions["Move"].performed += OnMoveInput;
        _playerInput.actions["Move"].performed += OnMoveCanceled;

        //アクションの有効化
        _playerInput.actions["Move"].Enable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        var val = context.ReadValue<Vector2>();
        _inputMoveVector = val;
    }
    
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputMoveVector = Vector3.zero;//変数の値をリセット
    }
}

//note
/*3D操作
 カメラとプレイヤーは依存関係にある
プレイヤーはかっめらの向きがわからないと動くことができないし、カメラもプレイヤーの位置がわからないといけない

PlayerInputどっちにつけるか
PlayerInputは原則１つしか使えない
変数で実装できていればバラバラでも

正規化
nomalize
(0.1f,0,5f,0.7f) (Vector)
↓
(1,1,1) (Vector)
正の数であれば全て１にする　0.1も100も
*/


