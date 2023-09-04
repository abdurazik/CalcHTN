using NCDK;
using NCDK.SMARTS;
using System.Text.RegularExpressions;
using NCDK.Isomorphisms;
using MoleculeClassifications;
using NCDK.Graphs;



public class HTSGenerator
{
    private PartNumberHandler _pn; // Object contains HTSCategory/Structural information
    private string _smiles;
    private IAtomContainer _molecule;

    public HTSGenerator(string pn, string smiles)
    {
        _pn = new PartNumberHandler(pn);
        _pn.EvaluateForCategory();
        _smiles = smiles;
        _molecule = Chem.MolFromSmiles(smiles);
    }
    static bool isHydrocarbon(string smiles, IAtomContainer mol)
    {
        Pattern p1 = SmartsPattern.Create("C[!F&!Br&!Cl&!I&!N&!S&!C]"); // Carbon bonded to anything NOT F,Br,Cl,I,N,S cannot be considered hydrocarbon 
        
        if (p1.Matches(mol)) return false;
        
        if(SmartsPattern.Create("C~O").Matches(mol)) return false; // Carbon cannot be bonded to Oxygen
        
        if (smiles.Contains('N'))
        {
            // If an organic molecule containes Nitrogen it may be considered a hydrocarbon if and only if
            // It contains a nitroso or nitrate group.
            
            Pattern amines = Pattern.CreateSubstructureFinder(Chem.MolFromSmiles("CNC"));
            if (amines.Matches(mol)) return false;
        
            int NitrogenPatternHits = SmartsPattern.Create("[$(C[N+](=O)[O-]),$(CN=O)]").MatchAll(mol).GetUniqueAtoms().Count();
            int NitrogenCount = (from charachter in smiles
                                where charachter == 'N'
                                select charachter).ToArray().Length;
            if (NitrogenCount != NitrogenPatternHits) return false;
            
            
        }
        if (smiles.Contains('S'))
        {
            // If an organic molecule contains Sulfure it may be considered a hydrocarbon if and only if
            // It contains a sulphate or sulphone or sulphonic acid or sulphonyl halide 
            SmartsPattern allowedSulfurPattern = SmartsPattern.Create("[#6]S(=O)(=O)[F,Br,Cl,I,O,O-,C]");
            int SulfurCount = new Regex(@"(S(?![a-z]))").Matches(smiles).Count;
            int SubstructurePatternHits = allowedSulfurPattern.MatchAll(mol).GetUniqueAtoms().Count();
            
        
            if(SubstructurePatternHits != SulfurCount) return false;
        }
        
        return true;
    }
    public void CyclicCheck(out bool isCyclic)
    {
        isCyclic = true;
        switch (_pn.GetStructureCode())
        {
            case StructureCode.ALKANES:
                isCyclic = false;
                break;
                
            case StructureCode.CYCLOALKANES:
                isCyclic = true;
                break;
                
            case StructureCode.ALKENES:
                isCyclic = false;
                break;
                
            case StructureCode.CYCLOALKENES:
                isCyclic = true;
                break;
                
            case StructureCode.ALKYNES:
                if (Cycles.FindAll(_molecule).GetNumberOfCycles() > 0) isCyclic = true;
                else isCyclic = false;
                break;
                
            case StructureCode.AROMATICS:
                isCyclic = true;
                break;
                
            case StructureCode.ARYL_ALKANES:
                isCyclic = true;
                break;
                
            case StructureCode.ARYL_TRIFLUOROMETHYL:
                isCyclic = true;
                break;
                
            case StructureCode.ARYL_ALKENES_ALKYNES:
                isCyclic = true;
                break;
            
            case StructureCode.HETROCYCLIC:
                isCyclic = true;
                break;
        }
    }
    public void HydrocarbonCalculation()
    {
        bool isCyclic ; // Because ALKYNES may be cyclic this var is used for structure identification
        CyclicCheck(out isCyclic);
        
        if (isCyclic)
        {

        }
        
        else if (!isCyclic)
        {

        }
    }
    
    public void CalculateTarrifCode()
    {
        if (_pn.MultipleCategories())
        {
            //Logic when PN's amibiguous for what HTN section to calculate for
        }
        else
        {
            HTSCategory category = _pn.GetCategoryArray()[0];
            switch (category)
            {
                case HTSCategory.I_HYDROCARBONS:
                    HydrocarbonCalculation();
                    break;
                
                default:
                    Console.WriteLine("[ERROR] Currently no support for calculation for HTS Category {0}", category);
                    Environment.Exit(2);
                    break;
            }
        }
    }
}