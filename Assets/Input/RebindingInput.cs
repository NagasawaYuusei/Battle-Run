using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingInput : MonoBehaviour
{
    [Tooltip("PlayerInputコンポーネントがあるゲームオブジェクト"), SerializeField] PlayerInput _pInput;

    [Tooltip("リバインディング中のメッセージ表示テキスト。アクティブ状態の可否に使用。"), SerializeField] GameObject _rebindingMessage;
    [Tooltip("リバインディングを開始するボタン。アクティブ状態の可否に使用。"), SerializeField] GameObject _rebindingButton;
    [Tooltip("リバインディング開始ボタンのテキスト。キー名を表示。"), SerializeField] Text _bindingName;

    [Tooltip("リバインディングしたいInputAction項目。今回はMap:PlayerのAction:Fireを使用しています。"), SerializeField] InputActionReference _action;
    [Tooltip("リバインディングしたいコントロールのインデックス")]string rebindingIndex;

    InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
    public void Start()
    {
        //アクション(Fire)ボタンを押下するとデバッグにFireと表示
        _pInput.actions["Fire"].performed += _ => Debug.Log("Fire");

        //すでにリバインディングしたことがある場合はシーン読み込み時に変更。
        string rebinds = PlayerPrefs.GetString("rebindSample");

        if (!string.IsNullOrEmpty(rebinds))
        {
            //リバインディング状態をロード
            _pInput.actions.LoadBindingOverridesFromJson(rebinds);

            //バインディング名を取得
            int bindingIndex = _action.action.GetBindingIndexForControl(_action.action.controls[0]);
            _bindingName.text = InputControlPath.ToHumanReadableString(
                _action.action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
        }


    }

    public void StartRebinding(string str)
    {
        //ボタンを消し、代わりにリバインディング中のメッセージを表示
        _rebindingButton.SetActive(false);
        _rebindingMessage.SetActive(true);

        //ボタン制御中の表示
        //ボタンの誤作動を防ぐため、何も無いアクションマップに切り替え
        _pInput.SwitchCurrentActionMap("Select");

        //Fireボタンのリバインディング開始
        _rebindingOperation = _action.action.PerformInteractiveRebinding()
            .WithTargetBinding(_action.action.GetBindingIndexForControl(_action.action.controls[0]))
            .WithControlsExcluding(str)
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    public void RebindComplete()
    {
        //fireアクションの1番目のコントロール(バインディングしたコントロール)のインデックスを取得
        int bindingIndex = _action.action.GetBindingIndexForControl(_action.action.controls[0]);

        //バインディングしたキーの名称を取得する
        _bindingName.text = InputControlPath.ToHumanReadableString(
            _action.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);

        _rebindingOperation.Dispose();

        //画面を通常に戻す
        _rebindingButton.SetActive(true);
        _rebindingMessage.SetActive(false);

        //リバインディング時は空のアクションマップだったので通常のアクションマップに切り替え
        _pInput.SwitchCurrentActionMap("Player");

        //リバインディングしたキーを保存(シーン開始時に読み込むため)
        PlayerPrefs.SetString("rebindSample", _pInput.actions.SaveBindingOverridesAsJson());
    }
}
