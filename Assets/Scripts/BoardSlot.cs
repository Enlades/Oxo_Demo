using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardSlot : MonoBehaviour
{
    public Vector2Int BoardPosition;
    public bool Marked{ get; private set;}
    private GameObject _xGO;

    private Action<int,int> _clickCallback;

    private void Awake(){
        _xGO = transform.Find("X").gameObject;

        Marked = false;
    }

    public void Init(Vector2Int p_gridPosition, Action<int,int> p_callback){
        name = "Slot_" + p_gridPosition.x + "_" + p_gridPosition.y;
        BoardPosition = p_gridPosition;
        _clickCallback = p_callback;
    }

    public void MarkButton(){
        if(Marked){
            return;
        }

        MarkSlot();

        _clickCallback.Invoke(BoardPosition.x, BoardPosition.y);
    }

    public void Reset(){
        DeMarkSlot();
    }

    private void MarkSlot(){
        Marked = true;
        _xGO.SetActive(true);
    }

    private void DeMarkSlot(){
        Marked = false;
        _xGO.SetActive(false);
    }
}
