using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorChosen
{
    public player_movement Player;
    public int PlayerColor;
}

public class colorUI : NetworkBehaviour
{
    public List<Color> ColorChoices;
    readonly public SyncList<ColorChosen> ColorSelected = new SyncList<ColorChosen>();
    public Transform HorizontalLayout;
    public Button ColorPrefab;
    private List<Button> _buttons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        ColorSelected.Callback += OnColorSelectedChanged;

        foreach (Color color in ColorChoices)
        {
            Button ColorBtn = Instantiate(ColorPrefab, HorizontalLayout);
            ColorBtn.image.color = color;
            ColorBtn.onClick.AddListener(() => SelectColor(color));
            _buttons.Add(ColorBtn);
        }
    }

    private void OnColorSelectedChanged(SyncList<ColorChosen>.Operation op, int itemIndex, ColorChosen oldItem, ColorChosen newItem)
    {
        _buttons.ForEach(x => x.interactable = true);
        foreach(ColorChosen chose in ColorSelected)
        {
            _buttons[chose.PlayerColor].interactable = false;
        }
    }
    
    private void SelectColor(Color color)
    {
        print("test");
        player_movement player = player_movement.Local;
        int colorIndex = ColorChoices.FindIndex(x => x == color);
        CmdAskForColor(colorIndex, player);
    }

    [Command(requiresAuthority = false)]
    public void CmdAskForColor(int color, player_movement player)
    {
        int colorIndex = ColorSelected.FindIndex(x => x.Player == player);
        if(colorIndex > -1)
        {
            ColorSelected.RemoveAt(colorIndex);
        }

        ColorChosen entry = new ColorChosen
        {
            Player = player,
            PlayerColor = color
        };
        ColorSelected.Add(entry);

        player.SetColor(ColorChoices[color]);
    }
    void Update()
    {
    }
}
