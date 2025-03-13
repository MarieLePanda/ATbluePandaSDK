# ATPbluePandaSDK

ATPbluePandaSDK is a .NET library for interacting with BlueSky and the ATP protocol. It provides functionalities to interact with BlueSky’s features and manage user actions through the ATP protocol.

## Features

The current functionalities include:

- **Authentication**: Authenticate with the BlueSky platform.
- **Create a Post**: Post content to BlueSky.
- **Get Timeline**: Fetch your timeline.
- **Custom Timeline**: Fetch a customized timeline.
- **Author Timeline**: Retrieve the timeline author of the connected user.
- **Get Thread**: Retrieve a specific thread.
- **Like / Unlike**: Like or unlike a post.
- **Follow / Unfollow**: Follow or unfollow a user.

## Installation

To install ATPbluePandaSDK, run the following command in your terminal:

```bash
dotnet add package ATPbluePandaSDK --version 0.1.1
```
Or use [NuGet Package Manager in Visual Studio.](https://docs.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio)

## Usage

Here’s a basic example to get started with ATPbluePandaSDK:
```csharp
//Initiate the ATP client
ATPClient client = new ATPClient();

//Get Connected
AuthRequest authRequest = new AuthRequest("User", "password");
AuthUser user = client.Authenticate(authRequest);

//Post on BlueSky
client.CreatePost(authUser, "Hello BlueSky from ATbluePandaSDK.0.1.1");

//Browse the timeline
TimelineResponse timeline = client.GetTimeline(authUser);
foreach(Feed feed in timeline.Feed)
{
    Console.WriteLine(feed.Post.Author.DisplayName);
    Console.WriteLine(feed.Post.Record.Text);

}
```

## License

This project is licensed under the MIT License.
