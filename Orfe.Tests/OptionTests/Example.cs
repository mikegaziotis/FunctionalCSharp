using Xunit;

namespace Orfe.Tests;

public class Example
{
    [Theory]
    [InlineData(null)]
    [InlineData(5)]
    public void TestNull(int? userId)
    {

        Option<Person> bongo = null!;
        Person? person = null;
        if(userId.HasValue)
        {
            bongo = Person.GetById(userId.Value);
        }
        Assert.NotNull(bongo);
        Assert.Null(person);
        Assert.Equal(userId.HasValue, bongo.HasValue);
        Works(bongo, userId.HasValue);

    }



    private static void Works(Option<Person> person, in bool shouldHaveValue)
    {
        Assert.Equal(shouldHaveValue, person.HasValue);
    }

}

public record Person
{
    public int Id {get; init;}

    private Person(int id)
    {
        Id = id;
    }
    public static Person GetById(int id) => new(id);
}
