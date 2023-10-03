using Pikaquote.Core.Models;

namespace Pikaquote.Core.Extensions
{
    public static class QuotesDictionaryExtensions
    {
        public static QuotesDictionaryDto ToDto(this QuotesDictionary dictionary)
        {
            return new QuotesDictionaryDto()
            {
                Slug = dictionary.Slug,
                Name = dictionary.Name
            };
        }
    }
}
