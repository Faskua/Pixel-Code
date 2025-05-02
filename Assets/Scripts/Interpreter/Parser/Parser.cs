using System;
using System.Collections.Generic;
using UnityEngine;

public class Parser 
{
    public Global Global { get; private set; }
    private List<Token> Tokens { get; set;}
    private int pos { get; set; }
    private Token nextToken { get; set; }
    private Wall Wall { get; set; }
    public Parser(Global global){
        Global = global;
    }

    private bool EOF() => pos == Tokens.Count-1;
    private bool CanGo() => pos < Tokens.Count-1;
    private Token NextTokenStay(int k = 1){
        if(CanGo()) return Tokens[pos + k];
        return null;    
    }
    private Token NextTokenMove(){
        if(CanGo()) return Tokens[pos++];
        return null;
    }
    private void LookAhead(List<TokenType>? types = null){
        if(!CanGo()){
            Global.AddError($"Unexpected end of file");
            return;
        }
        if(types == null) nextToken = Tokens[pos+1] ;
        else if(types.Contains(Tokens[pos+1].Type)) nextToken = Tokens[pos+1];
        else{
            Token token = Tokens[pos+1];
            Global.AddError($"UnexpectedToken at line: {token.Location.Line}, columnm: { token.Location.Column}");
        } 
    }
    private void LookAhead(TokenType type){
        List<TokenType> types = new List<TokenType>();
        types.Add(type);
        LookAhead(types);
    }
    private void Consume(TokenType type){
        if(!CanGo()){
            Global.AddError($"Unexpected end of file");
            return;
        }
        if(Tokens[pos+1].Type == type) pos++;
        else{
            Token token = Tokens[pos+1];
            Global.AddError($"Unexpected token at line: {token.Location.Line}, column: {token.Location.Column}");
        }
    }
    private void Consume(List<TokenType> types){
        foreach (TokenType type in types)
        {
            Consume(type);
        }
    }
    public List<Statement> Parse(List<Token> tokens){
        var Statements = new List<Statement>();
        Tokens = tokens;
        pos = -1;
        string currentLabel = "";
        CodeLocation labelLocation = null;
        var LabelStatements = new List<Statement>();
        LookAhead();
        while(pos < Tokens.Count-2){
            Statement current = null;
            switch(nextToken.Type){
                case TokenType.EOL:
                    Consume(TokenType.EOL);
                break;
                case TokenType.EOF:
                    Consume(TokenType.EOF);
                break;
                case TokenType.GoTo:
                    current = ParseGoTo();
                break;
                case TokenType.Identifier:
                    current = ParseVariable();
                break;
                case TokenType.Spawn:
                    current = ParseSpawn();
                break;
                case TokenType.Color:
                    current = ParseColor();
                break;
                case TokenType.Size:
                    current = ParseSize();
                break;
                case TokenType.DrawLine:
                    current = ParseDrawLine();
                break;
                case TokenType.DrawCircle:
                    current = ParseDrawCircle();
                break;
                case TokenType.DrawRectangle:
                    current = ParseDrawRectangle();
                break;
                case TokenType.Fill:
                    current = ParseFill();
                break;
                case TokenType.Label:
                    if(currentLabel != ""){
                        Statement block = new BlockStatement(IDType.Block, labelLocation, LabelStatements);
                        Global.AddLabel(currentLabel, block);
                    }  
                    currentLabel = nextToken.Value;
                    labelLocation = nextToken.Location;
                    LabelStatements = new List<Statement>();
                    Consume(TokenType.Label);
                break;
                default:
                    Debug.Log($"Error en el parse, token: {nextToken.Type}, valor: {nextToken.Value}");
                    Consume(nextToken.Type);
                    Global.AddError($"Unexpected statement at line: {nextToken.Location.Line}, column: {nextToken.Location.Column}");
                break;
                
            }
            if(currentLabel != "") LabelStatements.Add(current);
            if(current != null)Statements.Add(current);
            LookAhead();
        }
        if(currentLabel != ""){
            Statement block = new BlockStatement(IDType.Block, labelLocation, LabelStatements);
            Global.AddLabel(currentLabel, block);
        }
        return Statements;
    }

