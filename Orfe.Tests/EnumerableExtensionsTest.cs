using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Orfe.Tests;

public class EnumerableExtensionsTest
{

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public void ForEachTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = numbers.ForEach(n => sum += n);
        Assert.Equal(expectedSum, sum);
    }

    [Theory]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public async Task ForEachAsyncTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = await numbers.ForEachAsync(async n => sum += await Task.FromResult(n));
        Assert.Equal(expectedSum, sum);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public void IterTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = numbers.Iter(n => sum += n);
        Assert.Equal(expectedSum, sum);
    }

    [Theory]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public async Task IterAsyncTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = await numbers.IterAsync(async n => sum += await Task.FromResult(n));
        Assert.Equal(expectedSum, sum);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public void TapTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = numbers.Tap(n => sum += n);
        Assert.Equal(expectedSum, sum);
    }

    [Theory]
    [InlineData(1, 10, 55)]
    [InlineData(5, 15, 180)]
    public async Task TapAsyncTest(int lower, int upper, int expectedSum)
    {
        var numbers = Enumerable.Range(lower, upper).ToArray();
        var sum = 0;
        _ = await numbers.TapAsync(async n => sum += await Task.FromResult(n));
        Assert.Equal(expectedSum, sum);
    }

    [Fact]
    public void MapTest()
    {
        var numbers = Enumerable.Range(1, 10);
        var squares = numbers.Map(n => n * n).ToSafeArray();
        var expectedSquares = new[] { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100 };
        Assert.Equal(expectedSquares, squares);
    }

    [Fact]
    public async Task MapAsyncTest()
    {
        var numbers = Enumerable.Range(1, 10);
        var squares = await numbers.MapAsync(async n => await Task.FromResult(n * n)).ToSafeArrayAsync();
        var expectedSquares = new[] { 1, 4, 9, 16, 25, 36, 49, 64, 81, 100 };
        Assert.Equal(expectedSquares, squares);
    }

    [Fact]
    public async Task MapAsyncWithDelayTest()
    {
        var numbers = Enumerable.Range(1, 5);
        var squares = await numbers.MapAsync(async n =>
        {
            await Task.Delay(2); // Simulate async work
            return n * n;
        }).ToSafeArrayAsync();
        var expectedSquares = new[] { 1, 4, 9, 16, 25 };
        Assert.Equal(expectedSquares, squares);
    }

    [Fact]
    public void FilterTest()
    {
        var numbers = Enumerable.Range(1, 10);
        var evenNumbers = numbers.Filter(n => n % 2 == 0);
        var expectedEvens = new[] { 2, 4, 6, 8, 10 };
        Assert.Equal(expectedEvens, evenNumbers);
    }

    [Fact]
    public async Task FilterAsyncTest()
    {
        var numbers = Enumerable.Range(1, 10);
        var evenNumbers = await numbers.FilterAsync(async n => await Task.FromResult(n % 2 == 0));
        var expectedEvens = new[] { 2, 4, 6, 8, 10 };
        Assert.Equal(expectedEvens, evenNumbers);
    }

    [Fact]
    public void FoldTest()
    {
        var numbers = Enumerable.Range(1, 5); // 1, 2, 3, 4, 5
        var sum = numbers.Fold((acc, n) => acc + n, 0);
        Assert.Equal(15, sum);
    }

    [Fact]
    public async Task FoldAsyncTest()
    {
        var numbers = Enumerable.Range(1, 5); // 1, 2, 3, 4, 5
        var sum = await numbers.FoldAsync(async (acc, n) => await Task.FromResult(acc + n), 0);
        Assert.Equal(15, sum);
    }

    [Fact]
    public async Task ParallelForEachTest()
    {
        var numbers = Enumerable.Range(1, 10).ToSafeArray();
        var sum = 0;
        var lockObj = new Lock();

        _ = await numbers.ParallelForEachAsync(async (n, token) =>
        {
            if(token.IsCancellationRequested)
                return;
            var square = await Task.FromResult(n * n);
            lockObj.Enter();
            try
            {
                lock (lockObj)
                {
                    sum += square;
                }
            }
            finally
            {
                lockObj.Exit();
            }

        }, new ParallelOptions {MaxDegreeOfParallelism = 4});

        var expectedSum = numbers.Select(n => n * n).Sum();
        Assert.Equal(expectedSum, sum);
    }


    [Fact]
    public void ChainSyncTest()
    {
        var personsLogged = 0;
        var personsPromoted = 0;
        var avgAge = PersonService.GetPersons()
            .Tap(LogPerson)
            .Filter(x => x.IsAdult())
            .ForEach(p =>
            {
                if (p.CanBePromoted())
                    p.Promote();
            })
            .Iter(LogPromoted)
            .Map(RankedPerson.FromPerson)
            .Fold(
                (acc, rankedPerson) => (sum: acc.sum + rankedPerson.Person.Age, count: acc.count + 1),
                (sum: 0.0, count: 0))
            .Pipe(acc => acc.count == 0 ? 0 : acc.sum / acc.count);

        //Debug.Print(avgAge.ToString(CultureInfo.InvariantCulture));
        //Console.WriteLine(avgAge);

        Assert.True(avgAge.IsBetween(35.0, 37.0));
        Assert.Equal(9,personsLogged);
        Assert.Equal(7,personsPromoted);
        return;

        void LogPerson(Person person)
        {
            personsLogged++;
        }
        void LogPromoted(Person person)
        {
            if (person.IsPromoted)
                personsPromoted++;
        }
    }

    [Fact]
    public async Task ChainAfterAsyncTest()
    {
        var personsLogged = 0;
        var personsPromoted = 0;
        var avgAge = await PersonService
            .GetPersonsAsync()
            .Tap(LogPerson)
            .Filter(x => x.IsAdult())
            .ForEach(p =>
            {
                if (p.CanBePromoted())
                    p.Promote();
            })
            .Iter(LogPromoted)
            .Map(RankedPerson.FromPerson)
            .Fold(
                (acc, rankedPerson) => (sum: acc.sum + rankedPerson.Person.Age, count: acc.count + 1),
                (sum: 0.0, count: 0))
            .Pipe(GetAverage);

        //Debug.Print(avgAge.ToString(CultureInfo.InvariantCulture));
        //Console.WriteLine(avgAge);

        Assert.True(avgAge.IsBetween(35.0, 37.0));
        Assert.Equal(9,personsLogged);
        Assert.Equal(7,personsPromoted);
        return;

        void LogPerson(Person person)
        {
            personsLogged++;
        }
        void LogPromoted(Person person)
        {
            if (person.IsPromoted)
                personsPromoted++;
        }
        double GetAverage((double sum, int count) tuple)
        {
            var (sum, count) = tuple;
            return count == 0 ? 0 : sum / count;
        }
    }

    [Fact]
    public async Task AsyncChainTest()
    {
        var personsLogged = 0;
        var personsPromoted = 0;
        var avgAge = await PersonService.GetPersonsAsync()
            .TapAsync(LogPersonAsync)
            .FilterAsync(x => Task.FromResult(x.IsAdult()))
            .ForEachAsync(p =>
            {
                if (p.CanBePromoted())
                    p.Promote();
                return Task.FromResult(p);
            })
            .IterAsync(LogPromotedAsync)
            .MapAsync(x=> Task.FromResult(RankedPerson.FromPerson(x)))
            .FoldAsync(
                (acc, rankedPerson) =>Task.FromResult((sum: acc.sum + rankedPerson.Person.Age, count: acc.count + 1)),
                (sum: 0.0, count: 0))
            .PipeAsync(GetAverageAsync);

        //Debug.Print(avgAge.ToString(CultureInfo.InvariantCulture));
        //Console.WriteLine(avgAge);

        Assert.True(avgAge.IsBetween(35.0, 37.0));
        Assert.Equal(9,personsLogged);
        Assert.Equal(7,personsPromoted);
        return;

        Task<Person> LogPersonAsync(Person person)
        {
            personsLogged++;
            return Task.FromResult(person);
        }
        Task<Person> LogPromotedAsync(Person person)
        {
            if (person.IsPromoted)
                personsPromoted++;
            return Task.FromResult(person);
        }
        Task<double> GetAverageAsync((double sum, int count) tuple)
        {
            var (sum, count) = tuple;
            return Task.FromResult(count == 0 ? 0 : sum / count);
        }
    }

    private record Person(string Id, int Age)
    {
        private bool _promoted;

        public bool IsPromoted => _promoted;
        public bool IsAdult() => Age >= 18;

        public void Promote()
        {
            if (!_promoted)
                _promoted = true;
        }

        public bool CanBePromoted() => Age.IsBetween(18, 70) && !_promoted;
    }

    private enum RankType
    {
        Soldier,
        Captain,
        Major,
        General,
        Civilian
    }

    private record RankedPerson(RankType Rank, Person Person)
    {
        public static RankedPerson FromPerson(Person person)
        {
            if (person.Age.IsBetween(18, 25))
                return new RankedPerson(RankType.Soldier, person);
            if (person.Age.IsBetween(26, 35))
                return new RankedPerson(RankType.Captain, person);
            if (person.Age.IsBetween(36, 55))
                return new RankedPerson(RankType.Major, person);
            if (person.Age.IsBetween(56, 75))
                return new RankedPerson(RankType.General, person);
            return new RankedPerson(RankType.Civilian, Person: person);
        }
    };

    private static class PersonService
    {
        private static readonly Person[] People =
        [
            new(Guid.CreateVersion7().ToString(), 7),
            new(Guid.CreateVersion7().ToString(), 12),
            new(Guid.CreateVersion7().ToString(), 19),
            new(Guid.CreateVersion7().ToString(), 25),
            new(Guid.CreateVersion7().ToString(), 30),
            new(Guid.CreateVersion7().ToString(), 22),
            new(Guid.CreateVersion7().ToString(), 40),
            new(Guid.CreateVersion7().ToString(), 57),
            new(Guid.CreateVersion7().ToString(), 63)
        ];

        public static async Task<IEnumerable<Person>> GetPersonsAsync()
            => await Task.FromResult(People.AsEnumerable()).ConfigureAwait(false);

        public static IEnumerable<Person> GetPersons()
            => People.AsEnumerable();
    }
}
