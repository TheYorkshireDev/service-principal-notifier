using NUnit.Framework;
using SPN.Function.Services;
using SPN.Models;
using System;
using System.Collections.Generic;

namespace SPN.Function.Test
{
    public class FilterServicePrincipalsTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetExpiringAndExpired_XApplicationWithZeroCredentials_ExpiredPropertyReturnsEmpty(int numberOfApplications)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApplications; i++)
            {
                applications.Add(GetApplication());
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expired, Is.Empty);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetExpiringAndExpired_XApplicationWithZeroCredentials_ExpiringPropertyReturnsEmpty(int numberOfApplications)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApplications; i++)
            {
                applications.Add(GetApplication());
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expiring, Is.Empty);
        }

        [Test, Combinatorial]
        public void GetExpiringAndExpired_SingleApplicationWithActiveExpiringAndExpired_InputNumberMatchesReturned(
            [Values(0, 1, 2, 3)] int numberOfActiveSPs,
            [Values(0, 1, 2, 3)] int numberOfExpiredSPs,
            [Values(0, 1, 2, 3)] int numberOfExpiringSPs)
        {
            // Arrange
            var application = GetApplication(displayName: "App1");
            for (var i = 0; i < numberOfActiveSPs; i++)
            {
                application.ServicePrincipals.Add(GetActiveServicePrincipal());
            }
            for (var i = 0; i < numberOfExpiredSPs; i++)
            {
                application.ServicePrincipals.Add(GetExpiredServicePrincipal());
            }
            for (var i = 0; i < numberOfExpiringSPs; i++)
            {
                application.ServicePrincipals.Add(GetExpiringServicePrincipal());
            }

            var applications = new List<ActiveDirectoryApplication> { application };
            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            if (numberOfExpiredSPs > 0)
            {
                Assert.That(servicePrincipals.Expired, Has.Count.EqualTo(1));
                Assert.That(servicePrincipals.Expired[0].ServicePrincipals, Has.Count.EqualTo(numberOfExpiredSPs));
            }
            else
            {
                Assert.That(servicePrincipals.Expired, Has.Count.EqualTo(0));
            }

            if (numberOfExpiringSPs > 0)
            {
                Assert.That(servicePrincipals.Expiring, Has.Count.EqualTo(1));
                Assert.That(servicePrincipals.Expiring[0].ServicePrincipals, Has.Count.EqualTo(numberOfExpiringSPs));
            }
            else
            {
                Assert.That(servicePrincipals.Expiring, Has.Count.EqualTo(0));
            }
        }

        [TestCase(32)]
        [TestCase(31)]
        [TestCase(30)]
        [TestCase(29)]
        [TestCase(28)]
        [TestCase(27)]
        public void GetExpiringAndExpired_SingleApplicationWithXDaysRemaining_InputsThirtyOrLessAreExpiringIsReturned(int daysRemaining)
        {
            // Arrange
            var application = GetApplication(displayName: "App1");
            var dateInFuture = new DateTimeOffset(DateTime.UtcNow.AddDays(daysRemaining));
            application.ServicePrincipals.Add(GetServicePrincipal(endDateTime: dateInFuture));

            var applications = new List<ActiveDirectoryApplication> { application };
            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            if (daysRemaining <= 30)
            {
                Assert.That(servicePrincipals.Expiring, Has.Count.EqualTo(1));
                Assert.That(servicePrincipals.Expiring[0].ServicePrincipals, Has.Count.EqualTo(1));
            }
            else
            {
                Assert.That(servicePrincipals.Expiring, Has.Count.EqualTo(0));
            }

            Assert.That(servicePrincipals.Expired, Is.Empty);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetExpiringAndExpired_XApplicationsWithActiveCredentials_ExpiredPropertyReturnsEmpty(int numberOfApps)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApps; i++)
            {
                var application = GetApplication(displayName: $"App{i}");
                application.ServicePrincipals.Add(GetActiveServicePrincipal());
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expired, Is.Empty);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetExpiringAndExpired_XApplicationsWithActiveCredentials_ExpiringPropertyReturnsEmpty(int numberOfApps)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApps; i++)
            {
                var application = GetApplication(displayName: $"App{i}");
                application.ServicePrincipals.Add(GetActiveServicePrincipal());
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expiring, Is.Empty);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetExpiringAndExpired_XApplicationsWithExpiringCredentials_ExpiringPropertyReturnsX(int numberOfApps)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApps; i++)
            {
                var application = GetApplication(displayName: $"App{i}");
                application.ServicePrincipals.Add(GetExpiringServicePrincipal());
                applications.Add(application);
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expiring, Has.Count.EqualTo(numberOfApps));
            Assert.That(servicePrincipals.Expired, Is.Empty);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void GetExpiringAndExpired_XApplicationsWithExpiredCredentials_ExpiredPropertyReturnsX(int numberOfApps)
        {
            // Arrange
            var applications = new List<ActiveDirectoryApplication>();
            for (var i = 0; i < numberOfApps; i++)
            {
                var application = GetApplication(displayName: $"App{i}");
                application.ServicePrincipals.Add(GetExpiredServicePrincipal());
                applications.Add(application);
            }

            var filterServicePrincipals = new FilterServicePrincipals();

            // Act
            var servicePrincipals = filterServicePrincipals.GetExpiringAndExpired(applications);

            // Assert
            Assert.That(servicePrincipals.Expiring, Is.Empty);
            Assert.That(servicePrincipals.Expired, Has.Count.EqualTo(numberOfApps));
        }

        private static ActiveDirectoryApplication GetApplication(string id = null, string displayName = "")
        {
            return new ActiveDirectoryApplication
            {
                Id = id ?? Guid.NewGuid().ToString(),
                DisplayName = displayName,
                ServicePrincipals = new List<ServicePrincipal>()
            };
        }

        private static ServicePrincipal GetActiveServicePrincipal()
        {
            var dateInFuture = new DateTimeOffset(DateTime.UtcNow.AddYears(1));
            return GetServicePrincipal(endDateTime: dateInFuture);
        }

        private static ServicePrincipal GetExpiringServicePrincipal()
        {
            var dateWithinMonth = new DateTimeOffset(DateTime.UtcNow.AddDays(30));
            return GetServicePrincipal(endDateTime: dateWithinMonth);
        }

        private static ServicePrincipal GetExpiredServicePrincipal()
        {
            var dateInPast = new DateTimeOffset(DateTime.UtcNow.AddDays(-1));
            return GetServicePrincipal(endDateTime: dateInPast);
        }

        private static ServicePrincipal GetServicePrincipal(string displayName = null, DateTimeOffset? startDateTime = null, DateTimeOffset? endDateTime = null)
        {
            var servicePrincipalCredential = new ServicePrincipal
            {
                DisplayName = displayName,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };
            return servicePrincipalCredential;
        }
    }
}
