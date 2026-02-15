using System.Threading;

namespace Liamth99.Utils.Extensions;

/// <summary>
/// Provides extension methods for collection manipulation and utility operations.
/// </summary>
public static class CollectionExtensions
{
#if !NET8_0_OR_GREATER
    private static readonly ThreadLocal<Random> _random = new(() => new Random());

    private static Random SharedRandom => _random.Value!;
#endif

    /// <summary>
    /// Randomly shuffles the elements of a given list, modifying the list in place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle. The list is updated with the shuffled order in place.</param>
    public static void Shuffle<T>(this IList<T> list)
#if NET8_0_OR_GREATER
        => list.Shuffle(Random.Shared);
#else
        => list.Shuffle(SharedRandom);
#endif


    /// <summary>
    /// Randomly shuffles the elements of a given list, modifying the list in place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">The list to shuffle. The list is updated with the shuffled order in place.</param>
    /// <param name="random"><see cref="System.Random"/> instance.</param>
    public static void Shuffle<T>(this IList<T> list, Random random)
    {
        if (list is null)
            throw new ArgumentNullException(nameof(list));
        if (random is null)
            throw new ArgumentNullException(nameof(random));

        for (int i = list.Count - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);

            T temp = list[i];
            list[i] = list[k];
            list[k] = temp;
        }
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Randomly shuffles the elements of a given list, modifying the list in place.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="span">The span to shuffle. The span is updated with the shuffled order in place.</param>
    /// <param name="random"><see cref="System.Random"/> instance. Defaults to <see cref="System.Random.Shared"/></param>
    public static void Shuffle<T>(this Span<T> span, Random? random = null)
    {
        random ??= Random.Shared;

        for (int i = span.Length - 1; i > 0; i--)
        {
            int k = random.Next(i + 1);

            T temp = span[i];
            span[i] = span[k];
            span[k] = temp;
        }
    }
#endif
}