    #region ParseExpressions
    private Expression ParseNumber(int precedence = 0, bool first = true){
        List<TokenType> operators = new List<TokenType> {TokenType.Plus, TokenType.Minus, TokenType.Mult, TokenType.Div, TokenType.Pow};
        Expression left = null;
        switch(nextToken.Type){
            case TokenType.Int:
                left = new Number(int.Parse(nextToken.Value), nextToken.Location);
                Consume(TokenType.Int);
            break;
            case TokenType.Identifier:
                left = Global.GetVariable(nextToken.Value, nextToken.Location);
                Consume(TokenType.Identifier);
            break;
            default:
                Global.AddError($"Unexpected token at line: {nextToken.Location.Line}, column: {nextToken.Location.Column}");
            break;
        }
        LookAhead();
        int OPprecedence = 0;
        if(operators.Contains(nextToken.Type)){
            if(nextToken.Type == TokenType.Plus || nextToken.Type == TokenType.Minus) OPprecedence = 1;
            else if(nextToken.Type == TokenType.Mult || nextToken.Type == TokenType.Div) OPprecedence = 2;
            else OPprecedence = 3;
        }
        while(precedence < OPprecedence && operators.Contains(nextToken.Type)){
            Token op = nextToken;
            Consume(nextToken.Type);
            LookAhead();
            Expression right = ParseNumber(OPprecedence, false);
            left = new NumericBinaryOperation(left, op, right);
        }
        if(first){
            nextToken = Tokens[pos];
        }   
        return left;
    }
    private Expression ParseBoolean(){
        Expression left = ParseSimpleBoolean();
        LookAhead();
        while(nextToken.Type == TokenType.And || nextToken.Type == TokenType.Or){
            Token op = nextToken;
            Consume(nextToken.Type);
            LookAhead();
            Expression right = ParseSimpleBoolean();
            left = new BooleanBinaryExpression(left, op, right);
            LookAhead();
        }
        nextToken = Tokens[pos];
        return left;
    }
    private Expression ParseSimpleBoolean(){
        if(nextToken.Type == TokenType.True || nextToken.Type == TokenType.False){
            bool value = nextToken.Type == TokenType.True;
            Expression output = new Boolean(value, nextToken.Location);
            Consume(nextToken.Type);
            return output;
        }
        else if(nextToken.Type == TokenType.Int) return ParseNumericalBoolean();
        else{
            Expression variable = Global.GetVariable(nextToken.Value, nextToken.Location);
            if(variable.CheckType(IDType.Numeric)) return ParseNumericalBoolean();
            else{
                if(!variable.CheckType(IDType.Boolean)) Global.AddError($"Unvalid boolean expression at line: {nextToken.Location.Line}, column: {nextToken.Location.Column}");
                Consume(TokenType.Identifier);
                return (variable as Variable).Value;
            } 
        }
    }
    private Expression ParseNumericalBoolean(){
        List<TokenType> NumericalOperators = new List<TokenType> {TokenType.Less, TokenType.Greater,
                                                                  TokenType.Equal, TokenType.LessEqual, TokenType.GreaterEqual};
        Expression left = ParseNumber();
        LookAhead(NumericalOperators);
        Token op = nextToken;
        Consume(nextToken.Type);
        LookAhead(new List<TokenType> {TokenType.Int, TokenType.Identifier});
        Expression right = ParseNumber();
        Expression output = new BooleanBinaryExpression(left, op, right);
        return output;
    }
    private Statement ParseVariable(){
        List<TokenType> DSLExpressions  = new List<TokenType> {
            TokenType.GetActualX, TokenType.GetActualY, TokenType.GetCanvasSize, TokenType.GetColorCount, 
            TokenType.IsBrushColor, TokenType.IsBrushSize, TokenType.IsCanvasColor
        };
        List<TokenType> BooleanOperators = new List<TokenType> {
            TokenType.True, TokenType.False, TokenType.Equal, TokenType.Greater, 
            TokenType.Less, TokenType.GreaterEqual, TokenType.LessEqual, TokenType.And, TokenType.Or
        };
        CodeLocation location = nextToken.Location;
        string name = nextToken.Value;
        Consume(TokenType.Identifier);
        LookAhead(TokenType.Assignation);
        Consume(TokenType.Assignation);
        Expression variable = null;
        LookAhead();
        if(DSLExpressions.Contains(nextToken.Type)){
            switch(nextToken.Type){
                case TokenType.GetActualX:
                    variable = ParseGetActualX();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.GetActualY:
                    variable = ParseGetActualY();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.GetCanvasSize:
                    variable = ParseGetCanvasSize();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.GetColorCount:
                    variable = ParseGetColorCount();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.IsBrushColor:
                    variable = ParseIsBrushColor();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.IsBrushSize:
                    variable = ParseIsBrushSize();
                    variable.Type = IDType.Numeric;
                break;
                case TokenType.IsCanvasColor:
                    variable = ParseIsCanvasColor();
                    variable.Type = IDType.Numeric;
                break;
                default:
                break;
            }
        }
        else{
            int k = 0;
            while(NextTokenStay(k).Type != TokenType.EOL){
                if(BooleanOperators.Contains(NextTokenStay(k).Type)){
                    variable = ParseBoolean();
                    break;
                }
                k++;
            }
            if(variable == null){
                if(nextToken.Type == TokenType.Identifier && NextTokenStay().Type == TokenType.EOL){
                    variable = Global.GetVariable(nextToken.Value, NextTokenStay().Location);
                    Consume(TokenType.Identifier);
                } 
                else variable = ParseNumber();
            }
        } 
        Statement output = new Declaration(IDType.Declaration, location, name, variable);
        output.Evaluate(Global);
        return output;
    }
    private Statement ParseGoTo(){
        CodeLocation location = nextToken.Location;
        Consume(TokenType.GoTo);
        LookAhead(TokenType.LBracket);
        Consume(TokenType.LBracket);
        LookAhead(TokenType.Identifier);
        string label = nextToken.Value;
        Consume(TokenType.Identifier);
        LookAhead(TokenType.RBracket);
        Consume(TokenType.RBracket);
        LookAhead(TokenType.LParen);
        Consume(TokenType.LParen);
        LookAhead();
        Expression condition = ParseBoolean();
        LookAhead(TokenType.RParen);
        Consume(TokenType.RParen);
        Statement output = new GoTo(IDType.GoTo, location, label, condition);
        return output;
    }
    #endregion
    #region ParseDSLExpressions

