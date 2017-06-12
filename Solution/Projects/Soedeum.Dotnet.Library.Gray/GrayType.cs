namespace Soedeum.Dotnet.Library.Gray
{
    public enum GrayType
    {
        Error = -1,
        Eof, 
        
        CharacterToken,
        IdentifierToken,
        
        IsDefinedAsToken,
        ParenthesesStartToken,
        ParenthesesEndToken,
        SemicolonToken,

        CharSetKeyword,
        OrKeyword,
        ToKeyword,
        NotKeyword,

        GrayRule,
        DeclarationRule,
        CharSetDeclarationRule,
        CharListRule,
        CharSetRule,
        CharComplimentRule,
        CharRangeRule,
    }
}