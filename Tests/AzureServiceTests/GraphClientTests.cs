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
        public async Task GetAllApplicationsAsync_NoApplications_ReturnsEmpty()
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
        public async Task GetAllApplicationsAsync_SingleApplicationMappedToInternalClass_ReturnsOneWithAllProperties()
        {
            // Arrange
            var graphServiceClientMock = new Mock<IGraphServiceClient>();

            // Create an empty page of applications
            GraphServiceApplicationsCollectionPage page = new GraphServiceApplicationsCollectionPage
            {
                AdditionalData = new Dictionary<string, object>()
            };
            var passwordCredentialDisplayName = "rbac";
            var passwordCredentialExpiry = new DateTimeOffset(2021, 2, 27, 9, 0, 0,
                   DateTimeOffset.Now.Offset);
            var passwordCredentialStart = new DateTimeOffset(2020, 2, 27, 9, 0, 0,
                   DateTimeOffset.Now.Offset);
            var passwordCredential = new PasswordCredential
            {
                DisplayName = passwordCredentialDisplayName,
                EndDateTime = passwordCredentialExpiry,
                StartDateTime = passwordCredentialStart
            };
            var applicationDisplayName = "App1";
            var applicationId = Guid.NewGuid().ToString();
            page.Add(
                new Application()
                {
                    AppId = applicationId,
                    DisplayName = applicationDisplayName,
                    PasswordCredentials = new List<PasswordCredential> { passwordCredential }
                }
            );

            graphServiceClientMock.Setup(m => m.Applications.Request().Select(It.IsAny<Expression<Func<Application, object>>>()).Top(999).GetAsync()).ReturnsAsync(() => page);
            var graphClient = new GraphClient(graphServiceClientMock.Object);

            // Act
            var applications = await graphClient.GetAllApplicationsAsync();

            // Assert
            Assert.That(applications, Has.Count.EqualTo(1));
            Assert.That(applications[0].Id, Is.EqualTo(applicationId));
            Assert.That(applications[0].DisplayName, Is.EqualTo(applicationDisplayName));
            Assert.That(applications[0].ServicePrincipals, Has.Count.EqualTo(1));
            Assert.That(applications[0].ServicePrincipals[0].DisplayName, Is.EqualTo(passwordCredentialDisplayName));
            Assert.That(applications[0].ServicePrincipals[0].EndDateTime, Is.EqualTo(passwordCredentialExpiry));
            Assert.That(applications[0].ServicePrincipals[0].StartDateTime, Is.EqualTo(passwordCredentialStart));
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
            var app1DisplayName = "App1";
            var app2DisplayName = "App2";
            page.Add(GetApplication(Guid.NewGuid().ToString(), app1DisplayName));
            page.Add(GetApplication(Guid.NewGuid().ToString(), app2DisplayName));

            graphServiceClientMock.Setup(m => m.Applications.Request().Select(It.IsAny<Expression<Func<Application, object>>>()).Top(999).GetAsync()).ReturnsAsync(() => page);
            var graphClient = new GraphClient(graphServiceClientMock.Object);

            // Act
            var apps = await graphClient.GetAllApplicationsAsync();

            // Assert
            Assert.That(apps, Has.Count.EqualTo(2));
            Assert.That(apps[0].DisplayName, Is.EqualTo(app1DisplayName));
            Assert.That(apps[1].DisplayName, Is.EqualTo(app2DisplayName));
        }

        private Application GetApplication(string id, string displayName)
        {
            return new Application()
            {
                AppId = id,
                DisplayName = displayName,
                PasswordCredentials = new List<PasswordCredential> { }
            };
        }
    }
}
