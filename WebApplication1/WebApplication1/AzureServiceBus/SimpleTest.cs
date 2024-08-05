using System.Reflection;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace WebApplication1.AzureServiceBus;

public class SimpleTest
{
    public readonly string _connectString = "Endpoint=sb://lzzzmm.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tfxiSuuX8A4ImWarqT1f/QvVqPro9j93u+ASbLrDscg=";


    public async Task QueuePublish()
    {
        await using var queueClient = 
            new ServiceBusClient(_connectString);

        var sender = queueClient.CreateSender("azure_queue1");
        string messageBody = "hello";
        ServiceBusMessage message = new ServiceBusMessage(messageBody);
        
        await sender.SendMessageAsync(message);
        Console.WriteLine($"Sending message: {messageBody} success");

       
    }
    
    public async Task TopicPublish()
    {
        await using var queueClient = 
            new ServiceBusClient(_connectString);
        
        var sender = queueClient.CreateSender("topic_1");
        string messageBody = "hello topic";
        ServiceBusMessage message = new ServiceBusMessage(messageBody);
        
        await sender.SendMessageAsync(message);
        Console.WriteLine($"Sending message: {messageBody} success");
    }

    public async Task ConsumerQueueMessage()
    {
        await using var queueClient = 
            new ServiceBusClient(_connectString);
        
        var receiver = queueClient.CreateReceiver("azure_queue1");
      
        
        var peekMessagesAsync = await receiver.PeekMessagesAsync(100);
        
       
        var receivedMessage = await receiver.ReceiveMessageAsync();

        
        
        await receiver.CompleteMessageAsync(receivedMessage);
        
        string body = receivedMessage.Body.ToString();
        Console.WriteLine(body);
    }
    
    public async Task ConsumerTopicMessage()
    {
        await using var queueClient = 
            new ServiceBusClient(_connectString);
        
        ServiceBusReceiver receiver = queueClient.CreateReceiver("topic_1", "S1");
        
        var receivedMessage = await receiver.ReceiveMessageAsync();

        await receiver.CompleteMessageAsync(receivedMessage);
        
        string body = receivedMessage.Body.ToString();
        Console.WriteLine(body);
    }

    public async Task CreateQueue()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        var options = new CreateQueueOptions("api_test_queue")
        {
            AutoDeleteOnIdle = TimeSpan.FromDays(7),
            DefaultMessageTimeToLive = TimeSpan.FromDays(2),
            DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(1),
            EnableBatchedOperations = true,
            DeadLetteringOnMessageExpiration = true,
            EnablePartitioning = false,
            ForwardDeadLetteredMessagesTo = null,
            ForwardTo = null,
            LockDuration = TimeSpan.FromSeconds(45),
            MaxDeliveryCount = 8,
            MaxSizeInMegabytes = 2048,
            RequiresDuplicateDetection = true,
            RequiresSession = true,
            UserMetadata = "some metadata"
        };

        options.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
            "allClaims",
            new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

        QueueProperties createdQueue = await client.CreateQueueAsync(options);
    }

    public async Task CreateTopicAndSubscription()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        var topicOptions = new CreateTopicOptions("api_topic")
        {
            AutoDeleteOnIdle = TimeSpan.FromDays(7),
            DefaultMessageTimeToLive = TimeSpan.FromDays(2),
            DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(1),
            EnableBatchedOperations = true,
            EnablePartitioning = false,
            MaxSizeInMegabytes = 2048,
            RequiresDuplicateDetection = true,
            UserMetadata = "some metadata"
        };

        topicOptions.AuthorizationRules.Add(new SharedAccessAuthorizationRule(
            "allClaims",
            new[] { AccessRights.Manage, AccessRights.Send, AccessRights.Listen }));

        TopicProperties createdTopic = await client.CreateTopicAsync(topicOptions);

        string subscriptionName = "S2";
        var subscriptionOptions = new CreateSubscriptionOptions("api_topic", subscriptionName)
        {
            AutoDeleteOnIdle = TimeSpan.FromDays(7),
            DefaultMessageTimeToLive = TimeSpan.FromDays(2),
            EnableBatchedOperations = true,
            UserMetadata = "some metadata"
        };
        SubscriptionProperties createdSubscription = await client.CreateSubscriptionAsync(subscriptionOptions);
    }

    public async Task DeleteQueue()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        await client.DeleteQueueAsync("api_queue");
    }
    
    public async Task DeleteTopic()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        await client.DeleteTopicAsync("api_topic");
    }
    
    public async Task UpdateQueue()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        QueueProperties queue = await client.GetQueueAsync("api_queue");
        queue.AutoDeleteOnIdle = TimeSpan.FromSeconds(60);
        // 死信
        queue.DeadLetteringOnMessageExpiration = true;
        QueueProperties updatedQueue = await client.UpdateQueueAsync(queue);
    }
    
    public async Task UpdateTopic()
    {
        var client = new ServiceBusAdministrationClient(_connectString);
        TopicProperties topic = await client.GetTopicAsync("api_topic");
        
        topic.UserMetadata = "some metadata";
        TopicProperties updatedTopic = await client.UpdateTopicAsync(topic);
        
        SubscriptionProperties subscription = await client.GetSubscriptionAsync("api_topic", "api_sub");

        subscription.UserMetadata = "some metadata";
        SubscriptionProperties updatedSubscription = await client.UpdateSubscriptionAsync(subscription);
    }

    public async Task DeleteMessages()
    {
        string queueName = "azure_queue1";

        var client = new ServiceBusClient(_connectString);
        var receiver = client.CreateReceiver(queueName);

        Type myClassType = typeof(ServiceBusReceiver);
        
        // 獲取 internal 方法的信息
        MethodInfo methodInfo = myClassType.GetMethod("PurgeMessagesAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        
        object[] parameters = new object[] { null, CancellationToken.None };

        var result = await (Task<int>)methodInfo.Invoke(receiver, parameters);
        Console.WriteLine($"Result: {result}");
    }
}