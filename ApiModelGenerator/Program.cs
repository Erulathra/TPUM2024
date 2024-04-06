using ClientData;
using ConnectionApi;
using NJsonSchema;
using NJsonSchema.CodeGeneration.CSharp;

namespace ApiModelGenerator;

internal static class Program
{
	private static async Task Main(string[] args)
	{
		Console.WriteLine("JSON Schema: \n\n");
		JsonSchema schema = JsonSchema.FromType<GetItemsCommand>();
		string schemaString = schema.ToJson();
		
		Console.WriteLine(schemaString);
		
		Console.WriteLine("C#: \n\n");

		JsonSchema loadedSchema = await JsonSchema.FromJsonAsync(schemaString);
		CSharpGenerator csharpGenerator = new CSharpGenerator(loadedSchema);
		Console.WriteLine(csharpGenerator.GenerateFile());
	}
}