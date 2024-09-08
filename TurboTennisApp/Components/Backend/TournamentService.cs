using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend
{
    public class TournamentService
    {
        private const int PlayersInQuarterFinale = 8;
        private const int PlayersInHalfFinale = 4;
        private const int PlayersInFinale = 2;
        private const int AmountSets = 3;
        private readonly TournamentContext _context;

        public TournamentService(TournamentContext context)
        {
            _context = context;
        }

        public async Task AddPhaseAsync(Phase phase)
        {
            _context.Phases.Add(phase);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Phase>> GetAllPhasesAsync()
        {
            return await _context.Phases.ToListAsync();
        }

        // Beispiel: Füge ein neues Turnier hinzu
        public async Task AddTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
        }      

        // Beispiel: Hole ein Turnier nach ID
        public async Task<Tournament?> GetTournamentByIdAsync(int id)
        {
            return await _context.Tournaments.FindAsync(id);
        }   
        
        public async Task<Tournament?> GetCompleteTournamentByIdAsync(int id)
        {

            return await _context.Tournaments
                .Include(t => t.Groups)
                .ThenInclude(g => g.Players)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // Beispiel: Hole alle Turniere
        public async Task<List<Tournament>> GetAllTournamentsAsync()
        {
            return await _context.Tournaments.ToListAsync();
        }

        // Beispiel: Aktualisiere ein Turnier
        public async Task UpdateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Update(tournament);
            await _context.SaveChangesAsync();
        }

        // Beispiel: Lösche ein Turnier
        public async Task DeleteTournamentAsync(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament != null)
            {
                _context.Tournaments.Remove(tournament);
                await _context.SaveChangesAsync();
            }
        }

        public async Task InitTournamentAsync(Tournament tournament, List<Player> players, int minPlayersInGroup)
        {
            //await AddTournamentAsync(tournament);

            int amountGroups = players.Count / minPlayersInGroup;
            var shuffledPlayers = players.OrderBy(p => Guid.NewGuid()).ToList();
            var groups = new List<Group>();
            for (int i = 0; i < amountGroups; i++) 
            {
                Group newGroup = new Group
                {
                    Name = $"Gruppe {i + 1}",
                    Tournament = tournament,
                    TournamentId = tournament.Id,
                    Players = new List<Player>()
                };
                groups.Add(newGroup);
            }

            tournament.Groups = groups;

            int groupIndex = 0;
            foreach (var player in shuffledPlayers)
            {
                groups[groupIndex].Players.Add(player);
                groupIndex = (groupIndex + 1) % amountGroups;
            }
            

            var Phases = await GetAllPhasesAsync();
            var phaseGroup = Phases.First(p => p.Name == "Group");
            var phaseQuarter = Phases.First(p => p.Name == "Quarter");
            var phaseHalf = Phases.First(p => p.Name == "Half");
            var phasePlacement = Phases.First(p => p.Name == "Placement");
            GameStatus createdStatus = (await GetAllGameStatusAsync()).Where(s => s.Name == "Created").First();

            List<Game> games = new();
            foreach (var group in groups)
            {
                games.AddRange(CreateGroupGames(group, createdStatus, phaseGroup));
            }

            games.AddRange(CreateKnockoutGames(phaseQuarter, createdStatus, PlayersInQuarterFinale));
            games.AddRange(CreateKnockoutGames(phaseHalf, createdStatus, PlayersInHalfFinale));
            games.AddRange(CreateFinalGames(phasePlacement, createdStatus, PlayersInFinale));

            foreach (var game in games)
            {
                tournament.Games.Add(game);
            }
            await UpdateTournamentAsync(tournament);
        }

        private Game CreateGame(GameStatus gameStatus, Phase phase, int order, Player? player1 = null, Player? player2 = null)
        {
            Game game = new Game()
            {
                Status = gameStatus,
                StatusId = gameStatus.Id,
                Phase = phase,
                PhaseId = phase.Id,
                Order = order,
            };

            if (player1 is not null && player2 is not null)
            {
                game.PlayerGames.Add(CreatePlayerGame(gameStatus, player1));
                game.PlayerGames.Add(CreatePlayerGame(gameStatus, player2));
            }

            return game;
        }

        private void CreateSetsForGame(Game game, Player? player1 = null, Player? player2 = null)
        {
            for (int setNumber = 1; setNumber <= AmountSets; setNumber++)
            {
                Set set = new Set
                {
                    Game = game,
                    GameId = game.Id
                };

                if (player1 is not null && player2 is not null)
                {
                    set.PlayerSets.Add(CreatePlayerSet(player1));
                    set.PlayerSets.Add(CreatePlayerSet(player2));
                }

                game.Sets.Add(set);
            }
        }

        private List<Game> CreateKnockoutGames(Phase phase, GameStatus gameStatus, int playersCount)
        {
            List<Game> Games = new();
            int gameOrder = 1;
            for (int i = 0; i < playersCount; i++)
            {
                Game game = CreateGame(gameStatus, phase, gameOrder++);
                Games.Add(game);
                for (int k = 1; k <= AmountSets; k++)
                {
                    CreateSetsForGame(game);
                }
            }
            return Games;
        }

        private List<Game> CreateFinalGames(Phase phase, GameStatus gameStatus, int playersCount)
        {
            List<Game> Games = new();
            int placementOrder = 1;
            for (int i = 0; i < playersCount; i++)
            {
                Game game = CreateGame(gameStatus, phase, placementOrder);
                Games.Add(game);
                placementOrder += 2;

                for (int k = 1; k <= AmountSets; k++)
                {
                    CreateSetsForGame(game);
                }
            }
            return Games;
        }

        private List<Game> CreateGroupGames(Group group, GameStatus gameStatus, Phase phase)
        {
            List<Game> Games = new();
            var playersList = group.Players.ToList();
            int gameOrder = 1;
            for (int i = 0; i < playersList.Count; i++)
            {
                for (int j = i + 1; j < playersList.Count; j++)
                {
                    var player1 = playersList[i];
                    var player2 = playersList[j];
                    Game game = CreateGame(gameStatus, phase, gameOrder++, player1, player2);
                    Games.Add(game);
                    // Erstelle Sets für das Spiel
                    CreateSetsForGame(game, player1, player2);
                }
            }
            return Games;
        }

        private PlayerSet CreatePlayerSet(Player player)
        {
            return new PlayerSet()
            {
                Player = player,
                PlayerId = player.Id
            };
        }

        private PlayerGame CreatePlayerGame(GameStatus gameStatus, Player player)
        {
            return new PlayerGame()
            {
                Player = player,
                PlayerId = player.Id,
                Status = gameStatus,
                StatusId = gameStatus.Id
            };
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task AddPlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }

        public async Task AddGameAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }     
        
        public async Task AddGameAsync(IEnumerable<Game> games)
        {
            _context.Games.AddRange(games);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Group>> GetAllGroupsByTournamentId(int id)
        {
            return await _context.Groups
                .Where(g => g.TournamentId == id)
                .ToListAsync();
        }
        
        public async Task AddGroupAsync(IEnumerable<Group> groups)
        {
            _context.Groups.AddRange(groups);
            await _context.SaveChangesAsync();
        }


        public async Task<List<GameStatus>> GetAllGameStatusAsync()
        {
            return await _context.GameStatuses.ToListAsync();
        }
    }
}
