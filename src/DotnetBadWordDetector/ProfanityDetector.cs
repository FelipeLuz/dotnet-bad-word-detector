using Microsoft.ML;
using DotnetBadWordDetector.Model;

namespace DotnetBadWordDetector;
public class ProfanityDetector
{
    private const string MODELPATH_ENGLISH = "DotnetBadWordDetector.Data.bad-words-model-english.zip";
    private const string MODELPATH_SPANISH = "DotnetBadWordDetector.Data.bad-words-model-spanish.zip";
    private PredictionEngine<BadWord, BadWordPrediction> _predictionEngine;
    
    public ProfanityDetector(Language language = Language.english)
    {
        LoadTrainedModel(language);
    }

    private void LoadTrainedModel(Language language)
    {   
        var mlContext = new MLContext();
        var stream = GetModelStream(language);
        var trainedModel = mlContext.Model.Load(stream, out _);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<BadWord, BadWordPrediction>(trainedModel);
    }

    private Stream GetModelStream(Language language)
    {
        var assembly = typeof(ProfanityDetector).Assembly;
        var path = language == Language.english ? MODELPATH_ENGLISH : MODELPATH_SPANISH;
        return assembly.GetManifestResourceStream(path);
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