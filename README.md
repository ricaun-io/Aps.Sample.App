# Aps.Sample.App

[![Autodesk Platform Service](https://img.shields.io/badge/Autodesk%20Platform%20Service-black?logo=autodesk&logoColor=white)](../..)
[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 6.0](https://img.shields.io/badge/.NET%20Core-6.0-blue)](../..)

Desktop application to connect an Autodesk account using [Autodesk Platform Service - Authentication with PKCE](https://aps.autodesk.com/en/docs/oauth/v2/tutorials/get-3-legged-token-pkce/).

![Aps.Sample.App](assets/Aps.Sample.App.gif)

## How to use

### Create an Aps Application

1. Go to [Autodesk Platform Service](https://aps.autodesk.com/).
2. Create an application with the type [Desktop, Mobile, Single-Page App](https://aps.autodesk.com/en/docs/oauth/v2/tutorials/create-app/).
3. Copy the `Client ID` and `Callback URL`.
4. Paste the `Client ID` and `Callback URL` in the [MainWindow.xaml.cs](Aps.Sample.App/MainWindow.xaml.cs) file.

## Video

Videos in English about this project.

[![VideoIma1]][Video1] 

## Release

* [Latest release](../../releases/latest)

## License

This project is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this project? Please [star this project on GitHub](../../stargazers)!

[Video1]: https://youtu.be/O35t2bNLmJk
[VideoIma1]: https://img.youtube.com/vi/O35t2bNLmJk/mqdefault.jpg