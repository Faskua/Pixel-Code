using System;

public abstract class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public Token Operation { get; private set; }
    public Expression Right { get; private set; }

    public BinaryExpression(Expression left, Token  op, Expression right, IDType type) : base(type, left.Location){
        Left = left;
        Operation = op;
        Right = right;
    } 
}

public class NumericBinaryOperation : BinaryExpression
{
    public NumericBinaryOperation(Expression left, Token op, Expression right) : base(left, op, right, IDType.Numeric) {}
    public override bool Validate(){
        bool types = Left.Validate() && Right.Validate();
        string op = Operation.Value;
        bool operation = op == "+" || op == "-" || op == "*" || op == "/" || op == "**" || op == "%";
        return types && operation;
    }

    public override object Evaluate()
    {
        if(!Validate()) Global.AddError($"An error ocured at line: {Location.Line}, column: {Location.Column}");
        int left = (int)Left.Evaluate();
        int right = (int)Right.Evaluate();
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
    public override bool Validate()
    {
        string op = Operation.Value;
        bool output = true;
        if(op == "&&" || op == "||") output = Left.CheckType(IDType.Boolean) && Right.CheckType(IDType.Boolean);
        
        if(op == ">" || op == "<" || op == ">=" || op == "<=" || op == "==") output = Left.CheckType(IDType.Numeric) && Right.CheckType(IDType.Numeric);

        return output && Left.Validate() && Right.Validate();
    }
    public override object Evaluate()
    {
        if(!Validate()) Global.AddError($"An error ocured at line: {Location.Line}, column {Location.Column}");
        string operation = Operation.Value;
        if(operation == "&&" || operation == "||"){
            bool left = (bool)Left.Evaluate(), right = (bool)Right.Evaluate();
            switch(operation){
                case("&&"):
                    return left && right;
                default:
                    return left || right;
            }
        }
        else{
            int left = (int)Left.Evaluate(), right = (int)Right.Evaluate();
            switch (operation)
            {
                case(">"):
                    return left > right;
                case("<"):
                    return left < right;
                case(">="):
                    return left >= right;
                default:
                    return left <= right;
            }
        }
    }
}