using Microsoft.ML;
using DotnetBadWordDetector.Model;
using System.Text.RegularExpressions;

namespace DotnetBadWordDetector;
public class ProfanityDetector
{
    const string MODEL_BASE_PATH = "DotnetBadWordDetector.Data.bad-words-model-{LOCALE}.zip";
    const string TAG = "{LOCALE}";
    List<PredictionEngine<BadWord, BadWordPrediction>> _engines = new List<PredictionEngine<BadWord, BadWordPrediction>>();
    
    /// <summary>
    /// Default constructor to initialize the profanity detector with English locale or all locales
    /// Consider that different locales may cause false-positives on other languages, so use with caution. 
    /// </summary>
    /// <param name="allLocales"></param>
    public ProfanityDetector(bool allLocales = false)
    {

        Locales[] locales = allLocales ? [Locales.ENGLISH, Locales.SPANISH, Locales.PORTUGUESE]
                                        : [Locales.ENGLISH];
        LoadTrainedModel(locales);
    }

    /// <summary>
    /// Constructor to initialize the profanity detector with specific locales.
    /// Consider that different locales may cause false-positives on other languages, so use with caution. 
    /// </summary>
    /// <param name="locales"></param> <summary>
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
        var cleanWord = Regex.Replace(word, @"[^a-zA-Z0-9\s@]", "");
        var obj = new BadWord { Word = cleanWord };
        foreach (var engine in _engines)
        {
            if (engine.Predict(obj).Prediction)
               return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if a phrase contains any profane words
    /// </summary>
    /// <param name="phrase"></param>
    /// <returns>true if classified as profane</returns>
    public bool IsPhraseProfane(string phrase)
    {
        var words = phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in words)
        {
            if (IsProfane(word))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the probability of a given sentence to contain profane words
    /// </summary>
    /// <param name="word"></param>
    /// <returns> 0 < prediction < 1</returns>
    public float GetPhraseProfanityProbability(string phrase)
    {
        var words = phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var biggestProb = 0f;

        foreach (var word in words)
        {
            var currProb = GetProfanityProbability(word);
            biggestProb = Math.Max(currProb, biggestProb);
        }

        return biggestProb;
    }

    /// <summary>
    /// Gets the probability of a given word or small sentence being profane
    /// </summary>
    /// <param name="word"></param>
    /// <returns> 0 < prediction < 1</returns>
    public float GetProfanityProbability(string word)
    {
        var cleanWord = Regex.Replace(word, @"[^a-zA-Z0-9\s@]", "");
        var obj = new BadWord { Word = cleanWord };
        var biggestProb = 0f;

        foreach (var engine in _engines)
        {
            var currProb = engine.Predict(obj).Probability;
            biggestProb = Math.Max(currProb, biggestProb);
        }

        return biggestProb;
    }

    /// <summary>
    /// Masks the profane words in a phrase with a given character
    /// </summary>
    /// <param name="phrase"></param>
    /// <param name="maskChar"></param>
    /// <returns>Phrase with masked bad words</returns>
    public string MaskProfanity(string phrase, char maskChar = '*')
    {
        var words = phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var censoredWords = new List<string>();

        foreach (var word in words)
        {
            if (IsProfane(word))
            {
                var maskedWord = new string(maskChar, word.Length);
                censoredWords.Add(maskedWord);
            }
            else
                censoredWords.Add(word);
        }

        return string.Join(' ', censoredWords);
    }
}