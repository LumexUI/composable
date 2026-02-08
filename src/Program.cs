using System.CommandLine;

using LumexUI.Cli.Commands;

var rootCommand = new RootCommand( "LumexUI component copier - copy components from registry to your project" )
{
	AddCommand.Create(),
	ListCommand.Create()
};

var parseResult = rootCommand.Parse( args );

return await parseResult.InvokeAsync();
