using FluentAssertions;

namespace Hamjo.Scheduling.Team.Core.Tests.Unit;

[TestClass]
public sealed class CategoryScheduleTests
{
    [TestMethod]
    public void GivenNoPlayers_WhenMoveNext_ThenReturnsFalse()
    {
        using CategorySchedule categorySchedule = new(Enumerable.Empty<Player>().GetEnumerator(), 1);

        bool result = categorySchedule.MoveNext();

        Assert.IsFalse(result, $"{nameof(categorySchedule)} returned an alignment");
    }

    [DataTestMethod]
    [DataRow(2, 1, 1, new[] { 0 })]
    [DataRow(2, 1, 2, new[] { 1 })]
    [DataRow(2, 1, 3, new[] { 0 })]
    [DataRow(3, 2, 1, new[] { 0, 1 })]
    [DataRow(3, 2, 2, new[] { 2, 0 })]
    public void GivenAnAlignmentOfTwoPlayersAndATeamOf3Players_WhenMoveNextTwice_ThenReturnsThirdThenFirstPlayers(int playerCount, int playerCountPerAlignment, int countOfMoveNext, int[] playerIndicesExpected)
    {
        IEnumerable<Player> players = [.. Enumerable.Range(0, playerCount).Select(x => new Player())];
        using CategorySchedule categorySchedule = new(players.GetEnumerator(), playerCountPerAlignment);

        for (int i = 0; i < countOfMoveNext; i++)
        {
            bool result = categorySchedule.MoveNext();
            Assert.IsTrue(result, $"{nameof(CategorySchedule)} did not return an alignment");
        }

        categorySchedule.Current.Players.Should().Equal(playerIndicesExpected.Select(i => players.ElementAt(i)), object.ReferenceEquals);
    }

    [DataTestMethod]
    [DataRow(5, 2)]
    public void WhenResetAndMoveNext_ThenFirstPlayerReturned(int playerCount, int countOfMoveNext)
    {
        IEnumerable<Player> players = [.. Enumerable.Range(0, playerCount).Select(x => new Player())];
        using CategorySchedule categorySchedule = new(players.GetEnumerator(), 1);

        for (int i = 0; i < countOfMoveNext; i++)
        {
            bool result = categorySchedule.MoveNext();
            Assert.IsTrue(result, $"{nameof(CategorySchedule)} did not return an alignment");
        }
        {
            categorySchedule.Reset();
            bool result = categorySchedule.MoveNext();
            Assert.IsTrue(result, $"{nameof(CategorySchedule)} did not return an alignment");
        }

        categorySchedule.Current.Players.Should().Equal([players.ElementAt(0)], object.ReferenceEquals);
    }
}
