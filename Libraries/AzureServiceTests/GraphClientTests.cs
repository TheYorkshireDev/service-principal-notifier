using Microsoft.Graph;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SPN.Libraries.AzureService.Test
{
    public class GraphClientTests
    {
        [Test]
        public void Constructor_NullArguments_ThrowsException()
        {
            Assert.That(() => new GraphClient(null), Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public async Task GetAllApplicationsAsync_NoApplications_ReturnsEmptyAsync()
        {
            // Arrange
            var graphServiceClientMock = new Mock<IGraphServiceClient>();

            // Create an empty page of applications
            GraphServiceApplicationsCollectionPage page = new GraphServiceApplicationsCollectionPage
            {
                AdditionalData = new Dictionary<string, object>()
            };

            graphServiceClientMock.Setup(m => m.Applications.Request().Select(It.IsAny<Expression<Func<Application, object>>>()).Top(999).GetAsync()).ReturnsAsync(() => page);
            var graphClient = new GraphClient(graphServiceClientMock.Object);

            // Act
            var apps = await graphClient.GetAllApplicationsAsync();

            // Assert
            Assert.That(apps, Is.Empty);
        }

        [Test]
        public async Task GetAllApplicationsAsync_TwoApplicationsFromAPi_ReturnsListOfTwo()
        {
            // Arrange
            var graphServiceClientMock = new Mock<IGraphServiceClient>();

            GraphServiceApplicationsCollectionPage page = new GraphServiceApplicationsCollectionPage
            {
                AdditionalData = new Dictionary<string, object>()
            };
            page.Add(new Application() { AppId = "App1" });
            page.Add(new Application() { AppId = "App2" });

            graphServiceClientMock.Setup(m => m.Applications.Request().Select(It.IsAny<Expression<Func<Application, object>>>()).Top(999).GetAsync()).ReturnsAsync(() => page);
            var graphClient = new GraphClient(graphServiceClientMock.Object);

            // Act
            var apps = await graphClient.GetAllApplicationsAsync();

            // Assert
            Assert.That(apps, Has.Count.EqualTo(2));
            Assert.That(apps[0].AppId, Is.EqualTo("App1"));
            Assert.That(apps[1].AppId, Is.EqualTo("App2"));
        }
    }
}
