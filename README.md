# Sandberg.ChessELO

Easy to use library to calculate Chess ELO rating and ELO performance

### Calculate ELO rating
```c#
RatingResult Sandberg.Chess.ELO.CalculateNewRating(double, double, GameResult, int?, int?);
```

```c#
# Example

double playerWhite = 1298; // Set white player rating
double playerBlack = 1485; // Set black player rating
var result = Sandberg.Chess.ELO.GameResult.White; // White/Remis/Black
var ratingResult = Sandberg.Chess.ELO.CalculateNewRating(playerWhite, playerBlack, result);
```

You can set kFactor for both white player and black player. Default is 20 for both.

CalculateNewRating will return a `Sandberg.Chess.ELO.RatingResult` object:
```js
{
  "Winner": 1,
  "White": {
    "KFactor": 20,
    "ExpectedScore": 0.6400649998028851,
    "Score": 1,
    "OldRating": 1300,
    "NewRating": 1307.1987000039423,
    "RatingAdjustment": 7.198700003942349
  },
  "Black": {
    "KFactor": 20,
    "ExpectedScore": 0.35993500019711494,
    "Score": 0,
    "OldRating": 1200,
    "NewRating": 1192.8012999960577,
    "RatingAdjustment": -7.198700003942349
  }
}
```

### Calculate performance rating
Performance rating is a hypothetical rating that would result from the games of a single event only.
According to this algorithm, performance rating for an event is calculated in the following way:
- For each win, add your opponent's rating plus 400 *(factor)*,
- For each remis, add your opponent's rating
- For each loss, add your opponent's rating minus 400 *(factor)*,
- And divide this sum by the number of played games.

(You can use a different *factor* you like, but 400 is default)

```c#
double Sandberg.Chess.ELO.RatingPerformance(double[], double[], double[], int?);
```

```c#
double[] wins = [1200, 1300, 1100, 1600];
double[] remis = [1245, 1300];
double[] losses = [1500];

var performance = Sandberg.Chess.ELO.RatingPerformance(wins, remis, losses);
// performance rating --> 1492,142857142857
```