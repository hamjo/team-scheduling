using System.Collections;

namespace Hamjo.Scheduling.Team.Core;

public sealed class CategorySchedule(IEnumerator<Player> players, int playerCountPerAlignment) : IEnumerator<TeamAlignment>
{
    private TeamAlignment? _current;
    public TeamAlignment Current => _current!;
    object IEnumerator.Current => _current!;

    public void Dispose()
    {
        players.Dispose();
    }

    public bool MoveNext()
    {
        Player[] playersInAlignment = [.. Take(playerCountPerAlignment)];
        _current = new(playersInAlignment);
        return playersInAlignment.Length == playerCountPerAlignment;
    }

    private IEnumerable<Player> Take(int playerCount)
    {
        for (int i = playerCount - 1; i >= 0; i--)
        {
            if (players.MoveNext())
            {
                yield return players.Current;
            }
            else
            {
                players.Reset();
                if (players.MoveNext())
                {
                    yield return players.Current;
                }
                else
                {
                    yield break;
                }
            }
        }
    }

    public void Reset()
    {
        players.Reset();
    }
}
