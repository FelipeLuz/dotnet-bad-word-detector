using Microsoft.ML;
using DotnetBadWordDetector.Model;

namespace DotnetBadWordDetector;
public class ProfanityDetector
{
    
    private const string MODELPATH = "DotnetBadWordDetector.Data.bad-words-model-english.zip";
    private PredictionEngine<BadWord, BadWordPrediction> _predictionEngine;
    
    public ProfanityDetector()
    {
        LoadTrainedModel();
    }

    private void LoadTrainedModel()
    {   
        DataViewSchema modelSchema;
        var mlContext = new MLContext();
        var trainedModel = mlContext.Model.Load(GetModelStream(), out modelSchema);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<BadWord, BadWordPrediction>(trainedModel);
    }

    private Stream GetModelStream()
    {
        var assembly = typeof(DotnetBadWordDetector.ProfanityDetector).Assembly;
        return assembly.GetManifestResourceStream(MODELPATH);
    }

    /// <summary>
    /// Predicts if the word or small sentence is profane
    /// </summary>
    /// <param name="word"></param>
    /// <returns>true if classified as profane</returns>
    public bool IsProfane(string word)
    {
        var obj = new BadWord { Word = word };
        return _predictionEngine.Predict(obj).Prediction;
    }

    /// <summary>
    /// Gets the probability of a given word or small sentence being profane
    /// </summary>
    /// <param name="word"></param>
    /// <returns> 0 < prediction < 1</returns>
    public float GetProfanityProbability(string word)
    {
        var obj = new BadWord { Word = word };
        return _predictionEngine.Predict(obj).Probability;
    }
}