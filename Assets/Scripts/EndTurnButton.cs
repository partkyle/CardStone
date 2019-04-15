using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : MonoBehaviour
{
    public Color playerColor;
    public Color opponentColor;

    Button button;
    Image image;

    private void Awake() {
        button = GetComponent<Button>();
        // this button is handled by an image
        image = GetComponent<Image>();
    }

    public void SetTurn(GameManager.Turn turn) {
        if (turn == GameManager.Turn.Player) {
            image.color = playerColor;
        } else {
            image.color = opponentColor;
        }
    }
}
