

using System.Dynamic;
using System.Reflection.Metadata.Ecma335;

public abstract class ASTNode{
    public IDType Type { get; protected set; }
    public CodeLocation Location { get; protected set; }
    public ASTNode(IDType type, CodeLocation location){
        Type = type;
        Location = location;
    }
    public abstract bool Validate();
    public abstract object Evaluate();
    public bool CheckType(IDType type) => Type == type;
}

public class Number : ASTNode
{
    public double Value { get; private set; }
    public Number(double value, CodeLocation location) : base(IDType.Numeric, location){
        Value = value;
    }
    public override bool Validate() => Type == IDType.Numeric;
    public override object Evaluate() => Value;
}

public class Boolean : ASTNode
{
    public bool Value { get; private set; }
    public Boolean(bool value, CodeLocation location) : base(IDType.Boolean, location){
        Value = value;
    }
    public override bool Validate() => Type == IDType.Boolean;
    public override object Evaluate() => Value;
}

public class Variable : ASTNode
{
    public string Name { get; private set; }
    public ASTNode Value { get; private set; }
    public Variable(string name, ASTNode value) : base(value.Type, value.Location){
        Name = name;
        Value = value;
    }
    public void ChangeValue(ASTNode node){
        Value = node;
    }
    public override bool Validate() => Value.Validate();
    public override object Evaluate() => Value.Evaluate();
}

