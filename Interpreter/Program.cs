using System.IO;

public class Program{
    public static void Main(){
        Lexer lexer = new Lexer();
        string input = File.ReadAllText(@"D:\Universidad\Programación\Project Wall-E\INPUT.txt");
        var tokens = lexer.Tokenize(input);

        foreach (var token in tokens)
        {
            Console.WriteLine($"Tipo: {token.Type}, Valor: {token.Value}, Linea: {token.Location.Line}, Columna: {token.Location.Column}");
        }
    }
}