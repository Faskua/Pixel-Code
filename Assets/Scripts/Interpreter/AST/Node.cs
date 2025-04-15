

public abstract class ASTNode{
    public IDType Type { get; protected set; }
    public CodeLocation Location { get; protected set; }
    public ASTNode(IDType type, CodeLocation location){
        Type = type;
        Location = location;
    }
    public abstract bool Validate();
    public bool CheckType(IDType type) => Type == type;
}



