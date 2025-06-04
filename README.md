# .NET Bad Word Detector and Filter 
[![NuGet](https://img.shields.io/nuget/v/DotnetBadWordDetector.svg)](https://www.nuget.org/packages/DotnetBadWordDetector/)

This is a fast and robust library that detects offensive language within text strings. It currently supports English, Portuguese, and Spanish languages.

## How It Works

This library uses a logistic regression [ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet) model trained on thousands of human-labeled words. The trained model is embedded as a resource in this library and is consulted on every prediction.

## Why Use This Library?

Unlike other .NET profanity detection libraries that rely solely on static hard-coded lists of bad words — such as [ProfanityDetector](https://github.com/stephenhaunts/ProfanityDetector) — which uses [this list stored in memory](https://github.com/stephenhaunts/ProfanityDetector/blob/main/ProfanityFilter/ProfanityFilter/ProfanityList.cs), this library uses a machine learning approach that can detect creative substitutions and misspellings (e.g., "h0us3" instead of "house"). This makes it much harder to bypass.

## Performance

In benchmarks, this library was up to **618 times faster** than the most downloaded .NET package for detecting profanity. For 100 successive predictions, it was approximately **24 times faster**.

| Package                | 1 Prediction | 10 Predictions | 100 Predictions |
| ---------------------- | ------------ | -------------- | --------------- |
| .NET Bad Word Detector | 0.0462 ms    | 1.5508 ms      | 4.1887 ms       |
| ProfanityDetector      | 28.5823 ms   | 42.4606 ms     | 102.0750 ms     |

**PC specs:** Dell Inspiron 13, i7 8th gen, 16 GB RAM.

## Installation

```bash
dotnet add package DotnetBadWordDetector
```

## How to Use

### Create the detector

```csharp
using DotnetBadWordDetector;

// English only (default)
var detector = new ProfanityDetector();

// Or load all supported languages: English, Spanish, and Portuguese
var detectorAll = new ProfanityDetector(allLocales: true);
```

---

### Check if a word is offensive

```csharp
if (detector.IsProfane("example")) {
    // Word is classified as offensive
}
```

---

### Check if a phrase contains any offensive words

```csharp
if (detector.IsPhraseProfane("this is an example")) {
    // Phrase contains at least one offensive word
}
```

---

### Get the probability that a word or phrase is offensive

```csharp
float probWord = detector.GetProfanityProbability("example");
float probPhrase = detector.GetPhraseProfanityProbability("this is an example");
```

---

### Mask offensive words in a phrase

```csharp
string cleanText = detector.MaskProfanity("this is an example", '*');
```

This will replace any detected offensive words with asterisks or your chosen character.

## Model Quality

```bash
Model quality metrics evaluation
--------------------------------
Accuracy: 98.43%
AUC: 99.49%
F1 Score: 97.25%
```

## Notes

This library is not perfect: it is not 100% accurate and it is context-free — meaning it cannot detect profane phrases made of individually inoffensive words.
Definitions of "profanity" can vary by culture. This library uses human-labeled data, which might not align perfectly with all contexts.

## Tips

* Keep the detector instance in memory for better performance — it uses very little memory (less than 100 KB).
* Be cautious when enabling all locales together, as it may produce more false positives in multilingual texts.

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request with suggestions for new features, languages, or improvements.
