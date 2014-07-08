ServiceBusThroughputTester
==========================
The goal of this app is to stress the Azure Service Bus


Command line args:
==================
IsSender - do you want this to send messages to azure service bus

IsReciver - do you want this to get messages from azure service bus

(only one can be true, if both are false then it runs in setup mode, setup mode creates the queue, if needed, and creates the perfmon counters)

ConnectionString - the connection string from teh azure portal

QueueName - the queue you want the messages to go into

MessageSize - the size of the payload that will be sent with each message

TaskCount - the number of service bus clients to spin up in this instance

Express - should this queue have Express turned on

Partioned - should this queue be Partioned

MaxConcurrentCalls -  maximum number of concurrent calls to the callback the message pump should initiate


Perfmon Counter
===============
The tool adds a perfmon counter (ServiceBusTestCategory | # operations / sec). This counter logs one operation for a write and two operations for a read (read and commit).


Sample Command Line Setups
==========================

Setup Command Line
------------------
ServiceBusThroughputTester /IsSender false /IsReciver false /QueueName MyTestQueue /MessageSize 1 /Express true /Partioned true /MaxConcurrentCalls 0 /ConnectionString "from the azure portal"

Sender Command Line
-------------------
ServiceBusThroughputTester /IsSender true /IsReciver false /QueueName MyTestQueue /MessageSize 1 /TaskCount 10 /Express true /Partioned true /MaxConcurrentCalls 0 /ConnectionString "from teh azure portal"


Reciver Command Line
--------------------
ServiceBusThroughputTester /IsSender false /IsReciver true /QueueName MyTestQueue /MessageSize 1 /TaskCount 10 /Express true /Partioned true /MaxConcurrentCalls 1 /ConnectionString "from the azure portal"

