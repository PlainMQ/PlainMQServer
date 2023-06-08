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

#### Windows

1. Navigate to the [Releases Page](https://github.com/PlainMQ/PlainMQServer/releases)
1. Download the zip file and extract
2. Open up your terminal of choice at the extracted location
3. Run the following command `.\PlainMQServer.exe --port={your port} --addr={your address} --timeout={your timeout value}`


## Contributing

Thank you for scrolling to this section and registering your visual interest. All help is greatly appreciated. To get started check out the Issues tab. There will be some issues labelled as "Entrypoint" which will give you a nice foothold.

All merging to the master branch is done via Pull Requests.

Any further questions can be directed towards the project email or discord server chat.

## Consuming

Various consuming clients are in various stages of development, you can access these here:

- [C# Client & Examples](https://github.com/PlainMQ/PlainMQ.Net) | Partial completion
- [GO Client & Examples](https://github.com/PlainMQ/PlainMQ.GO) | to be done
- [NodeJS Client & Examples](https://github.com/PlainMQ/PlainMQ.Node) | Partial completion
- [Java Client & Examples](https://github.com/PlainMQ/PlainMQ.Java) | to be done

## Contact

Please feel free to create an [issue](https://github.com/PlainMQ/PlainMQServer/issues) or add something to the [discussion](https://github.com/PlainMQ/PlainMQServer/discussions)
