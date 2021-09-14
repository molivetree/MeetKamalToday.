﻿using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueueController : ControllerBase
    {
        static string connectionString = "Endpoint=sb://mysbnamespace202109.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=175AbY859MPka+sxeg3WDPjNAbBhdkfybn0uyVA41yw=";
        static string queueName = "the-queue";
        static ServiceBusClient client;
        static ServiceBusSender sender;

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            client = new ServiceBusClient(connectionString);
            sender = client.CreateSender(queueName);
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            messageBatch.TryAddMessage(new ServiceBusMessage(JsonConvert.SerializeObject(new { id })));

            try
            {
                await sender.SendMessagesAsync(messageBatch);
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
            return Ok();
        }
    }
}