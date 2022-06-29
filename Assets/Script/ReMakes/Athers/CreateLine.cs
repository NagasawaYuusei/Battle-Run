using UnityEngine;

public class CreateLine
{
    Transform _myTrans;
    Transform _targetTrans;

    GameObject _line;

    /// <summary>
    /// 疑似的にLineを引く際の二点間の設定
    /// </summary>
    /// <param name="my">自身</param>
    /// <param name="target">対象になるObgect</param>
    /// <returns></returns>
    public CreateLine Set(Transform my, Transform target)
    {
        _myTrans = my;
        _targetTrans = target;
        return this;
    }

    /// <summary>
    /// 疑似的にLineを引く際のObjectの生成
    /// </summary>
    /// <param name="size">ObjectのSize</param>
    /// <param name="type">Objectの形</param>
    /// <returns></returns>
    public CreateLine Create(float size, PrimitiveType type)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.GetComponent<Collider>().enabled = false;
        obj.transform.localScale = Vector3.one * size;
        obj.name = $"Line. Type:{type}";
        _line = obj;

        return this;
    }

    /// <summary>
    /// 生成されたLineを動的に動かす。
    /// </summary>
    public void UpDateLine()
    {
        if (_myTrans == null || _targetTrans == null)
        {
            Debug.Log($"NothingData. My {_myTrans}, Target {_targetTrans}");
            return;
        }

        if (_line == null)
        {
            Debug.Log($"NotCreate yet. {_line}");
            return;
        }

        Vector3 midpoin = (_myTrans.position + _targetTrans.position) / 2;
        _line.transform.position = midpoin;

        Vector3 dir = _myTrans.position - _targetTrans.position;
        _line.transform.localRotation = Quaternion.LookRotation(dir);

        float dist = Vector3.Distance(_myTrans.position, _targetTrans.position);
        Vector3 scale = _line.transform.localScale;
        scale.z = dist;
        _line.transform.localScale = scale;
    }

    public void Destroy()
    {
        Object.Destroy(_line);
        _myTrans = null;
        _targetTrans = null;
    }
}
