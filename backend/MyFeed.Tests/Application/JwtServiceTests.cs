using Moq;
using MyFeed.Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Xunit;

namespace MyFeed.Tests.Application
{
    public class JwtServiceTests
    {
        [Fact]
        public void GenerateToken_WithValidConfiguration_ReturnsToken()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);

            // Act
            var token = service.GenerateToken(1, "testuser");

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenerateToken_WithValidConfiguration_ContainsCorrectClaims()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);
            var userId = 42;
            var username = "testuser";

            // Act
            var token = service.GenerateToken(userId, username);
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Equal(userId.ToString(), jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
            Assert.Equal(username, jsonToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value);
            Assert.Equal(userId.ToString(), jsonToken.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value);
            Assert.NotNull(jsonToken.Claims.FirstOrDefault(c => c.Type == "jti"));
        }

        [Fact]
        public void GenerateToken_WithValidConfiguration_HasCorrectIssuerAndAudience()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);

            // Act
            var token = service.GenerateToken(1, "testuser");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Equal("MyFeed", jsonToken.Issuer);
            Assert.Contains("MyFeedUsers", jsonToken.Audiences);
        }

        [Fact]
        public void GenerateToken_WithValidConfiguration_TokenExpiresInCorrectTime()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);
            var beforeGeneration = DateTime.UtcNow;

            // Act
            var token = service.GenerateToken(1, "testuser");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var afterGeneration = DateTime.UtcNow;

            // Assert
            var expectedExpiration = beforeGeneration.AddHours(24);
            var actualExpiration = jsonToken.ValidTo;
            
            // Allow 1 minute tolerance for test execution time
            Assert.True(actualExpiration >= expectedExpiration.AddMinutes(-1));
            Assert.True(actualExpiration <= expectedExpiration.AddMinutes(1));
        }

        [Fact]
        public void GenerateToken_MissingSecretKey_ThrowsException()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns((string?)null);
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
                service.GenerateToken(1, "testuser")
            );
        }

        [Fact]
        public void GenerateToken_MissingIssuer_ThrowsException()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns((string?)null);
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
                service.GenerateToken(1, "testuser")
            );
        }

        [Fact]
        public void GenerateToken_MissingAudience_ThrowsException()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns((string?)null);
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("24");

            var service = new JwtService(configuration.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => 
                service.GenerateToken(1, "testuser")
            );
        }

        [Fact]
        public void GenerateToken_MissingExpirationHours_UsesDefault24Hours()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns((string?)null);

            var service = new JwtService(configuration.Object);
            var beforeGeneration = DateTime.UtcNow;

            // Act
            var token = service.GenerateToken(1, "testuser");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var afterGeneration = DateTime.UtcNow;

            // Assert - should default to 24 hours
            var expectedExpiration = beforeGeneration.AddHours(24);
            var actualExpiration = jsonToken.ValidTo;
            
            Assert.True(actualExpiration >= expectedExpiration.AddMinutes(-1));
            Assert.True(actualExpiration <= expectedExpiration.AddMinutes(1));
        }

        [Fact]
        public void GenerateToken_WithCustomExpirationHours_UsesCustomValue()
        {
            // Arrange
            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x["Jwt:SecretKey"]).Returns("2be36c272fcc5fe6009d3b00c07340ace8ccdd15693fc964a752097ea2e0ac94");
            configuration.Setup(x => x["Jwt:Issuer"]).Returns("MyFeed");
            configuration.Setup(x => x["Jwt:Audience"]).Returns("MyFeedUsers");
            configuration.Setup(x => x["Jwt:ExpirationHours"]).Returns("48");

            var service = new JwtService(configuration.Object);
            var beforeGeneration = DateTime.UtcNow;

            // Act
            var token = service.GenerateToken(1, "testuser");
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);

            // Assert - should use 48 hours
            var expectedExpiration = beforeGeneration.AddHours(48);
            var actualExpiration = jsonToken.ValidTo;
            
            Assert.True(actualExpiration >= expectedExpiration.AddMinutes(-1));
            Assert.True(actualExpiration <= expectedExpiration.AddMinutes(1));
        }
    }
}

