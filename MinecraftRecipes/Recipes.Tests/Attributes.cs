using AutoFixture.Xunit2;

namespace Recipes.Tests
{
    public static class Attributes
    {
        public class IdAutoDataAttribute : AutoDataAttribute
        {
            public IdAutoDataAttribute() : base(Fixtures.IntIdFixture)
            {
            }
        }
    }
}