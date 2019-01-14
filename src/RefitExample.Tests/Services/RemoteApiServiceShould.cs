using FluentAssertions;
using RefitExample.Models;
using RefitExample.Services;
using System;
using Xunit;

namespace RefitExample.Tests.Services
{
    public class RemoteApiServiceShould
    {
        [Fact]
        public void ConstructForNormalUse()
        {
            var remoteApiService = new RemoteApiService(LogLevel.None, TimeSpan.FromSeconds(30));
            remoteApiService.Should().BeOfType<RemoteApiService>();
        }

        [Fact]
        public void ConstructForAuthenticatedUse()
        {
            var remoteApiService = new RemoteApiService(LogLevel.None,
                                                        TimeSpan.FromSeconds(30),
                                                        new Login
                                                        {
                                                            UserName = "testuser",
                                                            Password = "testpassword"
                                                        });
            remoteApiService.Should().BeOfType<RemoteApiService>();
        }
    }
}