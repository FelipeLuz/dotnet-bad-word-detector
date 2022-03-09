# .NET Bad Word Detector

This is a fast and robust library that detects offensive language within text strings. It currently supports only English language, more languages will be added soon.

## How It Works

This library uses a logistic regression [ML.NET](https://dotnet.microsoft.com/en-us/apps/machinelearning-ai/ml-dotnet) model trained on thousands of human-labeled words. The trained model then is loaded as a resource for this lib and consulted on every new prediction. 

## Why to use this library?

Up to this moment all .NET profanity detection libraries use a hard-coded list of curse words to detect profanity, for instance, [ProfanityDetector](https://github.com/stephenhaunts/ProfanityDetector) uses this [list stored in memory](https://github.com/stephenhaunts/ProfanityDetector/blob/main/ProfanityFilter/ProfanityFilter/ProfanityList.cs), there are obvious glaring issues with this approach, and while they might be performant, these list based libraries are not comprehensive, they are easily outperformed by misspelling and by the human creativity to replace letters for meaningless chars creating new words that are perceived as curse words (e.g. house and h0us3).

## Performance

| Package                | 1 Prediction | 10 Predicitons | 100 predictions |
|------------------------|--------------|----------------|-----------------|
| .Net Bad Word Detector | 0.0462 ms    | 1.5508 ms      | 4.1887 ms       |
| ProfanityDetector      | 28.5823 ms   | 42.4606 ms     | 102.0750 ms     |

PC specs: Dell Inspiron 13, I7 8th gen, 16 GB.

## How to install

```bash
dotnet add package DotnetBadWordDetector --version 1.0.1
```

## How to use it

```csharp
var detector = new ProfanityDetector();

if(detector.IsProfane("foo bar")){
    //do something
}

```
It is strongly suggested to keep the library always loaded in memory to increase its performance, it uses very little memory (less than 100 KB).
## Accuracy, AUC and F1 score

```bash
Model quality metrics evaluation
--------------------------------
Accuracy: 98.43%
Auc: 99.49%
F1Score: 97.25%
```

## Caveat

This library is not perfect, it is not 100% precise, and it is context-free, e.g. it can not detect profane phrases consisted of decent words. Also people diverge on what is considered profane.





