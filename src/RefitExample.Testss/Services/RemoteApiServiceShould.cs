using FluentAssertions;
using RefitExample.Interfaces;
using RefitExample.Models;
using RefitExample.Services;
using Xunit;

namespace RefitExample.Testss.Services
{
    public class RemoteApiServiceShould
    {
        private readonly IRemoteApiService _remoteApiService;

        public RemoteApiServiceShould() =>
            _remoteApiService = new RemoteApiService(LogLevel.None);

        [Fact]
        public void Construct() =>
            _remoteApiService.Should().BeOfType<RemoteApiService>();
    }
}
