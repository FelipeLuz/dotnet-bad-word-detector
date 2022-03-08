using Microsoft.ML.Data;

namespace DotnetBadWordDetector.Model
{
    public class BadWord
    {
        [LoadColumn(0)]
        public string Word;

        [LoadColumn(1), ColumnName("Label")]
        public bool Value;
    }

    public class BadWordPrediction : BadWord
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}