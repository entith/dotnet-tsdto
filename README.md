This is dotnet cli tool to convert C# POCO classes to TypeScript interfaces.

There are other tools that already do this, but I each had some shortcomings.

- [Typewriter](https://frhagn.github.io/Typewriter/) - This works well but is a Visual Studio plugin, can't use it from CLI or Linux.
- [TypeLITE](http://type.litesolutions.net/) - This seems to be a popular one (I have never used it), but it doesn't support dotnet core.
- [Typescript.Definitions.Tools](https://github.com/originalmoose/Typescript.Definitions.Tools) - I liked this one as it was a dotnet cli tool (no dependence on Visual Studio or Windows) and could be run as part of the build process, however it has since been abandoned and doesn't work with newer versions of dotnet core.

This is currently unrefined as I wrote it to handle my specific use case. If there is interest in it, I would like to grow this project out to support all common use cases with more advanced configuration options. Pull requests gladly welcome.

# Install

Install the following Nuget package: `https://www.nuget.org/packages/Entith.DotNet.TSDto/0.1.0`

You'll also need to add the following `DotNetCliToolReference`: `https://www.nuget.org/packages/Entith.DotNet.TSDto.Tool/0.1.0`

Example snippet of what goes in the `.csproj` file:

```
  <ItemGroup>
    <PackageReference Include="Entith.DotNet.TSDto" Version="0.1.0" />
  </ItemGroup>

  <ItemGroup>
    <DotNetCliToolReference Include="Entith.DotNet.TSDto.Tool" Version="0.1.0" />
  </ItemGroup>
```

# Configuration

You'll need to create a class in your project that implements the `Entith.DotNet.TSDto.ITSDtoSetup` interface:

```
public class TSDtoSetup : ITSDtoSetup
{
    public void Setup(ITSDtoConfig config)
    {
        // output path of ts code
        string path = "ClientApp/app/models.ts";

        // get all types to convert into a collection
        // your can assemble this collection however you want
        ICollection<Type> models = typeof(SomeType).Assembly.GetTypes().Where(t => t.Namespace == "Some.Namespace");

        // pass everything to the provided config object
        config.Add(models, path);
    }
}
```

`config.Add` also accepts an optional 3rd parameter, a delegate function that'll serve as a name generator for the TypeScript interfaces. For example, if all your C# classes end in `Dto` (ex. `class SomeModelDto`), but you want to remove the `Dto` portion in the TypeScript interface (ex. `interface SomeModel`), you can do something like this:

```
config.Add(models, path, t => t.Name.Replace("Dto", ""));
```

You'll then need to run the tool after your build. You add something like the following to your `.csproj` file:

```
<Target Name="PostBuild" AfterTargets="PostBuildEvent">
	<Exec Command="dotnet tsdto $(TargetPath)" />
</Target>
```

The `dotnet tsdto` command itself takes one parameter: the path to your project's built DLL file.

# License

MIT license