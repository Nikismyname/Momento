namespace Momento.Tests.Tests.ListToDoTests
{
    using AutoMapper;
    using Momento.Services.Mapping;
    using Momento.Services.Models.VideoModels;
    using NUnit.Framework;

    [SetUpFixture]
    public class SharedSetUp
    {
        [OneTimeSetUp]
        public void InitializeAutoMapper()
        {
            AutoMapperConfig.RegisterMappings(typeof(VideoCreate).Assembly);
        }

        [OneTimeTearDown]
        public void ResetAutomapper()
        {
            Mapper.Reset();
        }
    }
}
