using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class Lexxer
{
    public Global Global { get; private set; }
    List<Token> tokens = new List<Token>();
    Regex text = new Regex(@"[_a-zA-Z][_a-zA-Z0-9-]*");
    Regex number = new Regex(@"\d+(\.\d+)?");
    Regex symbol = new Regex(@"(&&|\|\||<-|<=|>=|==|\*\*|[+\-*/%<>,()\[\]])");
    Regex Spaces = new Regex(@"[\s\t]+");
    Regex Invalid = new Regex(@"[;:\\}{]");  
    private string[] Colors = {"Red", "Blue", "Green", "Yellow", "Purple", "Orange", "Black", "White", "Gray", "Pink", "LightBlue", "LightGreen", "LightGray", "Brown", "Transparent"};
    private List<int> Lines = new List<int>();
    public Lexxer(Global global){
        Global = global;
    }

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
                        if (!Colors.Contains(Token) && !Lines.Contains(line))
                        {
                            Global.AddError($"Unvalid color at: {lastMark.line}, column: {lastMark.column}");
                            Lines.Add(line);
                            tokens.Add(new Token("Black", TokenType.PixelColor, new CodeLocation(line+1, column)));
                        }
                        else tokens.Add(new Token(Token, TokenType.PixelColor, new CodeLocation(line+1, column)));
                        Token = "";
                    }
                }
                else if(Marks){
                    Token += Line[column];
                }
                else{
                    if (Spaces.IsMatch(Line[column].ToString())) { }
                    else if (symbol.IsMatch(Line[column].ToString()) || Line[column].ToString() == "&" || Line[column].ToString() == "|" || Line[column].ToString() == "=") { AddToken(symbol.Match(Line, column).Value, line, ref column, TokenType.Unknown); }
                    else if (text.IsMatch(Line[column].ToString()))
                    {
                        if ((column + text.Match(Line, column).Length) >= Line.Length - 1 && column == 0) AddToken(text.Match(Line, column).Value, line, ref column, TokenType.Label);
                        else AddToken(text.Match(Line, column).Value, line, ref column, TokenType.Unknown);
                    }
                    else if (number.IsMatch(Line[column].ToString())) { AddToken(number.Match(Line, column).Value, line, ref column, TokenType.Int); }
                    else if (Invalid.IsMatch(Line[column].ToString()) && !Lines.Contains(line))
                    {
                        Lines.Add(line);
                        Global.AddError($"Invalid character: {Invalid.Match(Line, column).Value}, at line: {lastMark.line}, column: {lastMark.column}");
                    } 
                    
                }
                column++;
            }
            tokens.Add(new Token("", TokenType.EOL, new CodeLocation(line+1, column)));
            line ++;
            column = 0;
        }
        if(Marks) Global.AddError($"Missing clousing quote mark at line: {lastMark.line}, column: {lastMark.column}");
        tokens.Add(new Token("", TokenType.EOF, new CodeLocation(Splited.Length+1, 1)));
        return tokens;
    }

    public void AddToken(string match, int line, ref int column, TokenType type){
        if(type != TokenType.Unknown)   tokens.Add(new Token(match, type, new CodeLocation(line+1, column)));
        else if(!Token.Types.ContainsKey(match)) tokens.Add(new Token(match, TokenType.Identifier, new CodeLocation(line+1, column)));
        else{
            TokenType actualType = Token.Types[match];
            tokens.Add(new Token(match, actualType, new CodeLocation(line+1, column)));
        }
        
        column += match.Length - 1;
    }
}