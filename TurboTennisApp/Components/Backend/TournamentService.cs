using Microsoft.EntityFrameworkCore;
using TurboTennisApp.Components.Backend;
using TurboTennisApp.Components.Backend.Data;
using TurboTennisApp.Components.Backend.Models;

namespace TurboTennisApp.Components.Backend
{
    public class TournamentService
    {
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
        public async Task<Tournament> GetTournamentByIdAsync(int id)
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

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task AddPlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
    }
}
