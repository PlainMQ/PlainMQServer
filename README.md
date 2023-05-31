<p align="center">
	<img src="imgs/logo.png" width="150" height="200">
</p>

# PlainMQServer

PlainMQ is a multithreaded, wide-broadcast messaging queue. All other optimizations are up the the consumers. PlainMQ will provide you with what only what is necessary - everything else is up to your imagination.

## Building

### Source Code

This current version of PlainMQServer is built using the following:

- Visual Studio 2022
- .Net Core 6

If you have these setup in your environment you should just be able to clone the source and compile straight away.

There are some arguments that are used in the setup of the server. These are:

- "--port=x" _run the server at this specified port_
- "--addr=x" _run ther server at this specified address_
- "--timeout=x" _not used currently_

### Executable

1. Navigate to the [Releases Page](https://github.com/PlainMQ/PlainMQServer/releases)
1. Download the zip file
1. Run the following command `.\PlainMQServer.exe --port={your port} --addr={your address} --timeout={your timeout value}`

### Docker Image

todo

## Contributing

Thank you for scrolling to this section and registering your visual interest. All help is greatly appreciated. To get started check out the Issues tab. There will be some issues labelled as "Entrypoint" which will give you a nice foothold.

All merging to the master branch is done via Pull Requests.

Any further questions can be directed towards the project email or [discord channel](https://discord.com/channels/1113097416082735225/1113097416577658913).

## Consuming

Various consuming clients are in various stages of development, you can access these here:

- [C# Client & Examples](https://github.com/PlainMQ/PlainMQ.Net)
- [GO Client & Examples](https://github.com/PlainMQ/PlainMQ.GO)
- [NodeJS Client & Examples](https://github.com/PlainMQ/PlainMQ.Node)
- [Java Client & Examples](https://github.com/PlainMQ/PlainMQ.Java)

## Contact

Feel free to post a [repository issue](https://github.com/PlainMQ/PlainMQServer/issues) should you have a question or concern about the project or send a friendly message to the organization's email.

## FAQ
