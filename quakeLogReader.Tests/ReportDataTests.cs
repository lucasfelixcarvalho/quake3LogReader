using System.Linq;
using quakeLogReader.Dto;
using quakeLogReader.Report;
using Xunit;

namespace quakeLogReader.Tests
{
    public class ReportDataTests
    {
        [Fact]
        public void Test1_WhenGivenEmptyGame_ThenReportShouldHaveEmptyValues()
        {
            // Arrange
            ReportData report = new ReportData();

            // Assert
            Assert.Equal(0, report.Id);
            Assert.Equal(0, report.TotalKills);
            Assert.Empty(report.Players);
            Assert.Empty(report.Kills);
            Assert.Empty(report.KillsByMeans);
            Assert.NotEmpty(report.ToString());
        }

        [Fact]
        public void Test2_WhenGivenRealGame_ThenReportShouldHaveMatchingValues()
        {
            // Arrange
            GameDto game = new GameDto(1);
            game.TotalKills = 10;
            game.Players.Add(1, "Player 1");
            game.Players.Add(2, "Player 2");
            game.Score.Add(1, 6);
            game.Score.Add(2, 4);
            game.KillsByWeapon.Add(Common.MeansOfDeath.MOD_RAILGUN, 6);
            game.KillsByWeapon.Add(Common.MeansOfDeath.MOD_SHOTGUN, 4);

            // Act
            ReportData report = new ReportData(game);

            // Assert
            Assert.Equal(1, report.Id);
            Assert.Equal(10, report.TotalKills);
            Assert.Equal(2, report.Players.Length);
            Assert.Equal(1, report.Players.ToList().Count(x => x.Equals("Player 1")));
            Assert.Equal(1, report.Players.ToList().Count(x => x.Equals("Player 2")));
            Assert.Equal(6, report.Kills["Player 1"]);
            Assert.Equal(4, report.Kills["Player 2"]);
            Assert.Equal(6, report.KillsByMeans[Common.MeansOfDeath.MOD_RAILGUN.ToString()]);
            Assert.Equal(4, report.KillsByMeans[Common.MeansOfDeath.MOD_SHOTGUN.ToString()]);
            Assert.NotEmpty(report.ToString());
        }
    }
}
