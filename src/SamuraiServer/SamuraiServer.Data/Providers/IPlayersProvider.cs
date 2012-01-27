using System;
using System.Collections.Generic;

namespace SamuraiServer.Data
{
    public interface IPlayersProvider
    {
        IEnumerable<Player> GetLeaderboard(int page, int players);
        ValidationResult<Player> Create(string name);
        ValidationResult<Player> Login(string name, string apiKey);
        Player Get(Guid id);
        void Invite(Guid id);
        void Ban(Guid id);
        IEnumerable<Player> Search(string searchTerm);
    }
}