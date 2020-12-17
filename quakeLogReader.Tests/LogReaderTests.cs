using System;
using System.IO;
using System.Linq;
using quakeLogReader.Dto;
using Xunit;

namespace quakeLogReader.Tests
{
    public class LogReaderTests
    {
        [Fact]
        public void Test1_WhenGivenEmptyPath_ThenShouldReturnInvalidPathError()
        {
            // Arrange
            Quake3ArenaLogReader reader = new Quake3ArenaLogReader();

            // Act
            reader.ReadLog(string.Empty);

            // Assert
            Assert.True(reader.HasErrors);
            Assert.Equal(1, reader.Errors.Count(x => x.Equals("Invalid path")));
            Assert.Equal(1, reader.Errors.Count(x => x.Equals("File does not exist")));
        }

        [Fact]
        public void Test2_WhenGivenInvalidFilePath_ThenShouldReturnFileNotExistError()
        {
            // Arrange
            Quake3ArenaLogReader reader = new Quake3ArenaLogReader();

            // Act
            reader.ReadLog("anInvalidPathToAFile");

            // Assert
            Assert.True(reader.HasErrors);
            Assert.Equal(1, reader.Errors.Count(x => x.Equals("File does not exist")));
        }

        [Fact]
        public void Test3_WhenGivenMatchWithNoDeaths_ThenShouldNotReturnAnyDeath()
        {
            // Arrange
            Quake3ArenaLogReader reader = new Quake3ArenaLogReader();
            string currentDirectory = Directory.GetCurrentDirectory();

            // Act
            reader.ReadLog(@$"{currentDirectory}\input\Test3.log");

            // Assert
            Assert.False(reader.HasErrors);
            Assert.Single(reader.Games);
            Assert.Equal(0, reader.Games[0].TotalKills);
            Assert.Equal(2, reader.Games[0].Players.Count);
            Assert.Equal(1, reader.Games[0].Players.Values.Count(x => x.Equals("Isgalamido")));
            Assert.Equal(1, reader.Games[0].Players.Values.Count(x => x.Equals("Dono da Bola")));
        }

        [Fact]
        public void Test4_WhenGivenMatchWith5Deaths_ThenShouldReturn5TotalKills()
        {
            // Arrange
            Quake3ArenaLogReader reader = new Quake3ArenaLogReader();
            string currentDirectory = Directory.GetCurrentDirectory();

            // Act
            reader.ReadLog(@$"{currentDirectory}\input\Test4.log");

            // Assert
            Assert.False(reader.HasErrors);
            Assert.Single(reader.Games);
            
            GameDto game = reader.Games[0];
            Assert.NotNull(game);
            Assert.Equal(5, game.TotalKills);
            Assert.Equal(2, game.Players.Count);
            Assert.Equal(1, game.Players.Values.Count(x => x.Equals("Isgalamido")));
            Assert.Equal(1, game.Players.Values.Count(x => x.Equals("Dono da Bola")));
            Assert.Equal(1, game.Score[2]); // Isgalamido
            Assert.Equal(0, game.Score[3]); // Dono da Bola
            Assert.Equal(3, game.KillsByWeapon[Common.MeansOfDeath.MOD_ROCKET_SPLASH]);
            Assert.Equal(2, game.KillsByWeapon[Common.MeansOfDeath.MOD_TRIGGER_HURT]);
        }
    }
}
