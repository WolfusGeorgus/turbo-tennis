using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend
{
    public class TournamentService
    {
        private readonly TournamentContext _context;
        private int amountSets = 3;

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
            int amountGroups = players.Count / minPlayersInGroup;
            int amountInCross = 8;
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

            GameStatus gameStatus = (await GetAllGameStatusAsync()).Where(s => s.Name == "Created").First();
            Phase phase = (await GetAllPhasesAsync()).Where(s => s.Name == "Group").First();

            foreach (var group in groups)
            {
                int groupOrder = 1;
                for (int i = 0; i < group.Players.Count;i++)
                {
                    for (int j = i + 1; j < group.Players.Count; j++)
                    {
                        Game game = new();
                        game.Status = gameStatus;
                        game.StatusId = gameStatus.Id;
                        game.Phase = phase;
                        game.PhaseId = phase.Id;
                        game.Order = groupOrder++;
                        Player player1 = group.Players.ElementAt(i);
                        Player player2 = group.Players.ElementAt(j);

                        PlayerGame pg1 = new();
                        PlayerGame pg2 = new();
                        pg1.Player = player1;
                        pg1.PlayerId = player1.Id;   
                        pg2.Player = player2;
                        pg2.PlayerId = player2.Id;
                        pg1.Status = game.Status;
                        pg1.StatusId = game.StatusId;
                        pg2.Status = game.Status;
                        pg2.StatusId = game.StatusId;

                        await AddPlayerGameAsync(pg1);
                        await AddPlayerGameAsync(pg2);

                        game.PlayerGames.Add(pg1);
                        game.PlayerGames.Add(pg2);

                        for (int k = 1;  k <= amountSets; k++)
                        {
                            Set set = new Set();
                            set.Game = game;
                            set.GameId = game.Id;

                            PlayerSet playerSet1 = new PlayerSet();
                            PlayerSet playerSet2 = new PlayerSet();

                            playerSet1.Player = player1;
                            playerSet2.Player = player2;
                            playerSet1.PlayerId = player1.Id;
                            playerSet2.PlayerId = player2.Id;

                            set.PlayerSets.Add(playerSet1);
                            set.PlayerSets.Add(playerSet2);

                            game.Sets.Add(set);
                        }
                    }
                }
            }

            phase = (await GetAllPhasesAsync()).Where(s => s.Name == "Cross").First();
            int crossOrder = 1;
            for (int i = 0; i < amountInCross; i++)
            {
                Game game = new Game();
                game.Phase = phase;
                game.PhaseId = phase.Id;
                game.Order = crossOrder++;

                for (int k = 1; k <= amountSets; k++)
                {
                    Set set = new Set();
                    set.Game = game;
                    set.GameId = game.Id;

                    game.Sets.Add(set);
                }
            }

            phase = (await GetAllPhasesAsync()).Where(s => s.Name == "Half").First();
            int halfOrder = 1;
            for (int i = 0; i < 4; i++)
            {
                Game game = new Game();
                game.Phase = phase;
                game.PhaseId = phase.Id;
                game.Order = halfOrder++;

                for (int k = 1; k <= amountSets; k++)
                {
                    Set set = new Set();
                    set.Game = game;
                    set.GameId = game.Id;

                    game.Sets.Add(set);
                }
            }

            phase = (await GetAllPhasesAsync()).Where(s => s.Name == "Placement").First();
            int placementOrder = 1;
            for (int i = 0; i < 4; i++)
            {
                Game game = new Game();
                game.Phase = phase;
                game.PhaseId = phase.Id;
                game.Order = placementOrder;
                placementOrder += 2;

                for (int k = 1; k <= amountSets; k++)
                {
                    Set set = new Set();
                    set.Game = game;
                    set.GameId = game.Id;

                    game.Sets.Add(set);
                }
            }
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

        public async Task AddPlayerGameAsync(PlayerGame game)
        {
            _context.PlayerGames.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GameStatus>> GetAllGameStatusAsync()
        {
            return await _context.GameStatuses.ToListAsync();
        }
    }
}
