using EasyZap.Data;
using EasyZap.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace EasyZap.Service
{
    public class LinkService
    {
        private readonly EasyZapContext _context;

        private readonly IConfiguration _config;

        public LinkService(EasyZapContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<MasterLink> CreateMasterLinkAsync(int masterId, TimeSpan? ttl = null)
        {
            string token = null!;
            for(int attempt = 0; attempt < 5; attempt++)
            {
                token = GenerateToken();
                var exists = await _context.MasterLinks.AnyAsync(x => x.Token == token);

                if (!exists) 
                    break;

                token = null!;
            }

            if (token == null) throw new InvalidOperationException("Не удалось сгенерировать уникальный токен");

            var inv = new MasterLink
            {
                MasterId = masterId,
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = ttl.HasValue ? DateTime.UtcNow.Add(ttl.Value) : null,
                IsActive = true
            };

            _context.MasterLinks.Add(inv);
            await _context.SaveChangesAsync();

            return inv;
        }

        public Task<MasterLink?> GetByTokenAsync(string token) => 
            _context.MasterLinks.Include(x => x.MasterId)
            .FirstOrDefaultAsync(x => x.Token == token && x.IsActive);

        public async Task RevokeTokenAsync(string token)
        {
            var item = await _context.MasterLinks.FirstOrDefaultAsync(x => x.Token == token);
            if (item == null) return;
            item.IsActive = false;
            await _context.SaveChangesAsync();
        }

        public Task<List<MasterLink>> GetForMasterAsync(int masterId) =>
            _context.MasterLinks.Where(x => x.MasterId == masterId).ToListAsync();

        private string GenerateToken(int bytes = 12)
        {
            var data = RandomNumberGenerator.GetBytes(bytes); // 12 байт = ~16 символов
            return Base64UrlEncode(data);
        }

        private static string Base64UrlEncode(byte[] bytes)
        {
            var s = Convert.ToBase64String(bytes);
            s = s.TrimEnd('=').Replace('+', '-').Replace('/', '_');
            return s;
        }
    }
}