        private Expression ParseGetActualX(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.GetActualX);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new GetActualXExpression(IDType.GetActualX, location, Wall);
            return output;
        }
        private Expression ParseGetActualY(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.GetActualY);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new GetActualYExpression(IDType.GetActualY, location, Wall);
            return output;
        }
        private Expression ParseGetCanvasSize(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.GetCanvasSize);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new GetCanvasSizeExpression(IDType.GetCanvasSize, location, Wall);
            return output;
        }
        private Expression ParseGetColorCount(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.GetColorCount);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.PixelColor);
            string color = nextToken.Value;
            Consume(TokenType.PixelColor);
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression x1 = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression y1 = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression x2 = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression y2 = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new GetColorCountExpression(IDType.GetColorCount, location, Wall, color, (int)x1.Evaluate(Global), (int)y1.Evaluate(Global), (int)x2.Evaluate(Global), (int)y2.Evaluate(Global));
            return output;
        }
        private Expression ParseIsBrushColor(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.IsBrushColor);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.PixelColor);
            string color = nextToken.Value;
            Consume(TokenType.PixelColor);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new IsBrushColorExpression(IDType.IsBrushColor, location, Wall, color);
            return output;
        }
        private Expression ParseIsBrushSize(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.IsBrushSize);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression size = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new IsBrushSizeExpression(IDType.IsBrushSize, location, Wall, (int)size.Evaluate(Global));
            return output;
        }
        private Expression ParseIsCanvasColor(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.IsCanvasColor);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.PixelColor);
            string color = nextToken.Value;
            Consume(TokenType.PixelColor);
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression vertical = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression horizontal = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Expression output = new IsCanvasColorExpression(IDType.IsCanvasColor, location, Wall, color, (int)vertical.Evaluate(Global), (int)horizontal.Evaluate(Global));
            return output;
        }

    #endregion

    #region ParseDSLStatements

        private Statement ParseSpawn(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.Spawn);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression x = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression y = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new SpawnStatement(IDType.Spawn, location, (int)x.Evaluate(Global), (int)y.Evaluate(Global));
            return output;
        }
        private Statement ParseColor(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.Color);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.PixelColor);
            Expression color = new ColorExpression(nextToken.Value, nextToken.Location);
            Consume(TokenType.PixelColor);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new ColorStatement(IDType.Color, location, (string)color.Evaluate(Global));
            return output;
        }
        private Statement ParseSize(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.Size);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression size = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new SizeStatement(IDType.Size, location, (int)size.Evaluate(Global));
            return output;
        }
        private Statement ParseDrawLine(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.DrawLine);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression dx = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression dy = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression distance = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new LineStatement(IDType.DrawLine, location, (int)dx.Evaluate(Global), (int)dy.Evaluate(Global), (int)distance.Evaluate(Global));
            return output;
        }
        private Statement ParseDrawCircle(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.DrawCircle);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression x = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression y = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression radius = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new CircleStatement(IDType.DrawLine, location, (int)x.Evaluate(Global), (int)y.Evaluate(Global), (int)radius.Evaluate(Global));
            return output;
        }
        private Statement ParseDrawRectangle(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.DrawRectangle);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression dx = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression dy = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression distance = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression heigth = ParseNumber();
            LookAhead(TokenType.Comma);
            Consume(TokenType.Comma);
            LookAhead(new List<TokenType> { TokenType.Identifier, TokenType.Int });
            Expression width = ParseNumber();
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new RectangleStatement(IDType.DrawRectangle, location, (int)dx.Evaluate(Global), (int)dy.Evaluate(Global), (int)distance.Evaluate(Global), (int)heigth.Evaluate(Global), (int)width.Evaluate(Global));
            return output;
        }
        private Statement ParseFill(){
            CodeLocation location = nextToken.Location;
            Consume(TokenType.Fill);
            LookAhead(TokenType.LParen);
            Consume(TokenType.LParen);
            LookAhead(TokenType.RParen);
            Consume(TokenType.RParen);
            Statement output = new FillStatement(IDType.Fill, location);
            return output;
        }

    #endregion
}