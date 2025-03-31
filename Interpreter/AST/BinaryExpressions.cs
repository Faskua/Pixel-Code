

using System.ComponentModel.DataAnnotations;
using System.Dynamic;

public abstract class BinaryExpression : ASTNode
{
    public ASTNode Left { get; private set; }
    public Token Operation { get; private set; }
    public ASTNode Right { get; private set; }

    public BinaryExpression(ASTNode left, Token  op, ASTNode right, IDType type) : base(type, left.Location){
        Left = left;
        Operation = op;
        Right = right;
    } 
}

public class NumericBinaryOperation : BinaryExpression
{
    public NumericBinaryOperation(ASTNode left, Token op, ASTNode right) : base(left, op, right, IDType.Numeric) {}
    public override bool Validate(){
        bool types = Left.CheckType(IDType.Numeric) && Right.CheckType(IDType.Numeric);
        string op = Operation.Value;
        bool operation = op == "+" || op == "-" || op == "*" || op == "/" || op == "**" || op == "%";
        return types && operation;
    }

    public override object Evaluate()
    {
        if(!Validate()) throw new Exception($"An error ocured at line: {Location.Line}, column: {Location.Column}");
        double left = (double)Left.Evaluate();
        double right = (double)Right.Evaluate();
        switch (Operation.Value)
        {
            case("-"):
                return left - right;
            case("*"):
                return left * right;
            case("/"):
                if(right== 0) throw new Exception($"Attempt to divide by zero at line: {Right.Location.Line}, column: {Right.Location.Column}"); //esto hay que cambiarlo para guardar los errores
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
    public BooleanBinaryExpression(ASTNode left, Token op, ASTNode right) : base(left, op, right, IDType.Boolean) {}
    public override bool Validate()
    {
        string op = Operation.Value;
        if(op == "&&" || op == "||") return Left.CheckType(IDType.Boolean) && Right.CheckType(IDType.Boolean);
        
        if(op == ">" || op == "<" || op == ">=" || op == "<=") return Left.CheckType(IDType.Numeric) && Right.CheckType(IDType.Numeric);

        return false;
    }
    public override object Evaluate()
    {
        if(!Validate()) throw new Exception($"An error ocured at line: {Location.Line}, column {Location.Column}");
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
            double left = (double)Left.Evaluate(), right = (double)Right.Evaluate();
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