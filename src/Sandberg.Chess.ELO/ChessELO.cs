using System;
using System.Linq;

namespace Sandberg.Chess
{
    public class ELO
    {
        public enum GameResult { White = 1, Remis = 2, Black = 3 }

        public static RatingResult CalculateNewRating(double ratingPlayerWhite, double ratingPlayerBlack, GameResult result, int kFaktorWhite = 20, int kFaktorBlack = 20)
        {
            double _scoreWhite = result == GameResult.White ? 1 : result == GameResult.Remis ? 0.5 : 0;
            double _scoreBlack = result == GameResult.Black ? 1 : result == GameResult.Remis ? 0.5 : 0;

            var expectedScores = GetExpectedScore(ratingPlayerWhite, ratingPlayerBlack);
            var newRatings = GetNewRatings(ratingPlayerWhite, ratingPlayerBlack, expectedScores.playerWhite, expectedScores.playerBlack, _scoreWhite, _scoreBlack, kFaktorWhite, kFaktorBlack);

            return new RatingResult
            {
                Winner = result,
                White = new RatingPlayerResult(ratingPlayerWhite, newRatings.newRatingWhite, expectedScores.playerWhite, _scoreWhite, kFaktorWhite),
                Black = new RatingPlayerResult(ratingPlayerBlack, newRatings.newRatingBlack, expectedScores.playerBlack, _scoreBlack, kFaktorBlack)
            };
        }

        public static double RatingPerformance(double[] wins, double[] remis, double[] losses, int factor = 400)
        {
            var w = wins.Sum() + (wins.Count() * factor);
            var r = remis.Sum();
            var l = losses.Sum() - (losses.Count() * factor);
            var n = wins.Count() + remis.Count() + losses.Count();
            return (w + r + l) / n;
        }

        public static (double playerWhite, double playerBlack) GetExpectedScore(double ratingWhite, double ratingBlack)
        {
            double expectedScoreWhite = 1 / (1 + (Math.Pow(10, (ratingBlack - ratingWhite) / 400)));
            double expectedScoreBlack = 1 / (1 + (Math.Pow(10, (ratingWhite - ratingBlack) / 400)));
            return (expectedScoreWhite, expectedScoreBlack);
        }

        private static (double newRatingWhite, double newRatingBlack) GetNewRatings(double ratingWhite, double ratingBlack, double expectedWhite, double expectedBlack, double scoreWhite, double scoreBlack, int kFaktorWhite, int kFaktorBlack)
        {
            double newRatingA = ratingWhite + (kFaktorWhite * (scoreWhite - expectedWhite));
            double newRatingB = ratingBlack + (kFaktorBlack * (scoreBlack - expectedBlack));
            return (newRatingA, newRatingB);
        }


        public class RatingResult
        {
            public GameResult Winner { get; internal set; } = GameResult.Remis;
            public RatingPlayerResult White { get; internal set; } = new RatingPlayerResult();
            public RatingPlayerResult Black { get; internal set; } = new RatingPlayerResult();
        }

        public class RatingPlayerResult
        {
            public RatingPlayerResult() { }
            public RatingPlayerResult(double oldRating, double newRating, double expectedScore, double score, int kFactor) { NewRating = newRating; OldRating = oldRating; ExpectedScore = expectedScore; Score = score; KFactor = kFactor; }

            public int KFactor { get; private set; }
            public double ExpectedScore { get; private set; }
            public double Score { get; private set; }
            public double OldRating { get; private set; }
            public double NewRating { get; private set; }
            public double RatingAdjustment => NewRating - OldRating;
        }
    }
}