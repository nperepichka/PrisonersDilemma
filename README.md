# Prisoner`s Dilemma

## About
**Author**: N. V. Perepichka  
Implemented in scope of PhD study  

## Implementation
* .NET 8

## How to
```
Gameplay.exe [OptionsFilePath]
```

## Options
Syntax
```json
{
  "GameType": [string: (Tournament, Population)],
  "D": [int],
  "C": [int],
  "d": [int],
  "c": [int],
  "Strategies": [string[]],
  "HumaneFlexible": [bool],
  "SelfishFlexible": [bool],
  "FlexibilityValue": [int],
  "Seed": [double],
  "MinSteps": [int],
  "DominantStrategy": [string],
  "BasePopulationMultiplicity": [int],
  "Mutation": [double],
  "SameLastCooperationScores": [int],
  "ValuableCooperationScoreNumber": [double],
  "TournamentRepeats": [int]
}
```
Example:
```json
{
  "GameType": "Tournament",
  "D": 7,
  "C": 5,
  "d": 2,
  "c": 0,
  "Strategies": null,
  "HumaneFlexible": false,
  "SelfishFlexible": false,
  "FlexibilityValue": 0,
  "Seed": 0,
  "MinSteps": 100,
  "DominantStrategy": null,
  "BasePopulationMultiplicity": 1,
  "Mutation": 0,
  "SameLastCooperationScores": 10,
  "ValuableCooperationScoreNumber": 0.01,
  "TournamentRepeats": 1
}
```
