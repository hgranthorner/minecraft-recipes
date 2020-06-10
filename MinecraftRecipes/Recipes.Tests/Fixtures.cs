using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace Recipes.Tests
{
    public static class Fixtures
    {
        public static IFixture IntIdFixture()
        {
            var fix = new Fixture();
            fix.Customizations.Add(new IntIdBuilder());
            return fix;
        }

        private class IntIdBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (request is ParameterInfo pi &&
                    pi.Name == "id" &&
                    pi.ParameterType == typeof(int))
                    return new Random().Next(Seed.Items.Count);
                return new NoSpecimen();
            }
        }
    }
}