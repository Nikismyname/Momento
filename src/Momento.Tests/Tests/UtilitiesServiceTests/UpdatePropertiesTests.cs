namespace Momento.Tests.Tests.UtilitiesServiceTests
{
    using FluentAssertions;
    using Momento.Services.Contracts.Utilities;
    using Momento.Services.Implementations.Utilities;
    using Momento.Tests.Seeding;
    using NUnit.Framework;
    using System;

    public class UpdatePropertiesTests
    {
        private IUtilitiesService utilitiesService;

        [SetUp]
        public void Setup()
        {
            this.utilitiesService = new UtilitiesService();
        }

        [Test]
        public void ShouldWork()
        {
            var source = new TestSource();
            var target = new TestTarget();

            this.utilitiesService.UpdatePropertiesReflection(source, target);

            source.Date.Should().Be(target.Date);
            source.Name.Should().Be(target.Name);
            source.Id.Should().Be(target.Id);
        }

        [Test]
        public void ShouldThrowIfPropertiesAreDifferentTypes()
        {
            var source = new TestSource2();
            var target = new TestTarget2();

            Action action = () => this.utilitiesService.UpdatePropertiesReflection(source, target);
            action.Should().Throw<Exception>();
        }

        [Test]
        public void ShouldNotThrowWhenTheProblemPropNameIsIgnored()
        {
            var source = new TestSource2();
            var target = new TestTarget2();

            Action action = () => this.utilitiesService.UpdatePropertiesReflection(source, target,new string[] {"Collection" } );
            action.Should().NotThrow();
        }

        [Test]
        public void IfProprtiesAreIgnoredTheirValueShouldNotChange()
        {
            var source = new TestSource();
            var target = new TestTarget();

            var ignoredProperties = new string[] {"Id", "Name", "Date" };
            Action action = () => this.utilitiesService.UpdatePropertiesReflection(source, target, ignoredProperties);
            action.Invoke();

            source.Date.Should().NotBe(target.Date);
            source.Name.Should().NotBe(target.Name);
            source.Id.Should().NotBe(target.Id);
        }

        [Test]
        public void IfExclusiveProprtiesAreSetOnlyThenShouldChange()
        {
            var source = new TestSource();
            var target = new TestTarget();

            var exclusiveProperties = new string[] { "Id"};
            Action action = () => this.utilitiesService.UpdatePropertiesReflection(source, target, null, exclusiveProperties);
            action.Invoke();

            source.Date.Should().NotBe(target.Date);
            source.Name.Should().NotBe(target.Name);
            source.Id.Should().Be(target.Id);
        }
    }
}
