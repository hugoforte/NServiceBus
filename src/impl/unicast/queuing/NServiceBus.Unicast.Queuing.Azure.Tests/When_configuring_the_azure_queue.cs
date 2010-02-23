using Microsoft.WindowsAzure.StorageClient;
using NBehave.Spec.NUnit;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Unicast.Queuing.Azure.Config;
using NUnit.Framework;

namespace NServiceBus.Unicast.Queuing.Azure.Tests
{
    [TestFixture]
    public class When_configuring_the_azure_queue
    {
        [Test]
        public void The_storage_should_default_to_dev_settings_if_no_config_section_is_found()
        {

            Configure.With()
                .SpringBuilder()
                .CustomConfigurationSource(new NullSource())
                .AzureMessageQueue();

            Configure.Instance.Builder.Build<CloudQueueClient>().Credentials.AccountName.ShouldEqual("devstoreaccount1");
        }


        [Test]
        public void Storage_setting_should_be_read_from_configuration_source()
        {

            Configure.With()
                .SpringBuilder()
                .AzureMessageQueue();

            var storage = Configure.Instance.Builder.Build<CloudQueueClient>();

            storage.Credentials.AccountName.ShouldEqual("myaccount");
        }

        [Test]
        public void The_azurequeue_should_be_singleton()
        {
            Configure.With()
             .SpringBuilder()
             .AzureMessageQueue();

            Configure.Instance.Builder.Build<AzureMessageQueue>()
                .ShouldBeTheSameAs(Configure.Instance.Builder.Build<AzureMessageQueue>());
        }
    }

    public class NullSource : IConfigurationSource
    {
        T IConfigurationSource.GetConfiguration<T>()
        {
            return null;
        }
    }
} 