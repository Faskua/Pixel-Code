using System;

public abstract class BinaryExpression : Expression
{
    public Expression Left;
    public Token Operation;
    public Expression Right;

    public BinaryExpression(Expression left, Token  op, Expression right, IDType type) : base(type, left.Location){
        Left = left;
        Operation = op;
        Right = right;
    } 
}

public class NumericBinaryOperation : BinaryExpression
{
    public NumericBinaryOperation(Expression left, Token op, Expression right) : base(left, op, right, IDType.Numeric) {}
    public override bool Validate(Global Global){
        bool types = Left.Validate(Global) && Right.Validate(Global);
        string op = Operation.Value;
        bool operation = op == "+" || op == "-" || op == "*" || op == "/" || op == "**" || op == "%";
        return types && operation;
    }

    public override object Evaluate(Global Global)
    {
        if(!Validate(Global)) Global.AddError($"An error ocured at line: {Location.Line}, column: {Location.Column}");
        int left = (int)Left.Evaluate(Global);
        int right = (int)Right.Evaluate(Global);
        switch (Operation.Value)
        {
            case("-"):
                return left - right;
            case("+"):
                return left + right;
            case("*"):
                return left * right;
            case("/"):
                if(right== 0) Global.AddError($"Attempt to divide by zero at line: {Right.Location.Line}, column: {Right.Location.Column}"); //esto hay que cambiarlo para guardar los errores
                return left / right;
            case("**"):
                return Math.Pow(left, right);
            case("%"):
                return left % right;
            default:
                return left + right;
        }
    }
}

public class BooleanBinaryExpression : BinaryExpression
{
    public BooleanBinaryExpression(Expression left, Token op, Expression right) : base(left, op, right, IDType.Boolean) {}
    public override bool Validate(Global Global)
    {
        string op = Operation.Value;
        bool output = true;
        if(op == "&&" || op == "||") output = Left.CheckType(IDType.Boolean, Global) && Right.CheckType(IDType.Boolean, Global);
        
        if(op == ">" || op == "<" || op == ">=" || op == "<=" || op == "==") output = Left.CheckType(IDType.Numeric, Global) && Right.CheckType(IDType.Numeric, Global);

        return output && Left.Validate(Global) && Right.Validate(Global);
    }
    public override object Evaluate(Global Global)
    {
//        if(!Validate(Global)) Global.AddError($"An error ocured at line: {Location.Line}, column {Location.Column}");
        string operation = Operation.Value;
        if(operation == "&&" || operation == "||"){
            bool left = (bool)Left.Evaluate(Global), right = (bool)Right.Evaluate(Global);
            switch(operation){
                case("&&"):
                    return left && right;
                default:
                    return left || right;
            }
        }
        else{
            int left = (int)Left.Evaluate(Global), right = (int)Right.Evaluate(Global);
            switch (operation)
            {
                case(">"):
                    return left > right;
                case("<"):
                    return left < right;
                case("=="):
                    return left == right;
                case(">="):
                    return left >= right;
                default:
                    return left <= right;
            }
        }
    }
}