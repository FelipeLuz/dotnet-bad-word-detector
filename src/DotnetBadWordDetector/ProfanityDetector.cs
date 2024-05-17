using Microsoft.ML;
using DotnetBadWordDetector.Model;

namespace DotnetBadWordDetector;
public class ProfanityDetector
{
    const string MODEL_BASE_PATH = "DotnetBadWordDetector.Data.bad-words-model-{LOCALE}.zip";
    const string TAG = "{LOCALE}";
    List<PredictionEngine<BadWord, BadWordPrediction>> _engines;
    
    public ProfanityDetector(bool allLocales = false)
    {

        Locales[] locales = allLocales ? [Locales.ENGLISH, Locales.SPANISH, Locales.PORTUGUESE]
                                        : [Locales.ENGLISH];
        LoadTrainedModel(locales);
    }

    public ProfanityDetector(params Locales[] locales)
    {
        LoadTrainedModel(locales);
    }

    private void LoadTrainedModel(Locales[] locales)
    {   
        var mlContext = new MLContext();
        foreach (var locale in locales)
        {
            var path = MODEL_BASE_PATH.Replace(TAG, locale.GetDescription());
            var stream = GetModelStream(path);
            var trainedModel = mlContext.Model.Load(stream, out _);
            var engine = mlContext.Model.CreatePredictionEngine<BadWord, BadWordPrediction>(trainedModel);
            _engines.Add(engine);
        }
    }

    private Stream GetModelStream(string path)
    {
        var assembly = typeof(ProfanityDetector).Assembly;
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
        foreach (var engine in _engines)
        {
            if (!engine.Predict(obj).Prediction)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Gets the probability of a given word or small sentence being profane
    /// </summary>
    /// <param name="word"></param>
    /// <returns> 0 < prediction < 1</returns>
    public float GetProfanityProbability(string word)
    {
        var obj = new BadWord { Word = word };
        var biggestProb = 0f;

        foreach (var engine in _engines)
        {
            var currProb = engine.Predict(obj).Probability;
            biggestProb = Math.Max(currProb, biggestProb);
        }

        return biggestProb;
    }
}