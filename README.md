# eChartiFy

[![Build status](https://ci.appveyor.com/api/projects/status/c2oil2sqns671djd?svg=true)](https://ci.appveyor.com/project/shunjid/echartify)
**eChartiFy** is web application built with .net core 2.2 that has come with the idea of reading a data-set of Bangladesh (National Elections Data-set) in a fastest possible time and then representing the data-set with different statistical charts. **Overview**:

  - An authorized person will be uploading the election dataset (.xlsx) into the system 
  - Can manipulate uploaded data through dataTable
  - Non-authenticated (anonymous users) can explore election results in different charts (pie, doughnut, bar-horizontal, bar-vertical, dynamic-tabs & cards)
 
##### Cool Features!

  - You can download the charts as PNG file
  - Elegant, material-design charts with user-friendly report representation 

### Installation

 **eChartiFy** requires [.NET Core](https://dotnet.microsoft.com/download) v2.2+ to run. Install the dotnet core-sdk and launch the project. **For example:** 
 To install dotnet core-sdk 2.2 in Ubuntu 18.04 try this in your terminal:

```sh
$ wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
$ sudo dpkg -i packages-microsoft-prod.deb
$ sudo add-apt-repository universe
$ sudo apt-get install apt-transport-https
$ sudo apt-get update
$ sudo apt-get install dotnet-sdk-2.2
```

Now move onto project folder and run this commands to build the project:
```sh
$ cd eChartiFy
$ dotnet restore
$ dotnet run
```
If your see this message below then you have successfully build the project in your system. If you see any error the submit a new [issue here](https://github.com/TeamTigers-IT/eChartiFy/issues). Otherwise, open a browser in your system and browse to [localhost:5000](localhost:5000)
```
Hosting environment: Production
Content root path: /home/username/foldername/eChartiFy
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

### Tech & Resources

**eChartiFy** uses a number of open source projects to work properly:

* [.NET Core 2.2](https://dotnet.microsoft.com/learn/dotnet/hello-world-tutorial/intro) - a cross-platform version of .NET for building websites, services, and console apps!
* [NPOI Core](https://www.nuget.org/packages/DotNetCore.NPOI/) - a library for reading and writing Microsoft Office binary and OOXML file formats.
* [Materializecss](https://materializecss.com) - a modern responsive front-end framework based on Material Design
* [ChartJS](https://www.chartjs.org/) -  flexible JavaScript charting for designers & developers
* [National Elections Dataset](http://www.ecs.gov.bd/page/election-results) -  National Election Results dataset provided by Election comission of the People Republic of Bangladesh and collected from this [open source](https://github.com/mbaldassaro/bangladeshelectiondata)

### Screenshots

<table>
    <tr>
        <td align="center">
        <p align="center">
        <img src="https://github.com/TeamTigers-IT/eChartiFy/blob/master/wwwroot/img/screenshots/combined.png" width="600" />
        </p>
        </td>
    </tr>
    <tr>
        <td align="center">
        <p align="center">
        <img src="https://github.com/TeamTigers-IT/eChartiFy/blob/master/wwwroot/img/screenshots/districtWise.png" width="600" />
        </p>
        </td>
  </tr>
  <tr>
        <td align="center">
        <p align="center">
        <img src="https://github.com/TeamTigers-IT/eChartiFy/blob/master/wwwroot/img/screenshots/yearly.png" width="600" />
        </p>
        </td>
  </tr>

</table>
