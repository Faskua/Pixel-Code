using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Linq;

public class Lexxer
{
    List<Token> tokens = new List<Token>();
    Regex text = new Regex(@"[_a-zA-Z][_a-zA-Z0-9-]*");
    Regex number = new Regex(@"\d+(\.\d+)?");
    Regex symbol = new Regex(@"(<-|<=|>=|==|\*\*|&&|\|\||[+\-*/%<>,()\[\]])");
    Regex Spaces = new Regex(@"[\s|\t]+");
    Regex Invalid = new Regex(@"(=|;|:|\\|}|{|_|)");
    private string[] Colors = {"Red", "Blue", "Green", "Yellow", "Purple", "Orange", "Black", "White", "Transparent"};

    public List<Token> Tokenize(string input){
        tokens.Clear();
        string[] Splited = input.Split('\n');
        string Token = "";
        bool Marks = false;
        int line = 0;
        int column = 0;
        (int line, int column) lastMark = (0,0);

        while(line < Splited.Length){
            string Line = Splited[line];
            while ( column < Line.Length){
                if(Line[column] == '"'){
                    Marks = !Marks;
                    lastMark = (line+1, column+1);
                    if(!Marks){
                        if(!Colors.Contains(Token))  throw new Exception($"Unvalid color at: {lastMark.line}, column: {lastMark.column}");
                        tokens.Add(new Token(Token, TokenType.PixelColor, new CodeLocation(line, column)));
                        Token = "";
                    }
                }
                else if(Marks){
                    Token += Line[column];
                }
                else{
                    if(Spaces.IsMatch(Line[column].ToString())){}
                    else if(text.IsMatch(Line[column].ToString())){
                        if((column + text.Match(Line, column).Length) >= Line.Length-1) AddToken(text.Match(Line, column).Value, line, ref column, TokenType.Label);
                        else    AddToken(text.Match(Line, column).Value, line, ref column, TokenType.Unknown);
                    } 
                    else if(number.IsMatch(Line[column].ToString())) {AddToken(number.Match(Line, column).Value, line,ref column, TokenType.Int);}
                    else if(symbol.IsMatch(Line[column].ToString())) {AddToken(symbol.Match(Line, column).Value, line, ref column, TokenType.Unknown);}
                    else if(Invalid.IsMatch(Line[column].ToString())) throw new Exception($"Invalid character: {Invalid.Match(Line, column).Value}, at line: {lastMark.line}, column: {lastMark.column}");
                }
                column++;
            }
            line ++;
            column = 0;
        }
        if(Marks) throw new Exception($"Missing clousing quote mark at line: {lastMark.line}, column: {lastMark.column}");
        return tokens;
    }

    public void AddToken(string match, int line, ref int column, TokenType type){
        if(type != TokenType.Unknown)   tokens.Add(new Token(match, type, new CodeLocation(line, column)));
        else if(!Token.Types.ContainsKey(match)) tokens.Add(new Token(match, TokenType.Identifier, new CodeLocation(line, column)));
        else{
            TokenType actualType = Token.Types[match];
            tokens.Add(new Token(match, actualType, new CodeLocation(line, column)));
        }
        
        column += match.Length - 1;
    }
}