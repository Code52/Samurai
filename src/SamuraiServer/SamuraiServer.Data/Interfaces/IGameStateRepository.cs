using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamuraiServer.Data
{
    public interface IGameStateRepository
    {
        /// <summary>
        /// Load a GameSate by id
        /// </summary>
        GameState Load(Guid id);
        
        /// <summary>
        /// Save a GameState for later retrieval. An Id will be generated and stored on the object.
        /// </summary>
        void Save(GameState state);

        /// <summary>
        /// Fetch a list of currently active games that a player could join or spectate
        /// </summary>
        IEnumerable<GameState> ListOpenGames();

        /// <summary>
        
        /// Fetch a list of currently active games for a signed-in user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IEnumerable<GameState> ListCurrentGames(string userName);
    }
}
