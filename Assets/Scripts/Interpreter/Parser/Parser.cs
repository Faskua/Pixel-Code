using System;
using System.Collections.Generic;

public class Parser 
{
    public List<Token> Tokens { get; set;}
    private int pos { get; set; }
    private Token nextToken { get; set; }

    private bool EOF() => pos == Tokens.Count-1;
    private bool CanGo() => pos < Tokens.Count-1;
    private Token NextTokenStay(){
        if(CanGo()) return Tokens[pos + 1];
        return null;    
    }
    private Token NextTokenMove(){
        if(CanGo()) return Tokens[pos++];
        return null;
    }
    private void LookAhead(List<TokenType>? types){
        if(!CanGo()){
            Global.Errors.Add($"Unexpected end of file");
            return;
        }
        if(types == null) nextToken = Tokens[pos+1] ;
        if(types.Contains(Tokens[pos+1].Type)) nextToken = Tokens[pos+1];
        else{
            Token token = Tokens[pos+1];
            Global.Errors.Add($"UnexpectedToken at line: {token.Location.Line}, columnm: { token.Location.Column}");
        } 
    }
    private void LookAhead(TokenType type){
        List<TokenType> types = new List<TokenType>();
        types.Add(type);
        LookAhead(types);
    }
    private void Consume(TokenType type){
        if(!CanGo()){
            Global.Errors.Add($"Unexpected end of file");
            return;
        }
        if(Tokens[pos+1].Type == type) pos++;
        else{
            Token token = Tokens[pos+1];
            Global.Errors.Add($"Unexpected token at line: {token.Location.Line}, column: {token.Location.Column}");
        }
    }
    private void Consume(List<TokenType> types){
        foreach (TokenType type in types)
        {
            Consume(type);
        }
    }
}