using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sliders used to modify the width and height of the hex map
/// </summary>
public class MazeSizeSliders : MonoBehaviour
{
    [Tooltip("If false it will modify the columns")]
    [SerializeField] private bool isRow;
    private MazeGenerator mazeGenerator;
    [Tooltip("The limit of how large and small the maze can be")]
    [SerializeField] private int minCount=10, maxCount = 250;

    private Slider slider;
    private Text textDisplay;
    private void Start()
    {
        mazeGenerator = FindObjectOfType<MazeGenerator>();
        textDisplay = GetComponentInChildren<Text>();

        slider = GetComponent<Slider>();
        slider.maxValue = maxCount;
        slider.minValue = minCount;

        int sliderVal = Mathf.RoundToInt(slider.value);
        if (isRow)
        {
            slider.value = mazeGenerator.mazeRows;
            textDisplay.text = "Maze Rows: " + sliderVal;
        }
        else
        {
            slider.value = mazeGenerator.mazeColumns;
            textDisplay.text = "Maze Columns: " + sliderVal;
        }
    }

    public void Update()
    {
        int sliderVal = Mathf.RoundToInt(slider.value);
        if (isRow)
        {
            textDisplay.text = "Maze Rows: " + sliderVal;
            mazeGenerator.mazeRows = sliderVal;
        }
        else
        {
            textDisplay.text = "Maze Columns: " + sliderVal;
            mazeGenerator.mazeColumns = sliderVal;
        }
    }
}
