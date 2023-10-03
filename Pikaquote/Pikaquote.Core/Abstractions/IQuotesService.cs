using Pikaquote.Core.Models;

namespace Pikaquote.Core.Abstractions
{
    public interface IQuotesService
    {
        Task<IEnumerable<QuotesDictionaryDto>> GetDictionariesAsync();

        Task<QuoteDto> GetQuoteAsync(string dictionary);
    }
}
