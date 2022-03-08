using Microsoft.ML.Data;
namespace DotnetBadWordDetector.Model;
public class BadWord
{
    public string Word { get; set; }
}
public class BadWordPrediction : BadWord
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }
}