using FluentAssertions;
using RefitExample.Interfaces;
using RefitExample.Models;
using RefitExample.Services;
using System;
using Xunit;

namespace RefitExample.Tests.Services
{
    public class RemoteApiServiceShould
    {
        private readonly IRemoteApiService _remoteApiService;

        public RemoteApiServiceShould() =>
            _remoteApiService = new RemoteApiService(LogLevel.None, TimeSpan.FromSeconds(30));

        [Fact]
        public void Construct() =>
            _remoteApiService.Should().BeOfType<RemoteApiService>();
    }
}