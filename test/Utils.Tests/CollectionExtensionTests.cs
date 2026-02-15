using Liamth99.Utils.Extensions;
using Shouldly;

namespace Utils.Tests;

public class CollectionExtensionTests
{
    public class CollectionExtensionsTests
    {
        [Fact]
        public void Shuffle_ListIsNull_ThrowsArgumentNullException()
        {
            List<int>? list = null;

            Should.Throw<ArgumentNullException>(() => list!.Shuffle(new Random()));
        }

        [Fact]
        public void Shuffle_RandomIsNull_ThrowsArgumentNullException()
        {
            var list = new List<int> { 1, 2, 3 };
            Random? random = null;

            Should.Throw<ArgumentNullException>(() => list.Shuffle(random!));
        }

        [Fact]
        public void Shuffle_EmptyList_RemainsEmpty()
        {
            var list = new List<int>();
            list.Shuffle();
            list.ShouldBeEmpty();
        }

        [Fact]
        public void Shuffle_SingleItemList_RemainsUnchanged()
        {
            var list = new List<int> { 42 };
            list.Shuffle();

            list.Count.ShouldBe(1);
            list.ShouldBeEquivalentTo(new List<int> { 42, });
        }

        [Fact]
        public void Shuffle_List_ShufflesElements()
        {
            var list = Enumerable.Range(1, 10).ToList();
            list.Shuffle(new Random(0)); // Fixed seed

            list.ShouldBe([1, 5, 6, 9, 3, 2, 4, 7, 10, 8,]);
        }

#if NET8_0_OR_GREATER

        [Fact]
        public void Shuffle_Span_NullRandom_UsesSharedRandom()
        {
            var array = Enumerable.Range(1, 10).ToArray();
            array.AsSpan().Shuffle(new Random(1)); // Fixed seed

            array.ShouldBe([7, 9, 5, 2, 10, 8, 6, 4, 1, 3,]);
        }
#endif
    }
}