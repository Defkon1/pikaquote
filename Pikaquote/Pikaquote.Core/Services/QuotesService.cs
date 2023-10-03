using Pikaquote.Core.Abstractions;
using Pikaquote.Core.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Pikaquote.Core.Extensions;

namespace Pikaquote.Core.Services
{
    public class QuotesService : IQuotesService
    {
        private readonly ILogger<QuotesService> _logger;

        private readonly IEnumerable<QuotesDictionary> _supportedDictionaries = new List<QuotesDictionary>
        {
            new QuotesDictionary()
            {
                Slug = string.Empty,
                Name = "Main dictionary",
                Source = string.Empty
            },
            new QuotesDictionary()
            {
                Slug = "devops",
                Name = "DevOps Fortunes - https://pastebin.com/p296KDcE",
                Source = "/usr/games/fortune-devops",
            },
            new QuotesDictionary()
            {
                Slug = "murphy-it",
                Name = "Murphy's Laws (it) - https://pastebin.com/rHtSXFbx",
                Source = "/usr/games/fortune-murphy-it"
            }
        };

        public QuotesService(ILogger<QuotesService> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<QuotesDictionaryDto>> GetDictionariesAsync()
        {
            return _supportedDictionaries
                .OrderBy(d => d.Slug)
                .Select(d => d.ToDto());
        }

        public async Task<QuoteDto> GetQuoteAsync(string dictionary)
        {
            string errors = string.Empty;
            string results = string.Empty;

            var supportedDictionary = _supportedDictionaries.FirstOrDefault(d => string.Compare(d.Slug, dictionary, StringComparison.InvariantCultureIgnoreCase) == 0);

            var argument = supportedDictionary?.Source ?? string.Empty;

            try
            {
                var psi = new ProcessStartInfo()
                {
                    FileName = "/usr/games/fortune",
                    Arguments = argument,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (var process = Process.Start(psi))
                {
                    errors = await process.StandardError.ReadToEndAsync();

                    results = await process.StandardOutput.ReadToEndAsync();
                }

                return new QuoteDto()
                {
                    CreatedAt = DateTime.UtcNow,
                    Message = results,
                    UsedDictionary = supportedDictionary?.Name ?? _supportedDictionaries.First(d => string.IsNullOrEmpty(d.Slug)).Name
                };
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                _logger.LogError(exception, message: errors);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unknown error");
            }

            return null;
        }
    }
}
