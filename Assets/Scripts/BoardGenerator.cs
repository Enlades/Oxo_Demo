using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BoardGenerator : MonoBehaviour
{
    public GameObject GamePanel;
    public BoardSlot SlotPrefab;

    public GameObject GameCanvasGO;

    private RectTransform _gamePanelRT;
    private GridLayoutGroup _gamePanelGridLayout;

    private Canvas _gameCanvas;

    private void Awake(){
        _gamePanelRT = GamePanel.GetComponent<RectTransform>();
        _gamePanelGridLayout = GamePanel.GetComponent<GridLayoutGroup>();
        _gameCanvas = GameCanvasGO.GetComponent<Canvas>();
    }

    public BoardSlot[,] BuildBoard(int p_size , Action<int,int> p_callback){
        BoardSlot[,] result = new BoardSlot[p_size, p_size];

        Rect gamePanelRect = RectTransformUtility.PixelAdjustRect(_gamePanelRT, _gameCanvas);
       
        float panelWidth =  gamePanelRect.width;
        float panelHeight =  gamePanelRect.height;

        _gamePanelGridLayout.cellSize = new Vector2(panelWidth / p_size, panelHeight / p_size);

        for(int i = 0; i < p_size; i++){
            for(int j = 0; j < p_size; j++){
                BoardSlot newSlot = Instantiate(SlotPrefab);
                newSlot.Init(new Vector2Int(i,j), p_callback);
                newSlot.transform.SetParent(GamePanel.transform, false);
                newSlot.gameObject.SetActive(true);
                
                result[i,j] = newSlot;
            }
        }

        return result;
    }
}
