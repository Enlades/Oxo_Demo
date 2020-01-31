using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Action<int,int> BoardSlotClick;
    public GameObject GridSizeInputGO, ScoreTextGO;
    private BoardGenerator _boardGenerator;
    private BoardSlot[,] _gameBoard;
    private InputField _gridSizeInputField;
    private Text _scoreText;
    private int _score;

    private int _boardWidth{get{return _gameBoard.GetLength(0);}}
    private int _boardHeight{get{return _gameBoard.GetLength(1);}}
    
    private bool _boardReady;
    private void Awake(){
        _boardGenerator = GetComponent<BoardGenerator>();
        _gridSizeInputField = GridSizeInputGO.GetComponent<InputField>();
        _scoreText = ScoreTextGO.GetComponent<Text>();

        _boardReady = false;
        _score = 0;

        BoardSlotClick += GameCheck;

        SetScoreText();
    }

    public void GameCheck(int p_x, int p_y){
        List<Vector2Int> connectedSlotPositons = new List<Vector2Int>();
        int totalConnection = CheckAroundSlot(p_x, p_y, p_x, p_y, connectedSlotPositons);

        Debug.Log("Total : " + totalConnection);
        
        if(totalConnection >= 3){
            for(int i = 0; i < connectedSlotPositons.Count; i++){
                _gameBoard[connectedSlotPositons[i].x, connectedSlotPositons[i].y].Reset();
            }

            _score++;
            SetScoreText();
        }
    }

    public void BuildButton(){
        if(_boardReady){
            DestroyBoard();
        }

        BuildBoard();

        _gridSizeInputField.text = "";
    }

    private int CheckAroundSlot(int p_x, int p_y, int p_pX, int p_pY, List<Vector2Int> p_connectedSlotPositions){
        int totalConnection = 1;
        p_connectedSlotPositions.Add(new Vector2Int(p_x, p_y));

        if(p_y + 1 < _boardHeight && p_y + 1 != p_pY && _gameBoard[p_x, p_y + 1].Marked){
            totalConnection += CheckAroundSlot(p_x, p_y + 1, p_x, p_y, p_connectedSlotPositions);
        }

        if(p_x + 1 < _boardHeight && p_x + 1 != p_pX && _gameBoard[p_x + 1, p_y].Marked){
            totalConnection += CheckAroundSlot(p_x + 1, p_y, p_x, p_y, p_connectedSlotPositions);
        }

        if(p_y - 1 >= 0 && p_y - 1 != p_pY && _gameBoard[p_x, p_y -1].Marked){
            totalConnection += CheckAroundSlot(p_x, p_y - 1, p_x, p_y, p_connectedSlotPositions);
        }

        if(p_x - 1 >= 0 && p_x - 1 != p_pX && _gameBoard[p_x - 1, p_y].Marked){
            totalConnection += CheckAroundSlot(p_x - 1, p_y, p_x, p_y, p_connectedSlotPositions);
        }

        return totalConnection;
    }

    private void BuildBoard(){
        int inputNumber = -1;
        inputNumber = int.Parse(_gridSizeInputField.text);

        if(inputNumber <= 1 || inputNumber >= 12){
            return;
        }

        if(_boardReady){
            DestroyBoard();
        }

        _gameBoard = _boardGenerator.BuildBoard(inputNumber, BoardSlotClick);

        _boardReady = true;
    }

    private void DestroyBoard(){
        for(int i = 0; i < _gameBoard.GetLength(0); i++){
            for(int j = 0; j < _gameBoard.GetLength(1); j++){
                Destroy(_gameBoard[i,j].gameObject);
            }
        }

        _boardReady = false;
    }

    private void SetScoreText(){
        _scoreText.text = "Score : " + _score;
    }
}
