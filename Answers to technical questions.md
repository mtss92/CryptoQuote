## 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

I spent approximately 15 hours working on the coding assignment. During this time, I focused on the following:

- Implementing the core functionalities.
- Running the application using Docker.
- Writing tests focused on business logic and infrastructure services.
- Setting up Serilog and Seq to enhance logging in the future.
- Implementing an Exception Middleware to handle exceptions and display them to the client.

If I had more time, I would focus on the following enhancements, as outlined in the Feature section of the `README`:

- Adding more tests.
- Improving security.
- Enhancing logging.
- Implementing caching for API responses.

## 2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.

In this application I am using `.NET 6.0` and I haven’t worked with the latest version of .NET yet, but I plan to upgrade in the near future as it introduces several features that simplify the development of certain functionalities.

For instance, Rate Limiting, introduced in .NET 7, provides a built-in solution for controlling the number of requests to an application, which is very useful for enhancing the security of APIs.

Additionally, working with StackExchange.Redis has become significantly easier in the newer versions of .NET.

Upgrading to the latest version will enable me to take advantage of these features, improving both the efficiency and functionality of my projects.

But  now I am using some features of `.NET 6.0` for example:

I used one of the features from `.NET 6`, ```DateOnly```, because the API from `exchangeratesapi.io` returns only the date in its response, without the time. This feature allowed me to handle the date separately.

```csharp
public DateOnly Date { get; set; }
```

Additionally, I used global using directives:

```csharp
global using Microsoft.VisualStudio.TestTools.UnitTesting;
```

## 3. How would you track down a performance issue in production? Have you ever had to do this?

To track down a performance issue in production, I would follow a systematic approach:

**Monitor and Identify the Problem**:

I would start by reviewing monitoring tools like `Seq`, to analyze key performance metrics (e.g., response times). Logs and metrics help when and where the issue occurs.

Using structured logging (e.g., `Serilog`) I would trace the slow requests through the application to identify bottlenecks in specific methods, or external API calls.

**Reproduce the Issue**:

If possible, I would try to reproduce the issue in a staging or test environment that mirrors production. 

**Debug and Optimize**:

Once the bottleneck is identified, I would:

- Optimize the affected code or query.
- Scale resources (if necessary).
- Implement caching (e.g., `Redis`) to reduce redundant operations.

**Test and Deploy Fixes**:

After resolving the issue, I would test the fix thoroughly in staging before deploying it to production. Continuous monitoring post-deployment ensures the problem is fully resolved.

## 4. What was the latest technical book you have read or tech conference you have been to? What did you learn?

The latest technical book I read was `C# 12 in a Nutshell`. I particularly enjoyed the section on `Disposal and Garbage Collection`, especially where it introduces the LOH (Large Object Heap) and explains why it's important to be mindful of array capacity when working with collections. Inspired by this, I set an initial capacity for the lists in this project to optimize memory usage:

```csharp
var result = new List<CryptoRate>(foundSymbols.Count);
```

As for the last tech conference I attended, it was about two years ago, and it was focused on Microservices. It gave me a general overview of the concepts and architecture behind microservices.

## 5. What do you think about this technical assessment?

I think this technical assessment was a great opportunity to showcase my skills in .NET, C#, and API development. It allowed me to apply my knowledge in real-world scenarios, especially when it came to tasks like working with Docker, integrating logging tools like Serilog, and optimizing the application.

The challenges were interesting and gave me an opportunity to explore some advanced topics while keeping the project relatively simple. It also helped me reflect on how I can improve areas like security, logging, and performance in future projects.

## 6. Please, describe yourself using JSON.

```json
{
  "name": "Mohammad Tajik",
  "birthYear": 1987,
  "location": "Rey, Tehran",
  "profession": "Software Developer",
  "skills": [
    ".NET",
    "C#",
    "ASP.NET",
    "Angular",
    "CI/CD"
  ],
  "hobbies": [
    "Reading Tech Blogs",
    "Traveling",
    "Playing Chess"
  ],
  "goals": {
    "shortTerm": "Work in a new environment with new challenges.",
    "longTerm": "Become a solutions architect and lead large-scale software projects."
  }
}
```